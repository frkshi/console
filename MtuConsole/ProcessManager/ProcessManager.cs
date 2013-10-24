
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using MtuConsole.TcpProcess;
using DataAccess;
using DataEntity;

using MtuConsole.Common;

namespace MtuConsole.ProcessManager
{
    /// <summary>
    /// 管理processControl
    /// </summary>
    public class ServiceControl
    {
        private MtuLog _servicelog;
        /// <summary>
        /// 配置读取
        /// </summary>
        private ConfigManage _rc;

        /// <summary>
        /// api命令转送
        /// </summary>
        private CommandSend _commandsend;


        private RWDatabase _rwDatabase;


        /// <summary>
        /// api主机
        /// </summary>
        private ServerHost _host;

        //  private ServiceLog _servicelog;
        /// <summary>
        /// processcontrl对象列表
        /// </summary>
        public Hashtable TableProcessControl;



        public Hashtable _commandLists;
        private RTUDataCacheDictionary _rtuDataCache;
        private int _commandID;  ///command id ,identity
        private const int FREE_SPACE = 1024;  //mb

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceControl()
        {

            _servicelog = new MtuLog();

        }
        public ServiceControl(ServerHost host)
        {
            _host = host;
            _servicelog=new MtuLog();
        }
       
        /// <summary>
        /// 实例化过程

