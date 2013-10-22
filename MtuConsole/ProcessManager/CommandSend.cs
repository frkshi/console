
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DataAccess;
using DataEntity;
using MtuConsole.TcpProcess;
using MtuConsole.Common;
namespace MtuConsole.ProcessManager
{




    /// <summary>
    /// command message event handler
    /// </summary>
    /// <param name="objMessage"></param>
    public delegate void CommandMessageHandler(CommandMsg objMessage);

    public delegate void CommandReloadMeasuresetting();
    public delegate void CommandResetProcessControl(string communicationID, bool enable);
    //命令处理，根据id获取类型，以事件形式传出
    class CommandSend : ServiceControl
    {

        private MtuLog _logger;

        public event CommandReloadMeasuresetting ReloadMeasuresetting;
        public event CommandMessageHandler CommandArrived;
        public event CommandResetProcessControl ResetProcessControl;
        private Hashtable _tableProcessControl;
        private RWDatabase _rwDatabase;
        public CommandSend(RWDatabase getRWDB)
        {

            _rwDatabase = getRWDB;
        }

        public Hashtable ProcessTable
        {
            get { return _tableProcessControl; }
            set { _tableProcessControl = value; }
        }
        /// <summary>
        /// 处理rtucommand
        /// </summary>
        /// <param name="command">RTUcommand,command值</param>
        /// <param name="typeid">type id:general,communication
        /// <param name="linenumber">用于pulse data recovery</param>
        /// <param name="startdate">用于history data recovery</param>
        /// <param name="enddate">用于history data recovery</param>
        public void CommandProcess(string jid, RTUCommand command, RTUCommandType typeid, int linenumber, DateTime startdate, DateTime enddate, string portID)
        {

            switch (typeid)
            {
                case RTUCommandType.CommunicationConfig:
                case RTUCommandType.GenernalConfig:
                    CommandMsg objSendMsg = new CommandMsg();
                    
                    objSendMsg.Type = typeid;
                    
                    objSendMsg.DestinationRtuID = command.Id;
                    objSendMsg.LineNumber = linenumber;
                    objSendMsg.StartDate = startdate;
                    objSendMsg.EndDate = enddate;
                    objSendMsg.PortID = portID;
                   
                    objSendMsg.InitiateTime = DateTime.Now;
                    objSendMsg.Initiator = jid;
                    CommandArrived(objSendMsg);
                    break;
                case RTUCommandType.HistoryDataRecovery:
                case RTUCommandType.PulseVolumeTodayDataRecovery:
                case RTUCommandType.PulseVolumeYesterdayDataRecovery:
                case RTUCommandType.SystemTimeSetting:
                case RTUCommandType.QueryData:
                    objSendMsg = new CommandMsg();
                    
                    objSendMsg.Type = typeid;
                    
                    objSendMsg.DestinationRtuID = command.Id;
                    objSendMsg.LineNumber = linenumber;
                    objSendMsg.StartDate = startdate;
                    objSendMsg.EndDate = enddate;
                    objSendMsg.PortID = portID;
                    objSendMsg.InitiateTime = DateTime.Now;
                    objSendMsg.Initiator = jid;
                    CommandArrived(objSendMsg);
                    break;

                default:
                    break;


            }
        }



        /// <summary>
        /// NewDla用，查询语句,以及气压下发
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="typeid"></param>
        /// <param name="rtuid"></param>
        public void CommandProcess(string jid, RTUCommandType typeid, string rtuid)
        {
            CommandMsg objSendMsg = new CommandMsg();

            objSendMsg.Type = typeid;

            objSendMsg.DestinationRtuID = rtuid;

            objSendMsg.InitiateTime = DateTime.Now;
            objSendMsg.Initiator = jid;
            CommandArrived(objSendMsg);
        }




        /// <summary>
        /// to reload MeasureSetting
        /// </summary>
        /// <param name="reloadMeasuresetting"></param>
        public void ReloadMeasureCommandProcess(string rtuID)
        {


            try
            {
                foreach (object obj in _tableProcessControl.Values)
                {
                    ProcessControl processObject = (ProcessControl)obj;
                    processObject.LoadMeasureSettingTable();

                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

        }

        /// <summary>
        /// 各通道重载气压数据
        /// </summary>
        public void ReloadAirPressure()
        {


            try
            {
                foreach (object obj in _tableProcessControl.Values)
                {
                    ProcessControl processObject = (ProcessControl)obj;
                    processObject.ReloadAirPressure();

                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        /// <summary>
        /// 处理老数据
        /// </summary>
        public void RemainDataProcess()
        {
            foreach (object obj in _tableProcessControl.Values)
            {
                try
                {
                    ProcessControl processObject = (ProcessControl)obj;
                    processObject.RemainDataProcessBegin();
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                }

            }

        }
        public void ResetProcessControlProcess(string communicationID, bool enable)
        {
            ResetProcessControl(communicationID, enable);
        }


        /// <summary>
        /// 获取encode
        /// </summary>
        /// <param name="rtuID"></param>
        /// <returns></returns>
        public string GetCommandEncodeString(string rtuID, RTUCommandType typeid, int linenumber, DateTime startdate, DateTime enddate, string portID)
        {
            string result = string.Empty;

            ProcessControl obj = RtuIDtoProcessObj(rtuID);



            result = obj.GetCommandEncodeString(typeid, rtuID, linenumber, startdate, enddate, portID);
            return result;
        }

        /// <summary>
        /// 由rtuID 对应的processcontrol
        /// </summary>
        /// <param name="rtuID">rtu id</param>
        /// <returns></returns>
        private ProcessControl RtuIDtoProcessObj(string rtuID)
        {

            int processcontrolID;
            //从数据库中查出该rtu的类型





            //
            RTUSetting rs = _rwDatabase.LocalSettingManager.LoadRTUSetting(rtuID);

            if (rs != null)
            {
                processcontrolID = Convert.ToInt16(rs.CommunicationId);
            }
            else
            {
                return null;
            }

            ProcessControl obj = null;
            try
            {

                if (processcontrolID != -1)
                {
                    obj = (ProcessControl)_tableProcessControl["key_" + processcontrolID.ToString()];


                }

            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
            return obj;

        }

        /// <summary>
        /// 根据通道编号返回通道对象
        /// </summary>
        /// <param name="communicationid"></param>
        /// <returns></returns>
        private ProcessControl CommunicationIdToProcessObj(int communicationid)
        {

            ProcessControl obj = null;

            try
            {


                obj = (ProcessControl)_tableProcessControl["key_" + communicationid.ToString()];




            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
            return obj;
        }

        /// <summary>
        /// 返回指定通道属性
        /// </summary>
        /// <param name="communicationid"></param>
        /// <returns></returns>
        public ProcessControlPropties GetProcessControlPropties(int communicationid)
        {
            ProcessControlPropties result = null;

            try
            {
                ProcessControl obj = CommunicationIdToProcessObj(communicationid);
                if (obj != null)
                {
                    result = obj.Propties;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
            return result;
        }


    }
}
