
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


using MtuConsole.Common;

namespace MtuConsole.TcpProcess
{

   
    /// <summary>
    /// 
    /// TODO:  往界面发送消息
    /// </summary>
    public class SendToConsole
    {
        /// <summary>
        /// API server host
        /// </summary>
        private ServerHost _host;
        /// <summary>
        /// 命令恢复列表
        /// </summary>
        private Hashtable _commandListToReply;
        /// <summary>
        /// 日志对象
        /// </summary>
        private MtuLog _logger;

        

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="gethost"></param>
        public SendToConsole(ServerHost gethost)
        {
            _logger = new MtuLog();
            _commandListToReply = new Hashtable();
            _host = gethost;
        }
        public ServerHost Host
        {
            get
            {
                return _host;
            }
        }
        /// <summary>
        /// 发送命令加入列表
        /// </summary>
        /// <param name="tableid"></param>
        /// <param name="rtuid"></param>
        /// <param name="receivetime"></param>
        /// <param name="commandType"></param>
        /// <param name="portid"></param>
        /// <param name="processid"></param>
        public void AddToList(int tableid, string rtuid, DateTime receivetime, RTUCommandType commandType, int portid, int processid)
        {
            try
            {
                //CommandResponseMessageBody body = new CommandResponseMessageBody();
                //body.CommandType = commandType;
                //body.RtuId = rtuid;
                //body.InitiateTime = receivetime;
                //body.Port = portid.ToString();
                //body.CommunicationId = processid.ToString();

                //_commandListToReply.Add("key_" + tableid.ToString(), body);


            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }


        }
        public void SendSystemtimeSetToAPI(DateTime sendtime, string content, string ip, string rtuid)
        {
            //try
            //{
            //    CommandResponseMessageBody sendobj = new CommandResponseMessageBody()
            //    {
            //        IP = ip,
            //        Data = content,
            //        Result = true,
            //        CommandType = RTUCommandType.SystemTimeSetting,
            //        ResponseTime = sendtime,
            //        InitiateTime = sendtime,
            //        RtuId = rtuid
            //    };
            //    Message sendmessage = new Message();
            //    sendmessage.Type = MessageType.Notice;
            //    sendmessage.Body = sendobj;
            //    Send(sendmessage);
            //}
            //catch (Exception e)
            //{
            //    _logger.Error(e.Message, e);
            //}


        }
        /// <summary>
        /// 发送列表中条消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sendtime"></param>
        /// <param name="content"></param>
        public void SendList(int tableid, DateTime sendtime, string content, string ip)
        {
            //try
            //{
            //    CommandResponseMessageBody sendobj;
            //    sendobj = (CommandResponseMessageBody)_commandListToReply["key_" + tableid.ToString()];

            //    sendobj.IP = ip;
            //    sendobj.Data = content;
            //    sendobj.ResponseTime = sendtime;
            //    sendobj.Result = true;

            //    Message sendmessage = new Message();
            //    sendmessage.Type = MessageType.Notice;
            //    sendmessage.Body = sendobj;

            //    if (Send(sendmessage))
            //    {
            //        _commandListToReply.Remove("key_" + tableid.ToString());
            //    }
            //}
            //catch (Exception e)
            //{
            //    _logger.Error(e.Message, e);
            //}
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Send(Message msg)
        {
            bool result = false;
            if (msg != null)
            {
                if (msg.Body != null)
                {

                    try
                    {

                        _host.Send(msg);
                       
                        result = true;

                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message, e);
                    }
                }
            }
            return result;

        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public bool Send(List<Message> msgs)
        {
            bool result = true;
            //待写，for case sDataType.MeasureDataPulseHour
            foreach (Message msg in msgs)
            {

                result = result & Send(msg);
            }
            return result;

        }
    }
}
