
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Reflection;
using System.Threading;
using System.Globalization;


using MtuConsole.Common;
using FunctionLib;
using MtuConsole.TcpCommunication;
using DataAccess;
using System.Linq;
using DataEntity;


namespace MtuConsole.TcpProcess
{

    public class ProcessControl
    {

        #region 变量
        
        const int COMMUNICATION_THREAD_MAX = 12;
        private MtuLog _servicelog;
        private RWDatabase _rwDatabase = new RWDatabase();

        /// <summary>
        /// 时钟，以在00：00向所有rtu发送对时命令
        /// </summary>
        private Timer _rtuCheckSystemClockTimer;

        // private ServiceLog _servicelog;
        /// <summary>
        /// 时钟周期
        /// </summary>
        private int _timerInterval = 10;

        IDecode _decode;
        IEncode _encode;
        IResponseMessage _responseMessage;
        // int _addday, _addsecond;
        private FactoryDecodeEncode _factorydecodeencode;
        //  Communication _communication;

        //SocketListener _socketListener;
        private FactoryTcpListener _factorytcplistener;
        SocketListener _socketListener;

        private FactoryCommunicationProcess _factorycommunicataionprocess;

        private SendToConsole _sendToConsole;

        ManualResetEvent[] _commdoneEvents = new ManualResetEvent[COMMUNICATION_THREAD_MAX];
        CommunicationProcessBase[] _communicationArray = new CommunicationProcessBase[COMMUNICATION_THREAD_MAX];
        DataTable _measureSetingTable;
        DataTable _rtusettingtable;
        DataTable _airpressureTable;
        CommunicationProcessBase _communicataionProcessObj = new CommunicationProcessBase();

        public static int DatalogType;

        // private List<CommandMsg> _commandLists;
        private int _nowDay;

        private ProcessControlPropties _processControlPropties;

        private int _msgcount = 0;

        private Queue<SendMsg> _msgQueue;
        Thread _msgthread;
        bool msgthread_enable = true;
        #endregion

        #region Public Method

        public void SetRWobjects(RWDatabase getrwdatabase)
        {
            try
            {
                _rwDatabase = getrwdatabase;


                LoadMeasureSettingTable();
                ReloadAirPressure();
                _communicataionProcessObj.RwDatabase = _rwDatabase;
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }

        }
      
        public void SetCommandLists(Hashtable getcommandLists)
        {
            _processControlPropties.CommandList = getcommandLists;
            try
            {
                _communicataionProcessObj.CommandList = _processControlPropties.CommandList;
            }
            catch
            { }
        }

        public ProcessControlPropties Propties
        {
            get
            {
                return _processControlPropties;
            }
            set
            {
                _processControlPropties = value;
            }
        }

        public void SetApiObject(ServerHost gethost)
        {
            try
            {
                _sendToConsole = new SendToConsole(gethost);
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }

        }

