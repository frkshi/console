using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MtuConsole.Common;
using System.Threading;

using DataEntity;
using System.Collections;

namespace MtuConsole.TcpProcess
{


    public class CommunicationProcessBase
    {
        #region 变量
        public SendMsg commMsg;
        protected ManualResetEvent doneEvent;


        protected IDecode m_decode;

        protected SendToConsole sendtoapiobj;

        protected RWDatabase _rwDatabase;
        protected string processid;
        protected int _portID;
        protected MtuLog _servicelog;

        protected List<SendData> _senddatas;
        protected bool _savesenddatakey;//savesenddata 开关
        protected Hashtable _servicecommandlist;
        private int _commandid = 0;
        #endregion


        public Hashtable CommandList
        {
            set
            {
                _servicecommandlist = value;
            }
            get
            {
                return _servicecommandlist;
            }
        }

        public virtual RWDatabase RwDatabase
        {
            set { _rwDatabase = value; }
            get { return _rwDatabase; }
        }
        public virtual void SetSaveSendDataKey(bool key) { _savesenddatakey = key; }

        public virtual void SaveSendData()
        {
            while (_savesenddatakey)
            {
                try
                {

                    lock (_senddatas)
                    {

                        if (_senddatas.Count > 0)
                        {
                            SendData[] datas = new SendData[_senddatas.Count];

                            _senddatas.CopyTo(0, datas, 0, _senddatas.Count);
                            _senddatas.RemoveRange(0, _senddatas.Count);

                            _rwDatabase.LocalCollectionDataManager.AddToWrite(datas);

                        }
                    }

                    Thread.Sleep(3000);
                }
                catch (Exception e)
                { _servicelog.Error(e.Message, e); }
            }
        }
        public virtual void CommMsgProcess_Thread(object obj)
        {
            return;
        }
        public virtual void RemainMsgProcess()
        {
            return;
        }
        public CommunicationProcessBase()
        { }
        public CommunicationProcessBase(SendMsg msg, SendToConsole getsendtoapi, IDecode getdecode, RWDatabase getrw, ManualResetEvent commdoneEvent, string getprocessid, int iport)
        { }
        public virtual CommunicationProcessBase CreateInstance(SendMsg msg, SendToConsole getsendtoapi, IDecode getdecode, RWDatabase getrw, ManualResetEvent commdoneEvent, string getprocessid, int iport)
        {

            _servicelog = new MtuLog();
            commMsg = msg;
            doneEvent = commdoneEvent;
            _senddatas = new List<SendData>();

            m_decode = getdecode;//ProcessControl.m_decode;

            sendtoapiobj = getsendtoapi;
            processid = getprocessid;
            _rwDatabase = getrw;
            _portID = iport;
            m_decode.RwDataBase = getrw;


            return this;
        }
        public virtual void AddToSendDatas(string datastr, string remoteip, string rtuid)
        {
            try
            {
                SendData data = new SendData();
                data.SendTime = DateTime.Now;
                data.MessageContent = datastr;
                data.Status = 0;
                data.TransformMark = 0;
                data.RemoteIP = remoteip;
                data.RtuID = rtuid;
                _senddatas.Add(data);
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        protected void AddCommand(CommandMsg commandmsg)
        {
            commandmsg.CommandId = _commandid++.ToString();
            CommandList.Add("autokey_" + commandmsg.CommandId, commandmsg);

        }

    }


    public class FactoryCommunicationProcess
    {
        public CommunicationProcessBase CreateCommunicationProcessObj(string sDllName, string sObjectName)
        {
            return (CommunicationProcessBase)CommonMethod.CreatePkgObject(sDllName, sObjectName);
        }
    }


}
