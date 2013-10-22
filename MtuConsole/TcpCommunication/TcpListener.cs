using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Data;
using MtuConsole.Common;
using FunctionLib;

namespace MtuConsole.TcpCommunication
{

   
    public class SocketListener 
    {
        private string[] RECEIVE_HEADS = new string[] { "!", "*", "$", "~", "?" };
        IPAddress ip;
        IPEndPoint endPoint;
        TcpListener serv;
        private bool key = false;
        private bool _bNeedReply;
        private MtuLog _logger;
        string _receivedString;
        byte[] result = new byte[1024];
        public ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        private DataTable sendTable;
        private Hashtable _checkTimeList = new Hashtable();
        private int _addDay;
        private int _addSecond;

        private IResponseMessage _responseMessageObj;
        private Int64 _countlisten;
        public event CommnicationMessageHandler MessageArrived;
        public event MessageSentHandler MessageSent;

        // private Hashtable _handleTable;
        private Timer _checkClockTimer;

        private List<StateObject> _handleLists;

        private Hashtable _receiveMsgs;

        /// <summary>
        /// 每个ｒｔｕ，最末断包表
        /// </summary>
        private Hashtable _lastheadList;
        private MyPerformanceCounter myperformancecounter = new MyPerformanceCounter();

        // public SocketListener(int portid,IResponseMessage objmsg)
        public void CreateInstance(int portid, IResponseMessage objmsg)
        {
            _countlisten = 0;
            //_handleTable = new Hashtable();
            _handleLists = new List<StateObject>();
            _receiveMsgs = new Hashtable();
            _lastheadList = new Hashtable();
            _logger = new MtuLog();
            _responseMessageObj = objmsg;
            InitialSendTable();
        
            endPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), portid);
 
            serv = new TcpListener(endPoint);
            CreateCheckTimer();

