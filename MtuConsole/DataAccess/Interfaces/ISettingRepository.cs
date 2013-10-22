using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface ISettingRepository
    {
        
        /// <summary>
        /// insert service note
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool BulkInsertServiceNote(IEnumerable<ServiceNote> entitys);

        /// <summary>
        /// load service note
        /// </summary>
        /// <returns></returns>
        DataTable LoadServiceNote();

        /// <summary>
        /// load devicetable
        /// </summary>
        /// <returns></returns>
        DataTable LoadDeviceTable(string rtuid);
        /// <summary>
        /// load datablock
        /// </summary>
        /// <returns></returns>
        DataTable LoadDatablock(string rtuid);
        /// <summary>
        /// load dlg datatype
        /// </summary>
        /// <returns></returns>
        DataTable LoadDLGDataType();

        bool BulkInsertDatablock(IEnumerable<DatablockSetting> entitys);
        bool UpdateDatablock(DatablockSetting entity);
        bool DeleteDataBlock(string rtuid);
        bool DeleteDataBlock(int blockid);
        bool BulkInsertDeviceTable(IEnumerable<DeviceSetting> entitys);
        bool UpdateDeviceSetting(DeviceSetting entity);
        bool DeleteDeviceSetting(string rtuid);
        bool DeleteDeviceSetting(int blockid);
        string InsertDatablock(DatablockSetting entity);
        string InsertDevice(DeviceSetting entity);


        bool InsertUnconfiguredRtu(UnConfiguredRTU entity);
        UnConfiguredRTU LoadUnconfiguredRtu(string rtuid);
        /// <summary>
        /// 新增气压信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool InsertAirPressure(AirPressure entity);

         /// <summary>
        /// 读取airpressure
        /// </summary>
        /// <returns></returns>
        DataTable LoadAirPressure();


        /// <summary>
        /// 新增通道信息
        /// </summary>
        /// <param name="entity">通道实体类</param>
        /// <returns>bool型</returns>
        string InsertCommunicationSetting(CommunicationSetting entity);

        /// <summary>
        /// 批量新增通道信息
        /// </summary>
        /// <param name="entities">通道实体类组</param>
        /// <returns>bool型</returns>
        bool BulkInsertCommunicationSetting(IEnumerable<CommunicationSetting> entity);

        /// <summary>
        /// 更新通道信息
        /// </summary>
        /// <param name="entity">通道实体类</param>
        /// <returns>bool型</returns>
        bool UpdateCommunicationSetting(CommunicationSetting entity);

        /// <summary>
        /// 删除通道信息
        /// </summary>
        /// <param name="CommunicationId">通道编号</param>
        /// <returns>bool型</returns>
        bool DeleteCommunicationSetting(string CommunicationId);

        /// <summary>
        /// 根据通道编号获取通道信息
        /// </summary>
        /// <param name="communicationId">通道主键</param>
        /// <returns>通道实体类</returns>
        CommunicationSetting LoadCommunicationSetting(string key);

        /// <summary>
        /// 获取所有通道信息
        /// </summary>
        /// <returns>通道信息集合</returns>
        DataTable LoadCommunicationSetting();

        /// <summary>
        /// 获取所有通道信息
        /// </summary>
        /// <returns>通道信息列表</returns>
        List<CommunicationSetting> LoadCommunicationSettingList();

        /// <summary>
        /// 新增终端信息
        /// </summary>
        /// <param name="entity">终端实体类</param>
        /// <returns>终端编号</returns>
        bool InsertRTUSetting(RTUSetting entity);

        /// <summary>
        /// 批量新增终端信息
        /// </summary>
        /// <param name="entities">终端实体类组</param>
        /// <returns>bool型</returns>
        bool BulkInsertRTUSetting(IEnumerable<RTUSetting> entities);

        /// <summary>
        /// 更新终端信息
        /// </summary>
        /// <param name="entity">终端实体类</param>
        /// <returns>bool型</returns>
        bool UpdateRTUSetting(RTUSetting entity);

        /// <summary>
        /// 删除终端信息
        /// </summary>
        /// <param name="RtuId">终端编号</param>
        /// <returns>bool型</returns>
        bool DeleteRTUSetting(string RtuId);

        /// <summary>
        /// 获取终端信息
        /// </summary>
        /// <param name="RtuId">终端主键</param>
        /// <returns>终端实体类</returns>
        RTUSetting LoadRTUSetting(string key);

           /// <summary>
        /// 获取口径表
        /// </summary>
        /// <returns></returns>
        DataTable LoadCaliberAndFlux();

        /// <summary>
        /// 获取所有终端信息
        /// </summary>
        /// <returns>终端信息集合</returns>
        DataTable LoadRTUSetting();

        /// <summary>
        /// 获取所有终端信息
        /// </summary>
        /// <returns>终端信息集合</returns>
        List<RTUSetting> LoadRTUSettingList();

        /// <summary>
        /// 新增检测量配置
        /// </summary>
        /// <param name="entity">检测量配置实体类</param>
        /// <returns>bool型</returns>
        string InsertMeasureSetting(MeasureSetting entity);

        /// <summary>
        /// 批量新增检测量配置
        /// </summary>
        /// <param name="entities">检测量配置实体类组</param>
        /// <returns>bool型</returns>
        bool BulkInsertMeasureSetting(IEnumerable<MeasureSetting> entities);

        /// <summary>
        /// 更新检测量配置
        /// </summary>
        /// <param name="entity">检测量配置实体类</param>
        /// <returns>bool型</returns>
        bool UpdateMeasureSetting(MeasureSetting entity);

        /// <summary>
        /// 删除检测量配置信息
        /// </summary>
        /// <param name="entity">检测量配置实体类</param>
        /// <returns>bool型</returns>
        bool DeleteMeasureSetting(MeasureSetting entity);

        /// <summary>
        /// 批量删除检测量配置信息
        /// </summary>
        /// <param name="entity">检测量配置实体类组</param>
        /// <returns>bool型</returns>
        bool DeleteMeasureSetting(IEnumerable<MeasureSetting> entities);

        /// <summary>
        /// 删除检测量配置信息
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>bool型</returns>
        bool DeleteMeasureSetting(string condition);

        /// <summary>
        /// 根据编号获取检测量配置
        /// </summary>
        /// <param name="measureId">检测量编号</param>
        /// <returns>检测量配置实体类</returns>
        MeasureSetting LoadMeasureSetting(string measureId);

        /// <summary>
        /// 获取所有检测量配置
        /// </summary>
        /// <returns>检测量配置集合</returns>
        DataTable LoadMeasureSetting();

        /// <summary>
        /// 获取所有检测量配置
        /// </summary>
        /// <returns>检测量配置集合</returns>
        List<MeasureSetting> LoadMeasureSettingList();
        
        /// <summary>
        /// 获取所有检测量配置
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>检测量配置集合</returns>
        DataTable LoadMeasureSettingByRtuId(string rtuId);

        /// <summary>
        /// 获取所有检测量配置
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>检测量配置集合</returns>
        List<MeasureSetting> LoadMeasSettingByRtuId(string rtuId);

        /// <summary>
        /// 获取产品类型编号集合
        /// </summary>
        /// <returns>DataTable</returns>
        DataTable LoadProductTypeId();

        /// <summary>
        ///  获取产品类型编号集合
        /// </summary>
        /// <param name="communicationTypeId">通讯协议类型</param>
        /// <returns>产品类型编号</returns>
        string LoadProductTypeId(string communicationTypeId);

        /// <summary>
        /// 获取终端模板
        /// </summary>
        /// <param name="productTypeId">产品类型编号</param>
        /// <returns>终端模板</returns>
        RTUTemplet LoadRtuTemplet(string productTypeId);

        /// <summary>
        /// 获取检测量模板
        /// </summary>
        /// <param name="productTypeId">产品编号</param>
        /// <returns>检测量模板集合</returns>
        List<MeasureTemplet> LoadMeasureTempletByProductTypeId(string productTypeId);

        /// <summary>
        /// 获取所有模板（Rtu和Measure）
        /// </summary>
        /// <returns></returns>
        DataTable LoadAllTemplet();

        /// <summary>
        /// 新增操作痕迹
        /// </summary>
        /// <param name="entity">操作痕迹实体类</param>
        /// <returns>bool型</returns>
        bool InsertConfigTraceLog(ConfigTrack entity);

        /// <summary>
        /// 删除操作痕迹
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>bool型</returns>
        bool DeleteConfigTraceLog(string condition);

        /// <summary>
        /// 检索编号是否存在
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>bool型</returns>
        bool Exists(string rtuId);

        /// <summary>
        /// 获取最新终端编号
        /// </summary>
        /// <returns>终端编号</returns>
        string GetNewRtuId();

        /// <summary>
        /// 获取通讯协议类型
        /// </summary>
        /// <returns>通讯协议类型</returns>
        List<CommunicationType> LoadCommunicationType();

        /// <summary>
        /// 获取数据类型配置
        /// </summary>
        /// <returns>数据类型配置</returns>
        List<DataTypeConfig> LoadDataTypeConfig();

        /// <summary>
        /// 获取端口号
        /// </summary>
        /// <param name="productTypeId">产品型号</param>
        /// <returns>端口号</returns>
        List<string> LoadPortIdByProductType(string productTypeId);

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>Bool型</returns>
        bool IsUserExist(UserInfo userInfo);

        /// <summary>
        /// 修改终端模板
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateRtuTemplet(RTUTemplet entity);

           /// <summary>
        /// 新增检测量模板
        /// </summary>
        /// <param name="entity">检测量模板类</param>
        /// <returns>bool型</returns>
         bool InsertMeasureTemplet(MeasureTemplet entity);

         /// <summary>
        /// 删除单条检测量模板信息
        /// </summary>
        /// <param name="entity">检测量模板信息</param>
        /// <returns>bool型</returns>
         bool DeleteMeasureTemplet(MeasureTemplet entity);

        /// <summary>
        /// 更新单条检测量模板信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         bool UpdateMeasureTemplet(MeasureTemplet entity);

         Dictionary<string, int> LoadRtuObjectRelation();
         Dictionary<int, int> LoadMeasureTypeRelation();

        /// <summary>
        /// 删除指定RTU的Log信息
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
         bool DeleteRTULog(string rtuid);

        /// <summary>
        /// 插入RTULog, 记录终端某一时刻连接的IP地址
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
         bool InsertRTULog(RTULog rtulogs);

        /// <summary>
        /// 加载所有有过通讯的终端的通讯记录；
        /// </summary>
        /// <returns></returns>
         List<RTULog> LoadAllRTULog();

         bool InsertRtuSetting_Device(Rtusetting_Device entity);
         Rtusetting_Device LoadRtuSetting_Device(string rtuid);
         List<Rtusetting_Device> LoadRtuSetting_Device();
         void BulkInsertRTUlog(IEnumerable<RTULog> rtulogs);
    }
}