        /// <summary>
        /// state 指示processcontrol状态，0：未initial 1:开始initial 2:正常工作 3：load失败 4：正在退出
        /// </summary>
        public int State
        {
            get
            {
                return _processControlPropties.State;
            }
            set
            {
                _processControlPropties.State = value;
            }
        }
        /// <summary>
        /// 初始化paocessControl
        /// </summary>
        /// <param name="ProcessGroupID">进程组标号</param>
        public ProcessControl(int getprocessgroupID)
        {

            Thread.CurrentThread.IsBackground = true;
            _servicelog = new MtuLog();
            _msgQueue = new Queue<SendMsg>();
            try
            {

                _factorydecodeencode = new FactoryDecodeEncode();

                _factorytcplistener = new FactoryTcpListener();

                _factorycommunicataionprocess = new FactoryCommunicationProcess();
                _processControlPropties = new ProcessControlPropties();
                _processControlPropties.State = 0;   //State = 0;
                _processControlPropties.ProcessGroupID = getprocessgroupID; 
                _processControlPropties.StartTime = DateTime.Now;
                _communicataionProcessObj = new CommunicationProcess(); 


                _communicataionProcessObj.SetSaveSendDataKey(true);
                Thread savesendthread = new Thread(new ThreadStart(_communicataionProcessObj.SaveSendData));
                savesendthread.Start();

            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }

        /// <summary>
        /// initial 过程，启动各引用实例，初始化线程池

        /// </summary>
        public void CreateInstance()
        {
            try
            {
                State = 1;


                _nowDay = DateTime.Now.Day;
                for (int i = 0; i < COMMUNICATION_THREAD_MAX; i++)
                {
                    _commdoneEvents[i] = new ManualResetEvent(true);
                }

                
                this.LoadLib();
               

                if (_processControlPropties.CheckTimeMidNight)
                {
                    CreateCheckSystemClockTimer();
                }

                State = 2;

                _servicelog.Info(new LogMessage() { Action = ActionSource.Channel, Message = "通道[" + _processControlPropties.TcpPort.ToString() + "]开启", Level = EventLevel.Medium });
            }
            catch (Exception e)
            {
                State = 3;
                
                _servicelog.Info(new LogMessage() { Action = ActionSource.Channel, Message = "通道[" + _processControlPropties.TcpPort.ToString() + "]启用失败", Level = EventLevel.High });

            }
        }


        /// <summary>
        /// 退出过程，退出各实例，等待线程驰空
        /// </summary>
        public void ExitInstatnce()
        {
            State = 4;
            _servicelog.Info(new LogMessage() { Action = ActionSource.Channel, Message = "通道[" + _processControlPropties.TcpPort.ToString() + "]关闭", Level = EventLevel.Medium });
            _communicataionProcessObj.SetSaveSendDataKey(false);
            DisposeCheckSystemClockTimer();
            _socketListener.ExitInstance();
            msgthread_enable = false;
            _msgthread.Join();


            Thread.Sleep(500);
            //等待其他线程池空
           WaitHandle.WaitAll(_commdoneEvents);

            //////////////////////////////////////////////////////////////////////////
            // bug, _rwDatabase is belong to service control ,should not dispose here.affect other processcontol
            //_rwDatabase.Dispose();


            _decode = null;
            _encode = null;


            _servicelog.Debug("ExitInstatnce");

            _servicelog = null;
            //  System.Environment.Exit(0); 
            GC.SuppressFinalize(this);



        }

        public void PostToApi(DataTable dt, int typeid)
        {
            //SH3H.DataLog.API.Protocol.Client.Message sendmsg = new SH3H.DataLog.API.Protocol.Client.Message();


        }
        public string GetCommandEncodeString(RTUCommandType commandtype, string rtuID, int linenumber, DateTime StartDate, DateTime EndDate, string PortID)
        {
            string result = string.Empty;

            try
            {
                CommunicationSetting tmpcommunicationsettingEntity;

                RTUSetting tmpRTUsettingEntity;
                tmpcommunicationsettingEntity = _rwDatabase.LocalSettingManager.LoadCommunicationSetting(_processControlPropties.ProcessGroupID.ToString());
                DataTable tmpmeasuretable = _rwDatabase.LocalSettingManager.LoadMeasureSettingByRtuId(rtuID);

                tmpRTUsettingEntity = _rwDatabase.LocalSettingManager.LoadRTUSetting(rtuID);

                _encode.SetCommunicationSetting(tmpcommunicationsettingEntity);
                _encode.SetMeasureSetting(tmpmeasuretable);
                _encode.SetRtuSetting(tmpRTUsettingEntity);


                CommandParameters cmdparameters = new CommandParameters();
                cmdparameters.LineNumber = linenumber;
                cmdparameters.StartDate = StartDate;
                cmdparameters.EndDate = EndDate;
                cmdparameters.PortID = PortID;
                result = _encode.EncodeData(commandtype, cmdparameters);


            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);

            }


            return result;

        }