            _logger.Debug("TcpListener:["+ portid.ToString() + "]");
        }

        /// <summary>
        /// 检查数据列表，并对超时未收齐数据发送回复
        /// </summary>
        public void MultiDataCheck()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(500);
                    List<string> keys = new List<string>();
                    foreach (DictionaryEntry de in _receiveMsgs)
                    {
                        sReceiveEntity item = (sReceiveEntity)de.Value;
                        TimeSpan timspan = DateTime.Now - item.LastReceiveTime;
                        if (item.AllNum > item.CollectNums.Count)
                        {//有遗漏
                            if (timspan.TotalSeconds > 30)
                            {
                                string rtuidandframecount = de.Key.ToString().Substring(1);

                                List<int> missnums = new List<int>();
                                for (int i = 1; i <= item.AllNum; i++)
                                {
                                    if (!item.CollectNums.Contains(i))
                                    {
                                        missnums.Add(i);
                                    }
                                }

                                SendMissDataCommand(item.Head, rtuidandframecount, missnums);
                                keys.Add(de.Key.ToString());
                            }
                        }
                        else
                        { //无遗漏
                            SendDataFullReply(item.Head, de.Key.ToString().Substring(1, 10));
                            keys.Add(de.Key.ToString());
                        }
                    }
                    lock (_receiveMsgs)
                    {
                        foreach (string removekey in keys)
                        {
                            if (_receiveMsgs.Contains(removekey))
                            {
                                _receiveMsgs.Remove(removekey);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                }

            }
        }

        /// <summary>
        /// 向终端发送缺失数据桢回复
        /// </summary>
        /// <param name="rtuid"></param>
        private void SendMissDataCommand(string head, string rtuidandframecount, List<int> missnums)
        {
            try
            {
                //查找_handleLists，符合rtuid的client，用它发送缺失数据
                StateObject resultobj = _handleLists.Find(delegate(StateObject x) { if (x.RtuId == rtuidandframecount.Substring(0, 10)) { return true; } else { return false; } });
                //使用_responseMessageObj获取下发data

                string data = _responseMessageObj.GetMultiDataMissResponse(head, rtuidandframecount.Substring(0, 10), missnums);


                if (resultobj != null)
                {
                    _logger.Debug("Send NewDLA MissData Msg:" + data);
                    Send(resultobj.WorkSocket, data);

                }
                else
                {
                    _logger.Debug("not Send newDLA MissData Msg:" + data);
                }

            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        private void SendDataFullReply(string head, string rtuid)
        {
            try
            {
                StateObject resultobj = _handleLists.Find(delegate(StateObject x) { if (x.RtuId == rtuid) { return true; } else { return false; } });
                string data = _responseMessageObj.GetFullDataResponse(head, rtuid);
                if (resultobj != null)
                {
                    _logger.Debug("Send NewDLA Full Data:" + data);
                    Send(resultobj.WorkSocket, data);

                }
                else
                {
                    _logger.Debug("not NewDLA Full Data:" + data);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        private void UpdateReceiveMsgs(string content)
        {
            try
            {
                string head = content.Substring(0, 1);
                if ((head == "*" || head == "!") && content.Length > 17)
                {
                    string keystr = head + content.Substring(3, 10) + content.Substring(13, 1);
                    if (_receiveMsgs.Contains(keystr))
                    {
                        sReceiveEntity getobj = (sReceiveEntity)_receiveMsgs[keystr];
                        getobj.LastReceiveTime = DateTime.Now;

                        string tempstr = content.Substring(14, 1); //当前帧号

                        int frameid = (int)tempstr.ConvertFrom62();//   Convert.ToInt32(tempstr, 16);

                        if (!getobj.CollectNums.Contains(frameid))
                        {
                            getobj.CollectNums.Add(frameid);
                        }


                    }
                    else
                    {
                        //_responseMessageObj  获取该content F1类型的sReceiveEntity结构
                        // sReceiveEntity setobj = new sReceiveEntity();
                        sReceiveEntity setobj = _responseMessageObj.GetMultidataStruct(content);

                        if (setobj.AllNum > 1)
                        {
                            _receiveMsgs.Add(keystr, setobj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        /// <summary>
        /// 设置校时list
        /// </summary>
        public void SetCheckTimeList(List<string> rtuList)
        {
            // _checkTimeList = new Hashtable();
            try
            {
                foreach (string rtuid in rtuList)
                {
                    _checkTimeList.Add(rtuid, true);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return;
        }

        private bool NeedCheckTime(string rtuid)
        {
            bool result = false;
            try
            {
                if (_checkTimeList != null)
                {
                    result = _checkTimeList.Contains(rtuid);
                }
            }
            catch
            {

            }
            return result;
        }

        private void RemoveCheckTimeList(string rtuid)
        {

            _checkTimeList.Remove(rtuid);
        }

        public void ExitInstance()
        {
            key = false;

            try
            {

                if (serv.Server.Connected == true)
                {
                    serv.Server.Shutdown(SocketShutdown.Both);

                }
                serv.Server.Close();
                serv.Stop();
                serv = null;
                GC.SuppressFinalize(this);

            }
            catch (Exception e)
            {

                _logger.Error(e.Message, e);
            }

        }

        public int Add2SendList(string rtuid, string content, string commandtype)
        {
            string sql;
            DataRow tmpRow = sendTable.NewRow();
            tmpRow["rtuid"] = rtuid;
            tmpRow["content"] = content;
            tmpRow["commandtype"] = commandtype;
            if (commandtype != "HistoryDataRecovery" && commandtype != "PulseVolumeTodayDataRecovery")
            {
                sql = "rtuid='{0}' and commandtype='{1}' and content='{2}'";
                sql = string.Format(sql, rtuid, commandtype, content);
                DelRow(sendTable, sql);
            }
            sendTable.Rows.Add(tmpRow);
            return Convert.ToInt32(tmpRow["ID"].ToString());

        }
        private bool DelRow(DataTable Table, string sql)
        {

            try
            {
                lock (Table)
                {
                    DataRow[] DR = Table.Select(sql);
                    if (DR.Rank > 0)
                    {
                        foreach (DataRow dr in DR)
                        {
                            Table.Rows.Remove(dr);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {

                _logger.Error(e.Message, e);
                return false;
            }

        }
        public bool SetKey//设置服务器运行状态,key为false时断开
        {
            set
            {
                this.key = value;
            }
        }

        public bool SetNeepReply
        {
            set
            {
                _bNeedReply = value;
            }
        }

        public int SetAddDay
        {
            set
            {
                _addDay = value;
            }

        }

        public int SetAddSecond
        {
            set
            {
                _addSecond = value;
            }
        }

        public void StartListen()//开始监听
        {
            try
            {
                serv.Start(300);
                _logger.Debug("tcplistener start");
                manualResetEvent.Reset();
                serv.BeginAcceptTcpClient(new AsyncCallback(RecvCallBack), serv);
                manualResetEvent.WaitOne();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        private void RecvCallBack(IAsyncResult ar)//Accept回调方法
        {
            try
            {
                manualResetEvent.Set();
                _countlisten++;
                bool acceptfailed = false;

                TcpClient tcpRecv = new TcpClient();
                NetworkStream nStream = null;

                try
                {
                    tcpRecv = serv.EndAcceptTcpClient(ar);
                    nStream = tcpRecv.GetStream();
                }
                catch (Exception e)
                {
                    if (tcpRecv != null)
                    {
                        if (tcpRecv.Client != null)
                        {

                            _logger.Debug("get an invalid socket client");

                        }
                    }
                    _logger.Error("mark1:" + e.Message, e);
                    acceptfailed = true;
                }

                if (!key)
                {
                    serv.Stop();
                    serv = null;
                    return;
                }

                if (acceptfailed)
                {
                    _logger.Debug("mark2");
                }

                //manualResetEvent.Reset();
                _logger.Debug("count listen:" + _countlisten.ToString());
                serv.BeginAcceptTcpClient(new AsyncCallback(RecvCallBack), serv);
                manualResetEvent.WaitOne();

                if (acceptfailed)
                {
                    _logger.Debug("mark3");
                    _countlisten--;
                    //manualResetEvent.Set();
                    return;
                }
                StateObject stateobj = new StateObject();
                stateobj.WorkSocket = tcpRecv.Client;
                stateobj.WorkStream = nStream;
                stateobj.BeginTime = DateTime.Now;
                stateobj.RemoteIp = ((System.Net.IPEndPoint)tcpRecv.Client.RemoteEndPoint).Address.ToString();

                try
                {
                    StateObject resultobj = _handleLists.Find(delegate(StateObject x) { if (x.WorkSocket == stateobj.WorkSocket) { return true; } else { return false; } });
                    if (resultobj == null)
                    {
                        _handleLists.Add(stateobj);
                    }
                    else
                    {
                        resultobj.BeginTime = DateTime.Now;
                    }

                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                }

                if (acceptfailed)
                {
                    _logger.Debug("mark4");
                }
                //  nStream.BeginRead(result, 0, result.Length, new AsyncCallback(DoRecvCallBack), stateobj);
                nStream.BeginRead(stateobj.Buffer, 0, stateobj.Buffer.Length, new AsyncCallback(DoRecvCallBack), stateobj);
                if (acceptfailed)
                {
                    _logger.Debug("mark5");
                }


                _countlisten--;
                //manualResetEvent.Set();
            }
            catch (Exception e)
            {
                _countlisten--;
                _logger.Error(e.Message, e);
            }
        }

        private void DoRecvCallBack(IAsyncResult ar)//NetWorkStream.beginread回调方法
        {
            StateObject getstateobject = (StateObject)ar.AsyncState;
            Socket handler = getstateobject.WorkSocket;
            getstateobject.BeginTime = DateTime.Now;
            _handleLists.Find(delegate(StateObject x) { if (x.WorkSocket == handler) { x.BeginTime = DateTime.Now; return true; } else { return false; } });

            NetworkStream netStream = getstateobject.WorkStream;//ar.AsyncState as NetworkStream;
            // StringBuilder resultbuilder = getstateobject.sb;
            byte[] getresult = getstateobject.Buffer;
            if (netStream == null)
            {
                _logger.Debug("DoRecvCallBack : netStream is null;return");
                return;
            }
            int count = 0;

            try
            {

                if (handler.Connected)
                {
                    count = netStream.EndRead(ar);
                }

                if (count > 0)
                {

                    netStream.BeginRead(getstateobject.Buffer, 0, getstateobject.Buffer.Length, new AsyncCallback(this.DoRecvCallBack), getstateobject);
                }

            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
            if (count == 0)
            {

                return;
            }

            try
            {

                string read = Encoding.ASCII.GetString(getresult, 0, count);

                _logger.Debug("received from " + ((System.Net.IPEndPoint)(handler.RemoteEndPoint)).Address.ToString() + " to port[" + endPoint.Port.ToString() + "]" + read);
                ReceivedDataProcess(read, getstateobject);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

        }

        /// <summary>
        /// 处理接收数据块,包含 回复,待发命令下发,不完全包保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="handler"></param>
        private void ReceivedDataProcess(string read, StateObject getstateobject)
        {
         

            try
            {
                // myperformancecounter.AddValue("DLC_TCPlistener", "receive msgs", 1);
                Socket handler = getstateobject.WorkSocket;
                string remoteip = getstateobject.RemoteIp;

                read = read.TrimStart().TrimEnd();
                if (read.Length < 1)
                {
                    return;
                }

                SendMsg oWholeMsg = new SendMsg();
                oWholeMsg.Msg = read;
                oWholeMsg.Type = MsgType.WHOLE_SOURCE_MSG;
                oWholeMsg.Measuresetting = null;
                oWholeMsg.RtuId = "";
                oWholeMsg.RemoteIP = remoteip;
                oWholeMsg.RtuId = GetRtuIDFromContent(read);
                MessageArrived(oWholeMsg);

                string[] arraymsg = read.Split(new char[] { '#' });

                for (int n = 0; n < arraymsg.Length; n++)
                {
                    if (arraymsg[n].Length < 1)
                    {
                        continue;
                    }

                    string content;

                    //将最末字符为非#的字符串，放入列表中
                    if (n == arraymsg.Length - 1)//最末字符串
                    {
                        if (read.Substring(read.Length - 1, 1) != "#") //最后的字符串，无 #　结尾
                        {
                            content = arraymsg[n].TrimStart();

                            SavetoLastHeadlist(content, remoteip);  //有头无尾，将其存入列表，不处理
                            return;
                        }
                        else
                        {
                            content = arraymsg[n].TrimStart() + "#";
                        }
                    }
                    else
                    {
                        content = arraymsg[n].TrimStart() + "#";
                    }

                    if (n == 0)
                    {
                        bool hasheadstring = false;
                        foreach (string headstr in RECEIVE_HEADS)
                        {
                            if (headstr == read.Substring(0, 1))
                            {
                                hasheadstring = true;
                                break;
                            }
                        }

                        if (!hasheadstring)
                        {
                            content = GetLastHead(remoteip) + content;
                        }
                    }

                    string getrtuid = GetRtuIDFromContent(content);
                    bool isvirtualclient = IsVirtualClient(content);
                    if (getrtuid.Length == 10)
                    {
                        getstateobject.RtuId = getrtuid;
                    }
                    //if (VerfyData(content))
                    if (true)
                    {

                        if (NeedCheckTime(getrtuid))
                        {

                            string strTimeCheck = _responseMessageObj.GetCheckTimeString(getrtuid, _addDay, _addSecond);//GetCheckTimeString(getrtuid,_addDay,_addSecond);
                            _logger.Debug("CheckTime Send to [" + getrtuid + "]:" + strTimeCheck);
                            SendReply(handler, strTimeCheck);

                            SendMsg timeSendMsg = new SendMsg();
                            timeSendMsg.Msg = strTimeCheck;
                            timeSendMsg.Type = MsgType.SYSTEMTIME_SET;
                            timeSendMsg.SendTime = DateTime.Now;
                            timeSendMsg.TableID = 0;
                            timeSendMsg.Measuresetting = null;
                            timeSendMsg.RemoteIP = remoteip;
                            timeSendMsg.RtuId = getrtuid;
                            MessageArrived(timeSendMsg);

                            RemoveCheckTimeList(getrtuid);
                        }

                        if (_bNeedReply)
                        {
                            string strReply = _responseMessageObj.CreateResponeString(content);
                            _logger.Debug("send reply:" + strReply);
                            if (!string.IsNullOrEmpty(strReply))
                            {
                                SendReply(handler, strReply);
                            }
                            UpdateReceiveMsgs(content);
                        }

                        SendMsg oSendMsg = new SendMsg();
                        oSendMsg.Msg = content;
                        oSendMsg.Type = MsgType.DATA_MSG;
                        oSendMsg.Measuresetting = null;
                        oSendMsg.RtuId = getrtuid;
                        oSendMsg.RemoteIP = remoteip;

                        MessageArrived(oSendMsg);



                    }
                    else
                    {
                        _logger.Debug("the string [" + content + "] can not be verified ,throw it away!");
                    }


                }

                SendCommand(oWholeMsg.RtuId, remoteip, handler);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }
        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="getsendlist"></param>
        private void SendCommand(string rtuid, string ip, Socket handler)
        {
            string expression = "rtuid='" + rtuid + "'";

            DataRow[] resultrows;
            DataTable clonetable = sendTable.Clone(); ;
            clonetable = sendTable.Copy();
            lock (sendTable)
            {
                resultrows = clonetable.Select(expression);

                DelRow(sendTable, expression);
            }
            for (int i = 0; i < resultrows.Length; i++)
            {
                Send(handler, resultrows[i]["content"].ToString());
                Thread.Sleep(1000);
                //g_sendDone.WaitOne();
                SendMsg oSendMsg = new SendMsg();
                oSendMsg.Msg = resultrows[i]["content"].ToString();
                oSendMsg.Type = MsgType.COMMAND_SEND_COMPLETE;
                oSendMsg.SendTime = DateTime.Now;
                oSendMsg.TableID = Convert.ToInt32(resultrows[i]["ID"].ToString());
                oSendMsg.Measuresetting = null;
                oSendMsg.RemoteIP = ip;
                MessageArrived(oSendMsg);

            }



        }
        /// <summary>
        /// 将断包头保存如列表中
        /// </summary>
        /// <param name="content"></param>
        /// <param name="rtuid"></param>
        private void SavetoLastHeadlist(string content, string keyip)
        {
            string keystr = "key_" + keyip;
            try
            {

                if (_lastheadList.ContainsKey(keystr))
                {
                    _lastheadList[keystr] = content;
                }
                else
                {
                    _lastheadList.Add(keystr, content);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        /// <summary>
        /// 依据rtuid取出上次包尾内容,并移除
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
        private string GetLastHead(string keyip)
        {
            string result = "";
            try
            {
                if (_lastheadList.ContainsKey("key_" + keyip))
                {
                    result = _lastheadList["key_" + keyip].ToString();
                    _lastheadList.Remove("key_" + keyip);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }

        private void Send(Socket handler, String data)
        {

            try
            {
                // g_sendDone.Reset();
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                _logger.Debug("socket send data[" + data + "] to " + handler.RemoteEndPoint.ToString());
                string str_remoteip = ((IPEndPoint)handler.RemoteEndPoint).Address.ToString();

                MessageSent(data, str_remoteip);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);



            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        private void InitialSendTable()
        {
            sendTable = new DataTable();
            DataColumn column = new DataColumn();
            column.ColumnName = "ID";
            column.DataType = System.Type.GetType("System.Int32");
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            sendTable.Columns.Add(column);
            sendTable.Columns.Add("rtuid", typeof(String));
            sendTable.Columns.Add("content", typeof(String));
            sendTable.Columns.Add("commandtype", typeof(string));
        }

        private void SendReply(Socket handler, string getstr)
        {

            Send(handler, getstr);
        }
        /// <summary>
        /// 从内容判断是否虚拟终端
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool IsVirtualClient(string content)
        {
            bool result = false;


            return result;
        }

        private string GetRtuIDFromContent(string getstr)
        {
            string result = "";
            try
            {


                if (getstr.Length > 13)
                {
                    int ibegin = -1;
                    foreach (string headstr in RECEIVE_HEADS)
                    {
                        if (getstr.IndexOf(headstr) > -1)
                        {
                            ibegin = getstr.IndexOf(headstr);
                            break;
                        }
                    }
                    if (ibegin >= 0)
                    {
                        result = getstr.Substring(ibegin + 3, 10);
                    }

                }

            }
            catch
            {

                result = "";
            }

            return result;
        }

        /// <summary>
        /// 构造定时器
        /// </summary>
        protected void CreateCheckTimer()
        {
            // Create the delegate that invokes methods for the timer.
            TimerCallback timerDelegate = new TimerCallback(CheckClockTick);
            int interval = 30 * 1000;
            // Create a timer that waits x seconds, then invokes every x seconds.
            _checkClockTimer = new Timer(timerDelegate, this, interval, interval);

        }

        /// <summary>
        /// dispose timer
        /// </summary>
        protected void DisposeCheckClockTimer()
        {
            if (_checkClockTimer == null)
                return;

            _checkClockTimer.Dispose();
            _checkClockTimer = null;
        }

        /// <summary>
        /// 检查时间，到时检查handler队列
        /// </summary>
        /// <param name="state"></param>
        private void CheckClockTick(Object state)
        {
            try
            {

                RollHandle();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }



        }

        private void RollHandle()
        {


            List<StateObject> deleteList = new List<StateObject>();
            foreach (StateObject listenobj in _handleLists)
            {

                try
                {

                    TimeSpan timspan = DateTime.Now - listenobj.BeginTime;
                    if (timspan.TotalSeconds > 60)
                    {
                        try
                        {
                            /////if listenobj.WorkSocket.Poll(-1,SelectMode.SelectRead)
                            try
                            {
                                listenobj.WorkSocket.Shutdown(SocketShutdown.Both);
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e.Message, e);
                            }

                            listenobj.WorkSocket.Close();


                        }
                        catch (Exception e)
                        {
                            _logger.Error(e.Message, e);
                        }

                        if (deleteList != null)
                        {
                            deleteList.Add(listenobj);
                            _logger.Debug(listenobj.WorkSocket.ToString() + " closed!");
                        }



                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);

                }


            }

            try
            {
                foreach (StateObject removeobj in deleteList)
                {

                    _handleLists.Remove(removeobj);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

        }

        private bool ContainsHead(string content)
        {
            bool result = false;
            foreach (string headstr in RECEIVE_HEADS)
            {
                if (content.Contains(headstr))
                {
                    result = true;
                    break;
                }

            }

            return result;
        }
    }
}