        /// </summary>
        /// <returns></returns>
        public bool CreateInstance()
        {
            try
            {
                System.Threading.Thread.CurrentThread.IsBackground = true;


                _servicelog.Debug("ServiceControl.CreateInstance begin");
                _rc = new ConfigManage();

                if (!InitialRW())
                {
                    return false;
                }

                // oCommandSend = new CommandSend();

                _commandsend = new CommandSend(_rwDatabase);
                _commandsend.CommandArrived += new CommandMessageHandler(OnCommandArrived);
                _commandsend.ResetProcessControl += new CommandResetProcessControl(_commandsend_ResetProcessControl);

                _commandLists = new Hashtable();

                InitRTUDataCache();

                if (!InitialAPI())
                {
                    return false;
                }

                //  _commandLists = new List<CommandMsg>();

                TableProcessControl = new Hashtable();

                
                ProcessControl tempProcessControl = new ProcessControl(1);
                tempProcessControl.SetRWobjects(_rwDatabase);
                tempProcessControl.SetApiObject(_host);
                tempProcessControl.SetCommandLists(_commandLists);
                tempProcessControl.SetRTUDataCache(_rtuDataCache);
                tempProcessControl.CreateInstance();

                TableProcessControl.Add("key_" + "1", tempProcessControl);
                
                //实例化processcontrol
                

                _commandsend.ProcessTable = TableProcessControl;

                _servicelog.Info(new LogMessage() { Action = ActionSource.Service, Level = EventLevel.Medium, Message = "服务启动完成" });

            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
                _servicelog.Info(new LogMessage() { Action = ActionSource.Service, Level = EventLevel.High, Message = "服务启动失败" });
                return false;
            }
            return true;
        }
        /// <summary>
        /// 开启或关闭指定通道
        /// </summary>
        /// <param name="communicationID"></param>
        /// <param name="enable"></param>
        void _commandsend_ResetProcessControl(string communicationID, bool enable)
        {
            try
            {
                if (enable)
                {
                    //开通道
                    if (TableProcessControl["key_" + communicationID] == null)
                    {
                        ProcessControl tempProcessControl = new ProcessControl(Convert.ToInt16(communicationID));
                        tempProcessControl.SetRWobjects(_rwDatabase);
                        tempProcessControl.SetApiObject(_host);
                        tempProcessControl.SetCommandLists(_commandLists);
                        tempProcessControl.SetRTUDataCache(_rtuDataCache);
                        tempProcessControl.CreateInstance();
                        TableProcessControl.Add("key_" + communicationID, tempProcessControl);
                    }
                }
                else
                {
                    //关通道
                    ProcessControl obj = (ProcessControl)TableProcessControl["key_" + communicationID];
                    obj.ExitInstatnce();
                    TableProcessControl.Remove("key_" + communicationID);
                }
                _commandsend.ProcessTable = TableProcessControl;
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
        }
        /// <summary>
        /// 退出各实例，释放资源
        /// </summary>
        public void ExitInstance()
        {

            _servicelog.Debug("Exit Instance");

            //停processControl

            if (TableProcessControl != null)
            {
                foreach (object obj in TableProcessControl.Values)
                {
                    ProcessControl processObject = (ProcessControl)obj;
                    processObject.ExitInstatnce();
                    processObject = null;

                }
            }
           
            _servicelog.Info(new LogMessage() { Action = ActionSource.Service, Level = EventLevel.Medium, Message = "服务关闭" });
            _rwDatabase.Dispose();


            _rwDatabase = null;
            _servicelog = null;
            //    System.Environment.Exit(0);




        }

        /// <summary>
        /// 初始化读写组件
        /// </summary>
        /// <returns></returns>
        private bool InitialRW()
        {
            try
            {
                _rwDatabase = new RWDatabase();

                string sqlitepath = Environment.CurrentDirectory + "\\Data";
                var sqlite = new SqliteMeasureDataPersistenceContext(sqlitepath, FileSplitUnit.Hour, FREE_SPACE);
                SqliteAlertDataPersistenceContext sqlitealertcontext = new SqliteAlertDataPersistenceContext(sqlitepath, FileSplitUnit.Day, FREE_SPACE);
                SqliteMeasureDataPersistenceContext sqlitemeasurecontext = new SqliteMeasureDataPersistenceContext(sqlitepath, FileSplitUnit.Day, FREE_SPACE);
                _rwDatabase.LocalMeasureDataManager = new MeasureDataManager(sqlitemeasurecontext);
                SqliteSettingPersistenceContext sqlitesettingcontext = new SqliteSettingPersistenceContext(sqlitepath);
                _rwDatabase.LocalSettingManager = new SettingManager(sqlitesettingcontext);
                SqliteCollectionDataPersistenceContext sqlitecollectioncontext = new SqliteCollectionDataPersistenceContext(sqlitepath, FileSplitUnit.Day, FREE_SPACE);
                _rwDatabase.LocalCollectionDataManager = new CollectionDataManager(sqlitecollectioncontext);
                _rwDatabase.LocalAlertDataManager = new AlertDataManager(sqlitealertcontext);

                _rwDatabase.HasRemoteDB =  false;
                ///TODO:sql路径从配置读
                string sqlconnetstr = "";

                if (_rwDatabase.HasRemoteDB)
                {
                    var sqlConfigServer = new SqlServerMeasureDataPersistenceContext(sqlconnetstr);
                    _rwDatabase.RemoteMeasureDataManager = new MeasureDataManager(sqlConfigServer, sqlite);
                    SqlServerSettingPersistenceContext sqlsettingcontext = new SqlServerSettingPersistenceContext(sqlconnetstr);
                    _rwDatabase.RemoteSettingManager = new SettingManager(sqlsettingcontext);
                    SqlServerAlertDataPersistenceContext sqlalertcontext = new SqlServerAlertDataPersistenceContext(sqlconnetstr);
                    _rwDatabase.RemoteAlertDataManager = new AlertDataManager(sqlalertcontext, sqlitealertcontext);
                }

                _rwDatabase.HasOutSide = false;
                
                ///TODO:外部数据库链接配置
                string outsqlstr = "";
                if (_rwDatabase.HasOutSide)
                {
                    SqlServerMeasureDataExportPersistenceContext outsideMeasureContext = new SqlServerMeasureDataExportPersistenceContext(outsqlstr);
                    SqlServerAlertDataExportPersistenceContext outsideAlertContext = new SqlServerAlertDataExportPersistenceContext(outsqlstr);
                    _rwDatabase.OutSideMeasureDataManager = new MeasureDataExportManager(outsideMeasureContext, sqlitemeasurecontext);
                    _rwDatabase.OutSideAlertManager = new AlertDataExportManager(outsideAlertContext, sqlitealertcontext);
                }

              
            }
            catch (Exception ex)
            {
                _servicelog.Error(ex.Message, ex);
                return false;
            }


            return true;
        }

        /// <summary>
        /// 初始化api组件
        /// </summary>
        /// <returns></returns>
        private bool InitialAPI()
        {
            try
            {
               // _host = new ServerHost();

                //_host.Start(_rc.ApiPort);

                return true;
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
                return false;
            }
        }

        private void InitRTUDataCache()
        {
            _rtuDataCache = new RTUDataCacheDictionary();

            List<RTULog> logs = _rwDatabase.LocalSettingManager.LoadAllRTULog();

            foreach (RTULog item in logs)
                _rtuDataCache[item.RTUID].RTULog.Add(item);

        }
        /// <summary>
        /// api命令到达事件
        /// </summary>
        /// <param name="commMsg"></param>
        private void OnCommandArrived(CommandMsg commMsg)
        {
            try
            {
                commMsg.CommandId = _commandID++.ToString();
                DeleteOldCommand(commMsg);
                _commandLists.Add("key_" + commMsg.CommandId, commMsg);


                int communicationid = RtuIDtoCommunicationID(commMsg.DestinationRtuID);
                if (communicationid != -1)
                {
                    if (TableProcessControl["key_" + communicationid] != null)
                    {
                        ProcessControl obj = (ProcessControl)TableProcessControl["key_" + communicationid];
                        //obj.RefreshSendList(commMsg.DestinationRtuID);
                    }
                }

                //RefreshSendList




            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }

        }
        /// <summary>
        /// 删除重复记录
        /// </summary>
        /// <param name="commmsg"></param>
        private void DeleteOldCommand(CommandMsg commmsg)
        {

            List<string> deleteList = new List<string>();
            foreach (DictionaryEntry de in _commandLists)
            {
                CommandMsg obj = (CommandMsg)de.Value;
                if (commmsg.Type != RTUCommandType.HistoryDataRecovery )
                {
                    if (obj.DestinationRtuID == commmsg.DestinationRtuID && obj.Type == commmsg.Type)
                    {
                        deleteList.Add(de.Key.ToString());
                        _servicelog.Debug("合并删除" + obj.DestinationRtuID + "->" + commmsg.Type.ToString() + "指令");
                    }
                }
            }
            foreach (string deletekey in deleteList)
            {

                _commandLists.Remove(deletekey);
            }
        }
        /// <summary>
        /// 由rtuID 查找其communication id
        /// </summary>
        /// <param name="rtuID">rtu id</param>
        /// <returns></returns>
        private int RtuIDtoCommunicationID(string rtuID)
        {

            //从数据库中查出该rtu的类型
          
            RTUSetting rs = _rwDatabase.LocalSettingManager.LoadRTUSetting(rtuID);
            if (rs != null)
            {
                
                return rs.CommunicationId;
            }

            return -1;
        }

       

    }
}