        public void RemainDataProcessBegin()
        {
            Thread t = new Thread(new ThreadStart(RemainDataProcess));
            t.Start();

        }
        /// <summary>
        /// 重载air pressure
        /// </summary>
        public void ReloadAirPressure()
        {
            try
            {
                _airpressureTable = _rwDatabase.LocalSettingManager.LoadAirPressure();
                //  _decode.AirPressure = _airpressureTable;
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// 根据配置实例化各组件对象
        /// </summary>
        /// <returns></returns>
        private void LoadLib()
        {

            //XmlNode tmpNode;
            string dllname = "decode.dll";

            // object tmpObj = null;
            try
            {


               
                _processControlPropties.DecodeDllName = "decode.dll";
                //bCheckTime 标示是否需要0点对时

                _processControlPropties.CheckTimeMidNight = true;
                _processControlPropties.EnableAirPressure =false;
                _processControlPropties.NeedReply = true;
                _processControlPropties.TcpPort =Convert.ToInt16( ConfigureAppConfig.GetAppSettingsKeyValue("listenport")); //PORT;



                
                _encode = _factorydecodeencode.CreateEncode(dllname, "Decode.Encode");

                _decode = _factorydecodeencode.CreateDecode(dllname, "Decode.Decode");
                _responseMessage = _factorydecodeencode.CreateResponseMessage(dllname, "Decode.ResponseMessage");

                _decode.SetEnableAirPressure(_processControlPropties.EnableAirPressure);
                ////注册通讯事件

               // dllname = _configManager.ConfigSetting.CommunicationDllName;
                //dllname = "Mtu_communication.dll";
                _socketListener = new SocketListener();// _factorytcplistener.CreateTcpListener(dllname, "MtuConsole.TcpCommunication.SocketListener");  // new SocketListener(_processControlPropties.TcpPort, _responseMessage);
                _socketListener.CreateInstance(_processControlPropties.TcpPort, _responseMessage);
                _socketListener.SetKey = true;
                _socketListener.SetNeepReply = _processControlPropties.NeedReply;
                _socketListener.SetAddDay = _processControlPropties.AddDay;
                _socketListener.SetAddSecond = _processControlPropties.AddSecond;
                _socketListener.MessageArrived += new CommnicationMessageHandler(OnMsgArrived);

                _socketListener.MessageSent += new MessageSentHandler(OnMessageSent);




                Thread t = new Thread(new ThreadStart(_socketListener.StartListen));
                t.Start();//启动服务端

                if (_processControlPropties.NeedReply)
                {
                    Thread framecountthread = new Thread(new ThreadStart(_socketListener.MultiDataCheck));
                    framecountthread.Start(); //启动多桢收集检查 
                }
                _msgthread = new Thread(new ThreadStart(CallProcessMsgQueue));
                _msgthread.Start();




            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }



        public void LoadMeasureSettingTable()
        {
            try
            {
                _measureSetingTable = _rwDatabase.LocalSettingManager.LoadMeasureSetting();
                _rtusettingtable = _rwDatabase.LocalSettingManager.LoadRTUSetting();
                // _decode.MeasureSetting = _measureSetingTable;
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }
        private void OnMsgArrived(SendMsg msg)
        {
            AddToQueue(msg);
        }
        /// <summary>
        /// 压栈
        /// </summary>
        /// <param name="commmsg"></param>
        private void AddToQueue(SendMsg commmsg)
        {
            lock (_msgQueue)
            {
                _msgQueue.Enqueue(commmsg);
                // _servicelog.Info(new LogMessage() { Action = ActionSource.Service, Message = "Add to Queue" + commmsg.Msg });

            }
        }
        private void CallProcessMsgQueue()
        {
            while (msgthread_enable)
            {
                ProcessMsgQueue();
                Thread.Sleep(1000);
            }
        }
        private void ProcessMsgQueue()
        {

            List<SendMsg> msgs = new List<SendMsg>();

            if (_msgQueue.Count > 0)
            {
                lock (_msgQueue)
                {
                    if (_msgQueue.Count > 0)
                    {
                        while (_msgQueue.Count > 0)
                        {
                            msgs.Add(_msgQueue.Dequeue());
                        }
                    }


                }
                foreach (SendMsg item in msgs)
                {
                    CommMsgProcess(item);
                }
            }

        }
        /// <summary>
        /// 处理 communication event,判断事件类型，
        /// DADA事件将msg使用decode，后抛给rw和api
        /// err事件，将错误记录
        /// </summary>
        /// <param name="commMsg"></param>
        private void CommMsgProcess(SendMsg commMsg)
        {
            try
            {

                switch (commMsg.Type)
                {
                    case MsgType.DATA_MSG:
                        Thread.Sleep(50);
                        _msgcount++;
                        Console.WriteLine("[" + DateTime.Now.ToString() + "]message receives count:" + _msgcount.ToString());
                        RefreshSendList(commMsg.RtuId);


                        string msg = commMsg.Msg.TrimStart().TrimEnd();
                        SendMsg getmsg = new SendMsg();

                        getmsg.Msg = msg;//commMsg.Msg;
                        getmsg.RemoteIP = commMsg.RemoteIP;
                        getmsg.RtuId = commMsg.RtuId;
                        int freeindex = WaitHandle.WaitAny(_commdoneEvents);
                       

                        _decode.SetEnableAirPressure(_processControlPropties.EnableAirPressure);
                        _decode.MeasureSetting = _measureSetingTable;
                        _decode.RtuSetting = _rtusettingtable;
                        _decode.AirPressure = _airpressureTable;
                    
                        CommunicationProcessBase commthread = _communicataionProcessObj.CreateInstance(getmsg, _sendToConsole, _decode, _rwDatabase,
                               _commdoneEvents[freeindex], _processControlPropties.ProcessGroupID.ToString(), _processControlPropties.TcpPort);

                        _communicationArray[freeindex] = commthread;
                        ThreadPool.QueueUserWorkItem(commthread.CommMsgProcess_Thread, freeindex);
                        // }

                        // 记录终端通讯信息; 异步
                        RecordRTULog(commMsg);

                        break;
                    
                    case MsgType.ERR_MSG:

                        _servicelog.Error(commMsg.Msg);
                        //错误日志
                        break;
                    case MsgType.WHOLE_SOURCE_MSG:  //整个源包抵达
                        if (_rwDatabase != null)
                        {
                            CollectionData data = new CollectionData();
                            data.MessageContent = commMsg.Msg;
                            data.SendTime = DateTime.Now;
                            data.Status = 0;
                            data.RemoteIP = commMsg.RemoteIP;
                            data.RtuID = commMsg.RtuId;

                            _rwDatabase.LocalCollectionDataManager.AddToWrite(data);
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                if (_servicelog != null)
                {
                    _servicelog.Error(e.Message, e);
                }
            }


        }



        private void OnMessageSent(string msg, string remoteip)
        {
            try
            {
                string rtuid = GetRtuIDFromContent(msg);

                _communicataionProcessObj.AddToSendDatas(msg, remoteip, rtuid);


            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }

        private List<string> SplitMsg(string getMsg, List<string> inputList)
        {
            List<string> result = inputList;


            int beginPos = getMsg.IndexOf('*');
            int endPos = getMsg.IndexOf('#') + 2;
            if (beginPos > -1 && endPos > 1 && beginPos < endPos && getMsg.Length > endPos)
            {
                result.Add(getMsg.Substring(beginPos, endPos - beginPos + 1));
                SplitMsg(getMsg.Substring(endPos + 1, getMsg.Length - endPos - 1), result);
            }
            return result;
        }




        /// <summary>
        /// 处理未解包数据
        /// </summary>
        private void RemainDataProcess()
        {
            // ManualResetEvent doneEvent=new ManualResetEvent();
            try
            {
                LoadMeasureSettingTable();
                List<string> msgArr = new List<string>();     //SplitMsg(commMsg.Msg.TrimStart().TrimEnd(), new List<string>());
                foreach (string msg in msgArr)
                {
                    SendMsg getmsg = new SendMsg();

                    getmsg.Msg = msg;//commMsg.Msg;
                    getmsg.LocalFileName = "";
                    getmsg.NoteId = 100;
                    int freeindex = WaitHandle.WaitAny(_commdoneEvents);


                    //CommunicationProcessBase communicationprocessobj = new CommunicationProcessBase(getmsg, _sendToAPI, _decode, _rwDatabase, null, _processControlPropties.ProcessGroupID.ToString(), _processControlPropties.TcpPort);
                    CommunicationProcessBase communicationprocessobj = _communicataionProcessObj.CreateInstance(getmsg, _sendToConsole, _decode, _rwDatabase, null, _processControlPropties.ProcessGroupID.ToString(), _processControlPropties.TcpPort);
                    communicationprocessobj.RemainMsgProcess();


                }
            }
            catch (Exception e)
            {

            }
        }



        /// <summary>
        /// 刷新communication 中的发送列表
        /// </summary>
        /// <param name="rtuid"></param>
        public void RefreshSendList(string rtuid)
        {
            try
            {
                List<string> deleteList = new List<string>();

                Hashtable getcommandlist = (Hashtable)_processControlPropties.CommandList.Clone();

                foreach (DictionaryEntry de in getcommandlist)  //_processControlPropties.CommandList
                {
                    CommandMsg obj = (CommandMsg)de.Value;
                    string key = de.Key.ToString();
                    

                    if (obj.DestinationRtuID == rtuid)
                    {
                        if (obj.Type ==RTUCommandType.SystemTimeSetting)
                        {
                            List<string> rtulist = new List<string>();
                            rtulist.Add(rtuid);

                            _socketListener.SetCheckTimeList(rtulist);
                            deleteList.Add(key);
                            continue;
                        }

                        CommunicationSetting tmpcommunicationsettingEntity1;
                        CommunicationSetting tmpcommunicationsettingEntity2;
                        //  MeasureSetting tmpmeasuresettingEntity;
                        RTUSetting tmpRTUsettingEntity;
                        tmpRTUsettingEntity = _rwDatabase.LocalSettingManager.LoadRTUSetting(obj.DestinationRtuID);

                        tmpcommunicationsettingEntity1 = _rwDatabase.LocalSettingManager.LoadCommunicationSetting(tmpRTUsettingEntity.CommunicationId.ToString());
                        if (string.IsNullOrEmpty(tmpRTUsettingEntity.CommunicationId1.ToString()))
                        {
                            tmpcommunicationsettingEntity2 = tmpcommunicationsettingEntity1;
                        }
                        else
                        {
                            tmpcommunicationsettingEntity2 = _rwDatabase.LocalSettingManager.LoadCommunicationSetting(tmpRTUsettingEntity.CommunicationId1.ToString());
                        }
                        DataTable tmpmeasuretable = _rwDatabase.LocalSettingManager.LoadMeasureSettingByRtuId(obj.DestinationRtuID);

                        IEncode tmpEncode;

                        if (_processControlPropties.ProcessGroupID == tmpRTUsettingEntity.CommunicationId)
                        {
                            tmpEncode = _encode;
                        }
                        else
                        {
                  
              
                            tmpEncode = _factorydecodeencode.CreateEncode("Decode.dll", "Decode.Encode");

                        }

                        tmpEncode.SetCommunicationSetting(tmpcommunicationsettingEntity1, tmpcommunicationsettingEntity2);


                        tmpEncode.SetMeasureSetting(tmpmeasuretable);

                
                        tmpEncode.SetRtuSetting(tmpRTUsettingEntity);
                     

                        tmpEncode.SetEnableAirPressure(_processControlPropties.EnableAirPressure);
                        tmpEncode.AirPressure = _airpressureTable;

                        List<string> endcodedatalist;
                        CommandParameters cmdparameters = new CommandParameters();
                        cmdparameters.LineNumber = obj.LineNumber;
                        cmdparameters.StartDate = obj.StartDate;
                        cmdparameters.EndDate = obj.EndDate;

                       

                        cmdparameters.RtuID = obj.DestinationRtuID;
                        endcodedatalist = tmpEncode.EncodeDataList(obj.Type, cmdparameters);

                        foreach (string encodedata in endcodedatalist)
                        {
                            if (string.IsNullOrEmpty(encodedata))
                            {
                                continue;
                            }
                            int sendID = _socketListener.Add2SendList(obj.DestinationRtuID, encodedata, obj.Type.ToString());
                            DateTime receivetime = obj.InitiateTime; //DateTime.Now;
                            //_sendToAPI.AddToList(sendID, obj.DestinationRtuID, receivetime, obj.Type, _processControlPropties.TcpPort, _processControlPropties.ProcessGroupID);

                            obj.CommandSendState = true;
                            obj.Data = encodedata;
                        }
                      

                        deleteList.Add(key);
                    }
                }
                foreach (string deletekey in deleteList)
                {
                    _processControlPropties.CommandList.Remove(deletekey);
                }
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }

      
        /// <summary>
        /// 获取字符串中rtuID
        /// </summary>
        /// <param name="getstr"></param>
        /// <returns></returns>
        private string GetRtuIDFromContent(string getstr)
        {
            string result = "";


            try
            {

                getstr = getstr.Trim();
                if (getstr.Length > 14)
                {
                    if (getstr.Length - 1 == getstr.LastIndexOf('#'))
                    {
                        result = getstr.Substring(3, 10);
                    }
                    else if (getstr.Contains('*'))
                    {
                        result = getstr.Substring(getstr.IndexOf('*') + 1, 4);
                    }
                    else if (getstr.ToUpper().Contains("FEFEFD") && getstr.Length > 30)
                    {
                        result = CommonMethod.HexToString(getstr.Substring(10, 20));
                    }
                }
                else
                {
                    result = "";
                }
            }
            catch
            {

                result = "";
            }
            return result;
        }


        #region 零点对时

        /// <summary>
        /// 构造定时器
        /// </summary>
        protected void CreateCheckSystemClockTimer()
        {
            // Create the delegate that invokes methods for the timer.
            TimerCallback timerDelegate = new TimerCallback(CheckSystemClockTick);
            int interval = _timerInterval * 1000;
            // Create a timer that waits x seconds, then invokes every x seconds.
            _rtuCheckSystemClockTimer = new Timer(timerDelegate, this, interval, interval);
            _servicelog.Debug("[processcontrol " + _processControlPropties.ProcessGroupID.ToString() + "]  checktime timer initial");
        }

        /// <summary>
        /// dispose timer
        /// </summary>
        protected void DisposeCheckSystemClockTimer()
        {
            if (_rtuCheckSystemClockTimer == null)
                return;

            _rtuCheckSystemClockTimer.Dispose();
            _rtuCheckSystemClockTimer = null;
        }

        /// <summary>
        /// 检查时间，到时发送对时
        /// </summary>
        /// <param name="state"></param>
        private void CheckSystemClockTick(Object state)
        {


            if (_nowDay != DateTime.Now.Day)//(DateTime.Now.Second == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Hour == 0)
            {
                _servicelog.Debug(_processControlPropties.ProcessGroupID.ToString() + ": CheckSystemClockTick set");
                List<string> getRtulist = GetRtulist();
                _servicelog.Debug(_processControlPropties.ProcessGroupID.ToString() + ":[rtulist num]" + getRtulist.Count.ToString());

                _socketListener.SetCheckTimeList(getRtulist);
                _nowDay = DateTime.Now.Day;
            }


        }

        private List<string> GetRtulist()
        {
            List<string> result = new List<string>();
            DataTable tmpTable = _rwDatabase.LocalSettingManager.LoadRTUSetting();

            foreach (DataRow dr in tmpTable.Rows)
            {
                if (dr["CommunicationId"].ToString() == _processControlPropties.ProcessGroupID.ToString())
                {
                    result.Add(dr["rtuid"].ToString());
                }
            }

            return result;
        }

        #endregion



        public void SetRTUDataCache(RTUDataCacheDictionary rtuDataCache)
        {
            _processControlPropties.RTUDataCache = rtuDataCache;
        }

        private void RecordRTULog(SendMsg commMsg)
        {

            try
            {
                if (string.IsNullOrEmpty(commMsg.RtuId))
                {
                    return;
                }
                RTUDataCache _rtuCache = _processControlPropties.RTUDataCache[commMsg.RtuId];


                RTULogStack _stack = _rtuCache.RTULog;

                _stack.Add(new RTULog()
                {
                    RTUID = commMsg.RtuId,
                    IP = commMsg.RemoteIP,
                    Time = DateTime.Now,
                    Data = commMsg.Msg
                });


                if (_rwDatabase != null)
                {

                    _rwDatabase.LocalSettingManager.AddToWrite(_stack.First());

                }
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }
        #endregion
    }

}
