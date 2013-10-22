using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataEntity;
using System.Data;
using MtuConsole.Common;

namespace DataAccess
{
    public class SettingManager
    {
        private ISettingPersistenceContext _context;
        private List<RTULog> _rtulogqueue;
        private MtuLog _logger;
        private RtuLogDataSaver _logSaver;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">sqlserver数据库存储参数</param>
        public SettingManager(SqlServerSettingPersistenceContext context)
        {
            _logger = new MtuLog();
            _context = context;
       
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">SQLite文件存储参数</param>
        public SettingManager(SqliteSettingPersistenceContext context)
        {
            _logger = new MtuLog();
            _context = context;
            _rtulogqueue = new List<RTULog>();
            _logSaver = new RtuLogDataSaver(this);
            _logSaver.StartWork();


        }
        #region 服务重要日志
        

        /// <summary>
        /// insert service note
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool BulkInsertServiceNote(IEnumerable< ServiceNote> entitys)
        {
            try
            {
                return _context.GetRepository().BulkInsertServiceNote(entitys);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }

        }
        /// <summary>
        /// load service note
        /// </summary>
        /// <returns></returns>
        public DataTable LoadServiceNote()
        {
            DataTable result = _context.GetRepository().LoadServiceNote();


            return result;
        }

        #endregion

        #region  NewDLA新增
        public bool InsertRtuSetting_Device(Rtusetting_Device entity)
        {
            bool result = false;
            try
            {
                result = _context.GetRepository().InsertRtuSetting_Device(entity);
             }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;

        }

        /// <summary>
        /// load rtusetting_device list
        /// </summary>
        /// <returns></returns>
        public List<Rtusetting_Device> LoadRtuSetting_Device()
        {
            List<Rtusetting_Device> result = new List<Rtusetting_Device>();

            try
            {
                result = _context.GetRepository().LoadRtuSetting_Device();
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
            }
        
            return result;
        }

        public Rtusetting_Device LoadRtuSetting_Device(string rtuid)
        {
            Rtusetting_Device result = new Rtusetting_Device();
            try
            {
                result = _context.GetRepository().LoadRtuSetting_Device(rtuid);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }


            return result;
        }
        #endregion

        #region DLG附加配置
        public DataTable LoadDeviceTable(string rtuid)
        { 
            DataTable result=new DataTable();
            try
            {
                 result = _context.GetRepository().LoadDeviceTable(rtuid);
            
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }


        public DataTable LoadDatablock(string rtuid)
        {
            DataTable result = new DataTable();

            try
            {
                result = _context.GetRepository().LoadDatablock(rtuid);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;

        }


        public DataTable LoadDLGDataType()
        {
            DataTable result = new DataTable();
            try
            {
                result = _context.GetRepository().LoadDLGDataType();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }

        /// <summary>
        /// 批量插入datablock数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool BulkInsertDatablock(IEnumerable<DatablockSetting> entitys)
        {
            bool result = false;

            try
            {
                result = _context.GetRepository().BulkInsertDatablock(entitys);
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }
        public string InsertDatablock(DatablockSetting entity)
        {
            string result = string.Empty;

            try
            {
                result = _context.GetRepository().InsertDatablock(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }

        public string InsertDevice(DeviceSetting entity)
        {
            string result = string.Empty;

            try
            {
                result = _context.GetRepository().InsertDevice(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }
        /// <summary>
        /// 修改datablocksetting
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool UpdateDatablock(DatablockSetting entity)
        {
            bool result = false;

            try
            {
                result = _context.GetRepository().UpdateDatablock(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }

    

        /// <summary>
        /// 删除datablock记录,by rtuid
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
       public bool DeleteDataBlock(string rtuid)
       {
           bool result = false;

           try
           {
               result = _context.GetRepository().DeleteDataBlock(rtuid);
           }
           catch (Exception e)
           {
               _logger.Error(e.Message, e);
           }

           return result;
       }

       public bool DeleteDataBlock(int blockid)
       {
           bool result = false;

           try
           {
               result = _context.GetRepository().DeleteDataBlock(blockid);
           }
           catch (Exception e)
           {
               _logger.Error(e.Message, e);
           }

           return result;
       }

       /// <summary>
       /// 批量插入devicetable
       /// </summary>
       /// <param name="entity"></param>
       /// <returns></returns>
       public bool BulkInsertDeviceTable(IEnumerable<DeviceSetting> entitys)
       {
           bool result = false;

           try
           {
               result = _context.GetRepository().BulkInsertDeviceTable(entitys);
           }
           catch (Exception e)
           {
               _logger.Error(e.Message, e);
           }

           return result;
       }

        /// <summary>
        /// 修改devicesetting
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool UpdateDeviceSetting(DeviceSetting entity)
       {
           bool result = false;

           try
           {
               result = _context.GetRepository().UpdateDeviceSetting(entity);
           }
           catch (Exception e)
           {
               _logger.Error(e.Message, e);
           }

           return result;
       }

       public bool DeleteDeviceSetting(string rtuid)
       {
           bool result = false;

           try
           {
               result = _context.GetRepository().DeleteDeviceSetting(rtuid);
           }
           catch (Exception e)
           {
               _logger.Error(e.Message, e);
           }

           return result;
       }
       public bool DeleteDeviceSetting(int deviceid)
       {
           bool result = false;

           try
           {
               result = _context.GetRepository().DeleteDeviceSetting(deviceid);
           }
           catch (Exception e)
           {
               _logger.Error(e.Message, e);
           }

           return result;
       }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        public bool InsertUnconfiguredRtu(UnConfiguredRTU entity)
        {
            try
            {
                return _context.GetRepository().InsertUnconfiguredRtu(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        public UnConfiguredRTU LoadUnconfiguredRtu(string rtuid)
        {
            UnConfiguredRTU result;
            try
            {

                result = _context.GetRepository().LoadUnconfiguredRtu(rtuid);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                result = null;

            }
            return result;
        }

        #endregion

        #region 气压表
        /// <summary>
        /// 新增气压信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertAirPressure(AirPressure entity)
        { 
        
          //  return _context.GetRepository().in
            try
            {
                return _context.GetRepository().InsertAirPressure(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// 读取airpressure
        /// </summary>
        /// <returns></returns>
        public DataTable LoadAirPressure()
        {
            DataTable result = _context.GetRepository().LoadAirPressure();

            return result;
        }

        #endregion

        #region 通讯部分

        /// <summary>
        /// 新增通道信息
        /// </summary>
        /// <param name="entity">通道实体类</param>
        /// <returns>通道编号</returns>
        public string InsertCommunicationSetting(CommunicationSetting entity)
        {
            try
            {
                return _context.GetRepository().InsertCommunicationSetting(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return null;
            }
        }

        /// <summary>
        /// 批量新增通道信息
        /// </summary>
        /// <param name="entities">通道实体类组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertCommunicationSetting(IEnumerable<CommunicationSetting> entities)
        {
            try
            {
                return _context.GetRepository().BulkInsertCommunicationSetting(entities);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// 更新通道信息
        /// </summary>
        /// <param name="entity">通道实体类</param>
        /// <returns>bool型</returns>
        public bool UpdateCommunicationSetting(CommunicationSetting entity)
        {
            try
            {
                return _context.GetRepository().UpdateCommunicationSetting(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }
        
        /// <summary>
        /// 删除通道信息
        /// </summary>
        /// <param name="CommunicationId">通道编号</param>
        /// <returns>bool型</returns>
        public bool DeleteCommunicationSetting(string CommunicationId)
        {
            try
            {
                return _context.GetRepository().DeleteCommunicationSetting(CommunicationId);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// 获取通道信息
        /// </summary>
        /// <param name="communicationId">通道Id</param>
        /// <returns>通道实体类</returns>
        public CommunicationSetting LoadCommunicationSetting(string communicationId)
        {
            return _context.GetRepository().LoadCommunicationSetting(communicationId);
        }
        /// <summary>
        /// 获取通道信息
        /// </summary>
        /// <returns>通道实体类</returns>
        public DataTable LoadCommunicationSetting()
        {
            return _context.GetRepository().LoadCommunicationSetting();
        }

        #endregion

        #region 终端部分

        /// <summary>
        /// 新增终端信息
        /// </summary>
        /// <param name="entity">终端实体类</param>
        /// <returns>终端信息</returns>
        public bool InsertRTUSetting(RTUSetting entity)
        {
            try
            {
                RTUTemplet templet = _context.GetRepository().LoadRtuTemplet(entity.ProductTypeId);
                if (templet != null)
                    entity.AddInfoByTemplet(templet);

                return _context.GetRepository().InsertRTUSetting(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);                
                return false;
            }
        }



        
        /// <summary>
        /// 批量新增终端信息
        /// </summary>
        /// <param name="entities">终端实体类组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertRTUSetting(IEnumerable<RTUSetting> entities)
        {
            try
            {
                string productTypeId = string.Empty;
                RTUTemplet templet = null;
                foreach (var entity in entities)
                {
                    if (string.IsNullOrEmpty(productTypeId))
                    {
                        productTypeId = entity.ProductTypeId;
                        templet = _context.GetRepository().LoadRtuTemplet(productTypeId);
                    }
                    if (templet != null)
                        entity.AddInfoByTemplet(templet);
                }

                return _context.GetRepository().BulkInsertRTUSetting(entities);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }

        }

        /// <summary>
        /// 更新终端信息
        /// </summary>
        /// <param name="entity">终端实体类</param>
        /// <returns>bool型</returns>
        public bool UpdateRTUSetting(RTUSetting entity)
        {
            try
            {
                return _context.GetRepository().UpdateRTUSetting(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        
        /// <summary>
        /// 删除终端信息
        /// </summary>
        /// <param name="RtuId">终端编号</param>
        /// <returns>bool型</returns>
        public bool DeleteRTUSetting(string RtuId)
        {
            try
            {
                return _context.GetRepository().DeleteRTUSetting(RtuId);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// 获取终端信息
        /// </summary>
        /// <param name="rtuId">终端Id</param>
        /// <returns>终端实体类</returns>
        public RTUSetting LoadRTUSetting(string rtuId)
        {
            return _context.GetRepository().LoadRTUSetting(rtuId);
        }


        /// <summary>
        /// 获取口径表
        /// </summary>
        /// <returns></returns>
        public DataTable LoadCaliberAndFlux()
        {
            return _context.GetRepository().LoadCaliberAndFlux();
        }

        /// <summary>
        /// 获取终端信息
        /// </summary>
        /// <returns>终端实体类</returns>
        public DataTable LoadRTUSetting()
        {
            return _context.GetRepository().LoadRTUSetting();
        }

        /// <summary>
        /// 获取新的RTU编号
        /// </summary>
        /// <returns>RTU编号</returns>
        public string GetNewRtuId()
        {
            return _context.GetRepository().GetNewRtuId();
        }

        /// <summary>
        /// 检索RTU编号是否存在
        /// </summary>
        /// <param name="rtuId">RTU编号</param>
        /// <returns>bool型</returns>
        public bool Exists(string rtuId)
        {
            try
            {
                return _context.GetRepository().Exists(rtuId);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }
        #endregion

        #region 检测量配置部分

        /// <summary>
        /// 新增检测量信息
        /// </summary>
        /// <param name="entity">检测量实体类</param>
        /// <returns>检测量编号</returns>
        public string InsertMeasureSetting(MeasureSetting entity)
        {
            try
            {
                return _context.GetRepository().InsertMeasureSetting(entity);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return null;
            }
        }

        /// <summary>
        /// 批量新增检测量信息
        /// </summary>
        /// <param name="entities">检测量实体类组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertMeasureSetting(IEnumerable<MeasureSetting> entities)
        {
            try
            {
                return _context.GetRepository().BulkInsertMeasureSetting(entities);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// 更新检测量信息
        /// </summary>
        /// <param name="entity">检测量实体类</param>
        /// <returns>bool型</returns>
        public bool UpdateMeasureSetting(MeasureSetting entity)
        {
            try
            {
                return _context.GetRepository().UpdateMeasureSetting(entity);
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// 删除检测量信息
        /// </summary>
        /// <param name="entity">检测量实体类</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureSetting(MeasureSetting entity)
        {
            try
            {
                return _context.GetRepository().DeleteMeasureSetting(entity);
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        public bool DeleteMeasureSetting(IEnumerable<MeasureSetting> entities)
        {
            try
            {
                return _context.GetRepository().DeleteMeasureSetting(entities);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }

        /// <summary>
        /// 获取检测量信息
        /// </summary>
        /// <param name="measureSettingId">检测量Id</param>
        /// <returns>检测量实体类</returns>
        public MeasureSetting LoadMeasureSetting(string measureSettingId)
        {
            return _context.GetRepository().LoadMeasureSetting(measureSettingId);
        }
        /// <summary>
        /// 获取检测量信息
        /// </summary>
        /// <returns>检测量实体类</returns>
        public DataTable LoadMeasureSetting()
        {
            return _context.GetRepository().LoadMeasureSetting();
        }

        /// <summary>
        /// 根据RTU编号获取检测量配置
        /// </summary>
        /// <param name="rtuId">RTU编号</param>
        /// <returns>检测量配置</returns>
        public DataTable LoadMeasureSettingByRtuId(string rtuId)
        {
            return _context.GetRepository().LoadMeasureSettingByRtuId(rtuId);
        }

        /// <summary>
        /// 根据RTU编号获取检测量配置
        /// </summary>
        /// <param name="rtuId">RTU编号</param>
        /// <returns>检测量配置</returns>
        public List<MeasureSetting> LoadMeasSettingByRtuId(string rtuId)
        {
            return _context.GetRepository().LoadMeasSettingByRtuId(rtuId);
        }

        #endregion

        #region 模板部分

        /// <summary>
        /// 根据产品类型编号获取检测量模板
        /// </summary>
        /// <param name="productTypeId">产品类型编号</param>
        /// <returns>检测量模板集合</returns>
        public List<MeasureTemplet> LoadMeasureTemplet(string productTypeId)
        {
            return _context.GetRepository().LoadMeasureTempletByProductTypeId(productTypeId);
        }

        /// <summary>
        /// 根据产品类型编号获取终端模板
        /// </summary>
        /// <param name="productTypeId">产品类型编号</param>
        /// <returns>终端模板</returns>
        public RTUTemplet LoadRtuTemplet(string productTypeId)
        {
            return _context.GetRepository().LoadRtuTemplet(productTypeId);
        }

        /// <summary>
        /// 获取所有模板（Rtu和Measure）
        /// </summary>
        /// <returns>Rtu和Measure关联模板</returns>
        public DataTable LoadAllTemplet()
        {
            return _context.GetRepository().LoadAllTemplet();
        }


        /// <summary>
        /// 修改rtu模板
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateRtuTemplet(RTUTemplet entity)
        {
            bool result = false;

            try
            { 
                result =_context.GetRepository().UpdateRtuTemplet(entity);
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                result=false;
            }

            return result;
        }

        /// <summary>
        /// 新增检测量模板
        /// </summary>
        /// <param name="entity">检测量模板类</param>
        /// <returns>bool型</returns>
        public bool InsertMeasureTemplet(MeasureTemplet entity)
        {
            bool result = false;

            try
            {
                result = _context.GetRepository().InsertMeasureTemplet(entity);
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 删除单条检测量模板信息
        /// </summary>
        /// <param name="entity">检测量模板信息</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureTemplet(MeasureTemplet entity)
        {

            bool result = false;
            try
            {
                result = _context.GetRepository().DeleteMeasureTemplet(entity);
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 更新单条检测量模板信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateMeasureTemplet(MeasureTemplet entity)
        {
            try
            {
                return _context.GetRepository().UpdateMeasureTemplet(entity);
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }


        #endregion

        #region  其余配置
        /// <summary>
        /// 获取产品类型编号集合
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable LoadProductTypeId()
        {
            return _context.GetRepository().LoadProductTypeId();
        }
        /// <summary>
        ///  获取产品类型编号集合
        /// </summary>
        /// <param name="communicationTypeId">通讯协议类型</param>
        /// <returns>产品类型编号</returns>
        public string LoadProductTypeId(string communicationTypeId)
        {
            return _context.GetRepository().LoadProductTypeId(communicationTypeId);
        }

        /// <summary>
        /// 获取通讯协议类型
        /// </summary>
        /// <returns>通讯协议类型</returns>
        public List<CommunicationType> LoadCommunicationType()
        {
            return _context.GetRepository().LoadCommunicationType();
        }

        /// <summary>
        /// 获取数据类型配置
        /// </summary>
        /// <returns>数据类型配置</returns>
        public List<DataTypeConfig> LoadDataTypeConfig()
        {
            return _context.GetRepository().LoadDataTypeConfig();
        }

        /// <summary>
        /// 获取端口号
        /// </summary>
        /// <param name="productTypeId">产品型号</param>
        /// <returns>端口号</returns>
        public List<string> LoadPortIdByProductType(string productTypeId)
        {
            return _context.GetRepository().LoadPortIdByProductType(productTypeId);
        }
        #endregion

        #region 配置操作痕迹记录
        /// <summary>
        /// 配置操作痕迹记录
        /// </summary>
        /// <param name="configTrack">配置操作类</param>
        /// <returns>bool型</returns>
        public bool InsertConfigTraceLog(ConfigTrack configTrack)
        {
            try
            {
                return _context.GetRepository().InsertConfigTraceLog(configTrack);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }

        }

        #endregion

        #region 用户信息
        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>Bool型</returns>
        public bool IsUserExist(UserInfo userInfo)
        {
            try
            {
                return _context.GetRepository().IsUserExist(userInfo);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return false;
            }
        }
        #endregion

        #region 液位仪对应关系配置
        public Dictionary<string, int> LoadRtuObjectRelation()
        {
            return _context.GetRepository().LoadRtuObjectRelation();
        }

        public Dictionary<int, int> LoadMeasureTypeRelation()
        {
            return _context.GetRepository().LoadMeasureTypeRelation();
        }
        #endregion

        public void AddToWrite(RTULog item)
        {
            _rtulogqueue.Add(item);
        }
        public bool DeleteRTULog(string rtuid)
        {
            return _context.GetRepository().DeleteRTULog(rtuid);
        }

        public bool InsertRTULog(RTULog rtulogs)
        {
            return _context.GetRepository().InsertRTULog(rtulogs);
        }

        public List<RTULog> LoadAllRTULog() 
        {
            return _context.GetRepository().LoadAllRTULog();
        }

        public void BulkInsertRTULog(IEnumerable<RTULog> rtulogs)
        {
             _context.GetRepository().BulkInsertRTUlog(rtulogs);
        }
        public List<RTULog> GetRtuLogQueue()
        {
            return _rtulogqueue;
        }
        /// <summary>
        /// 线程结束命令，调用该方法确保队列中的数据持久化到数据库
        /// </summary>
        public void EnsureQueueDataPersisted()
        {
            _logSaver.SetWorkFlag(false);
            _logSaver.GetWorkerThread().Join();
        }
       
    }
}
