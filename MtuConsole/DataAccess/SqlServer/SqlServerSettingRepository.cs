using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataEntity;
using DataAccess.Interfaces;
using DataAccess;
using MtuConsole.Common;

namespace DataAccess.SqlServer
{
    public class SqlServerSettingRepository : SqlServerRepositoryBase, ISettingRepository
    {
          private MtuLog _logger;

         
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerSettingRepository(string connectionString) : base(connectionString) 
        {
            _logger = new MtuLog();
        }

        #region ISettingRepository Members
        #region newdla
        
        public bool InsertRtuSetting_Device(Rtusetting_Device entity)
        { return false; }

        public List<Rtusetting_Device> LoadRtuSetting_Device()
        {
            return null;
        }
        public  Rtusetting_Device LoadRtuSetting_Device(string rtuid)
        {
            return null;
        }
        #endregion
        #region service note
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool BulkInsertServiceNote(IEnumerable<ServiceNote> entities)
        {
            bool result = true;

            DataTable dt = new DataTable();
            dt.CaseSensitive = true;
            dt.Columns.Add("Notetime", typeof(DateTime));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("NoteType", typeof(int));
            dt.Columns.Add("State", typeof(int));

            foreach (ServiceNote ent in entities)
            {
                DataRow dr = dt.NewRow();
                dr["Notetime"] = ent.NoteTime;
                dr["Note"] = ent.Note;
                dr["NoteType"] = ent.DataType;
                dr["State"] = ent.State;    
                dt.Rows.Add(dr);
            }

            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "ServiceNote";
                    bulkCopy.ColumnMappings.Add("Notetime", "Notetime");
                    bulkCopy.ColumnMappings.Add("Note", "Note");
                    bulkCopy.ColumnMappings.Add("NoteType", "NoteType");
                    bulkCopy.ColumnMappings.Add("State", "State");
                    conn.Open();
                    bulkCopy.WriteToServer(dt);
                }
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }

        }


        /// <summary>
        /// load service note
        /// </summary>
        /// <returns></returns>
        public DataTable LoadServiceNote()
        {
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, "select * from servicenote");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #endregion

        #region DLG附加
        /// <summary>
        /// load devicetable
        /// </summary>
        /// <returns></returns>
       public DataTable LoadDeviceTable(string rtuid)
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultDeviceFields, SQLItems.DefaultDeviceTableName,"rtuid='" + rtuid + "'", string.Empty);
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// load datablock
        /// </summary>
        /// <returns></returns>
       public   DataTable LoadDatablock(string rtuid)
        {
            string sql = "select * from {0} where deviceid in (select deviceid from {1} where rtuid='{2}')";
            sql = string.Format(sql, SQLItems.DefaultDatablockTableName, SQLItems.DefaultDeviceTableName, rtuid);
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text,sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

       public DataTable LoadDLGDataType()
       {
           string sql = SQLItems.CreateSelectSql(SQLItems.DefaultDLGDatatypeFields, SQLItems.DefaultDLGDatatypeTableName, string.Empty, string.Empty);
           DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql);
           if (ds != null && ds.Tables.Count > 0)
           {
               return ds.Tables[0];
           }
           return null;
       }
        /// <summary>
        /// 空方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool InsertUnconfiguredRtu(UnConfiguredRTU entity)
       {
           throw new Exception();
       }

       public UnConfiguredRTU LoadUnconfiguredRtu(string rtuid)
       {
           throw new Exception();
       }

       public bool BulkInsertDatablock(IEnumerable<DatablockSetting> entitys)
       {
           bool result = true;

           DataTable dt = new DataTable();
           dt.CaseSensitive = true;
           dt.Columns.Add("BlockID", typeof(int));
           dt.Columns.Add("MeasureId", typeof(int));
           dt.Columns.Add("DeviceId", typeof(int));
           dt.Columns.Add("DataTypeId", typeof(int));
           dt.Columns.Add("Swap", typeof(int));
           dt.Columns.Add("Start", typeof(int));
           dt.Columns.Add("Length", typeof(int));
           dt.Columns.Add("Digit", typeof(int));


           foreach (DatablockSetting ent in entitys)
           {
               DataRow dr = dt.NewRow();
               dr["BlockID"] = ent.DataBlockID;
               dr["MeasureId"] = ent.Measureid;
               dr["DeviceId"] = ent.DeviceID;
               dr["DataTypeId"] = ent.DatatypeID;
               dr["Swap"] = ent.Swap;
               dr["Start"] = ent.Start;
               dr["Length"] = ent.Length;
               dr["Digit"] = ent.Digit;
               dt.Rows.Add(dr);
           }

           try
           {
               using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
               using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
               {
                   bulkCopy.DestinationTableName =SQLItems.DefaultDatablockTableName;
                   bulkCopy.ColumnMappings.Add("BlockId", "BlockId");
                   bulkCopy.ColumnMappings.Add("MeasureId", "MeasureId");
                   bulkCopy.ColumnMappings.Add("DeviceId", "DeviceId");
                   bulkCopy.ColumnMappings.Add("Swap", "Swap");
                   bulkCopy.ColumnMappings.Add("Start", "Start");
                   bulkCopy.ColumnMappings.Add("Length", "Length");

                   conn.Open();
                   bulkCopy.WriteToServer(dt);
               }
               return result;
           }
           catch (Exception e)
           {
               throw e;
           }
       }


     public  bool UpdateDatablock(DatablockSetting entity)
       {
           string sql = @"UPDATE " + SQLItems.DefaultDatablockTableName + " SET "
                   + (entity.CheckPropertyChanged("Measureid") ? "Measureid='{1}'" : "")
                   + (entity.CheckPropertyChanged("DeviceID") ? ",DeviceID='{2}'" : "")
                   + (entity.CheckPropertyChanged("DatatypeID") ? ",DatatypeID='{3}'" : "")
                   + (entity.CheckPropertyChanged("Swap") ? ",Swap='{4}'" : "")
                   + (entity.CheckPropertyChanged("Start") ? ",Start='{5}'" : "")
                   + (entity.CheckPropertyChanged("Length") ? ",Length='{6}'" : "")
                    + (entity.CheckPropertyChanged("Digit") ? ",Digit='{7}'" : "")
                   + " WHERE BlockID={0}";
           sql = string.Format(sql, entity.DataBlockID, entity.Measureid, entity.DeviceID, entity.DatatypeID, entity.Swap, entity.Start, entity.Length,entity.Digit);
           return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
       }
     public  bool DeleteDataBlock(string rtuid)
     {
         string sql = "delete from {0} where deviceid in (select deviceid from {1} where rtuid='{2}')";
         sql = string.Format(sql, SQLItems.DefaultDatablockTableName, SQLItems.DefaultDeviceTableName, rtuid);
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
     }
     public bool DeleteDataBlock(int blockid)
     {
         string sql = "delete from {0} where blockid='{1}'";
         sql = string.Format(sql, SQLItems.DefaultDatablockTableName, blockid.ToString());
         return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
     }

      public bool BulkInsertDeviceTable(IEnumerable<DeviceSetting> entitys)
       {
           bool result = true;

           DataTable dt = new DataTable();
           dt.CaseSensitive = true;
           dt.Columns.Add("DeviceId", typeof(int));
           dt.Columns.Add("RtuId", typeof(string));
           dt.Columns.Add("DeviceName", typeof(string));
           dt.Columns.Add("CommandCount",typeof(int));
     


           foreach (DeviceSetting ent in entitys)
           {
               DataRow dr = dt.NewRow();
               dr["DeviceId"] = ent.DeviceID;
               dr["RtuId"] = ent.RtuID;
               dr["DeviceName"] = ent.DeviceName;
               dr["CommandCount"] = CommonMethod.ToInt(ent.CommandCount);
    
               dt.Rows.Add(dr);
           }

           try
           {
               using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
               using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
               {
                   bulkCopy.DestinationTableName = SQLItems.DefaultDeviceTableName;
                   bulkCopy.ColumnMappings.Add("DeviceId", "DeviceId");
                   bulkCopy.ColumnMappings.Add("RtuId", "RtuId");
                   bulkCopy.ColumnMappings.Add("DeviceName", "DeviceName");
                   bulkCopy.ColumnMappings.Add("CommandCount", "CommandCount");
                   conn.Open();
                   bulkCopy.WriteToServer(dt);
               }
               return result;
           }
           catch (Exception e)
           {
               throw e;
           }
       }
      public bool UpdateDeviceSetting(DeviceSetting entity)
      {
          string sql = @"UPDATE " + SQLItems.DefaultDeviceTableName + " SET "
                 + (entity.CheckPropertyChanged("RtuID") ? "RtuID='{1}'" : "")
                 + (entity.CheckPropertyChanged("DeviceName") ? ",DeviceName='{2}'" : "")
                 + (entity.CheckPropertyChanged("CommandCount") ? ",CommandCount='{3}'" : "")
                 + " WHERE DeviceID={0}";
          sql = string.Format(sql, entity.DeviceID, entity.RtuID, entity.DeviceName,entity.CommandCount);
          return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
      }
      public bool DeleteDeviceSetting(string rtuid)
      {
          string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultDeviceTableName, " rtuid='" + rtuid + "'");
          return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
      }
      public bool DeleteDeviceSetting(int deviceid)
      {
          string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultDeviceTableName, " deviceid='" + deviceid.ToString() + "'");
          return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
      }
      public string InsertDatablock(DatablockSetting entity)
      {
          string sql = @"insert into {0}  (MeasureId,DeviceId,DataTypeId,Swap,Start,Length,BlockID,Digit) values ('{1}','{2}','{3}','{4}','{5}','{6}',{7},{8})";

          //BlockID,MeasureId,DeviceId,DataTypeId,Swap,Start,Length
          sql = string.Format(sql, SQLItems.DefaultDatablockTableName,  entity.Measureid,
              entity.DeviceID,entity.DatatypeID, entity.Swap, entity.Start, entity.Length,entity.DataBlockID,entity.Digit);
          return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0 ? entity.DataBlockID.ToString() : null;

      }
      public string InsertDevice(DeviceSetting entity)
      {

          string sql = @"insert into {0}  (RtuID,DeviceName,DeviceID,CommandCount) values ('{1}','{2}',{3},'{4}')";
          sql = string.Format(sql, SQLItems.DefaultDeviceTableName, entity.RtuID, entity.DeviceName,entity.DeviceID,entity.CommandCount);
          return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0 ? entity.DeviceID.ToString() : null;
      }

        #endregion


        #region 气压信息
        /// <summary>
        /// 新增气压信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertAirPressure(AirPressure entity)
        {
           
            
            string sql = @"INSERT INTO {0} (collecttime,airpressure) values ('{1}','{2}') " ;
            sql=string.Format(sql, SQLItems.DefaultAirPressureTableName,entity.CollectTime.ToString(),entity.AirPressureValue.ToString());
                       
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
        }


        /// <summary>
        /// 读取airpressure
        /// </summary>
        /// <returns></returns>
        public DataTable LoadAirPressure()
        {
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, "select top 2000 *,datediff(second,'1970-01-01 00:00:00' ,CollectTime) as intcollecttime from " + SQLItems.DefaultAirPressureTableName + " order by collecttime desc");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #endregion

        #region 新增通道

        /// <summary>
        /// 新增通道信息（编号CommunicationId必须赋值）
        /// </summary>
        /// <param name="entity">通道实体类</param>
        /// <returns>成功返回编号，失败返回null</returns>
        public string InsertCommunicationSetting(CommunicationSetting entity)
        {
            string sql = @"INSERT INTO " + SQLItems.DefaultCommSettingTableName + @" ( " + SQLItems.DefaultCommSettingFields + @" ) 
                        VALUES(@CommunicationId,@CommunicationName,@MobileNo,@Note,@APN,@Dns1,@Dns2,@IP,@ExTcp,@InTcp,@CommunicationTypeId,@Tag)";
            SqlParameter[] para = this.CreateSqlParametersWithIdentity(entity);

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, para) > 0 ? entity.CommunicationId.ToString() : null;
           
        }

        /// <summary>
        /// 批量新增通道信息
        /// </summary>
        /// <param name="entities">通道实体类组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertCommunicationSetting(IEnumerable<CommunicationSetting> entities)
        {
            bool result = true;

            DataTable dt = new DataTable();
            dt.CaseSensitive = true;
            dt.Columns.Add("CommunicationId", typeof(int));
            dt.Columns.Add("CommunicationName", typeof(string));
            dt.Columns.Add("MobileNo", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("APN", typeof(string));
            dt.Columns.Add("Dns1", typeof(string));
            dt.Columns.Add("Dns2", typeof(string));
            dt.Columns.Add("IP", typeof(string));
            dt.Columns.Add("ExTcp", typeof(string));
            dt.Columns.Add("InTcp", typeof(string));
            dt.Columns.Add("CommunicationTypeId", typeof(string));
            dt.Columns.Add("Tag", typeof(byte));


            foreach (CommunicationSetting ent in entities)
            {
                DataRow dr = dt.NewRow();
                dr["CommunicationId"] = ent.CommunicationId;
                dr["CommunicationName"] = ent.CommunicationName;
                dr["MobileNo"] = ent.MobileNo;
                dr["Note"] = ent.Note;
                dr["APN"] = ent.APN;
                dr["Dns1"] = ent.Dns1;
                dr["Dns2"] = ent.Dns2;
                dr["IP"] = ent.IP;
                dr["ExTcp"] = ent.ExTcp;
                dr["InTcp"] = ent.InTcp;
                dr["CommunicationTypeId"] = ent.CommunicationTypeId;
                dr["Tag"] = ent.Tag;
                dt.Rows.Add(dr);
            }

            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = SQLItems.DefaultCommSettingTableName;
                    bulkCopy.ColumnMappings.Add("CommunicationId", "CommunicationId");
                    bulkCopy.ColumnMappings.Add("CommunicationName", "CommunicationName");
                    bulkCopy.ColumnMappings.Add("MobileNo", "MobileNo");
                    bulkCopy.ColumnMappings.Add("Note", "Note");
                    bulkCopy.ColumnMappings.Add("APN", "APN");
                    bulkCopy.ColumnMappings.Add("Dns1", "Dns1");
                    bulkCopy.ColumnMappings.Add("Dns2", "Dns2");
                    bulkCopy.ColumnMappings.Add("IP", "IP");
                    bulkCopy.ColumnMappings.Add("ExTcp", "ExTcp");
                    bulkCopy.ColumnMappings.Add("InTcp", "InTcp");
                    bulkCopy.ColumnMappings.Add("CommunicationTypeId", "CommunicationTypeId");
                    bulkCopy.ColumnMappings.Add("Tag", "Tag");

                    conn.Open();
                    bulkCopy.WriteToServer(dt);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #endregion

        #region 更新通道

        /// <summary>
        /// 更新通道信息
        /// </summary>
        /// <param name="entity">通道实体类</param>
        /// <returns>bool型</returns>
        public bool UpdateCommunicationSetting(CommunicationSetting entity)
        {

            string sql = @"UPDATE " + SQLItems.DefaultCommSettingTableName + " SET UpdateTime=getdate() "
                + (entity.CheckPropertyChanged("CommunicationName") ? ",CommunicationName=@CommunicationName" : "")
                + (entity.CheckPropertyChanged("MobileNo") ? ",MobileNo=@MobileNo" : "")
                + (entity.CheckPropertyChanged("Note") ? ",Note=@Note" : "")
                + (entity.CheckPropertyChanged("APN") ? ",APN=@APN" : "")
                + (entity.CheckPropertyChanged("Dns1") ? ",Dns1=@Dns1" : "")
                + (entity.CheckPropertyChanged("Dns2") ? ",Dns2=@Dns2" : "")
                + (entity.CheckPropertyChanged("IP") ? ",IP=@IP" : "")
                + (entity.CheckPropertyChanged("ExTcp") ? ",ExTcp=@ExTcp" : "")
                + (entity.CheckPropertyChanged("InTcp") ? ",InTcp=@InTcp" : "")
                + (entity.CheckPropertyChanged("CommunicationTypeId") ? ",CommunicationTypeId=@CommunicationTypeId" : "")
                + (entity.CheckPropertyChanged("Tag") ? ",Tag=@Tag" : "")
                + " WHERE CommunicationId=@CommunicationId";

            SqlParameter[] para = new SqlParameter[12];
            this.CreateSqlParameters(entity).CopyTo(para, 0);
            para[11] = new SqlParameter("@CommunicationId", entity.CommunicationId);

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, para) > 0;
        }

        #endregion

        #region 删除通道信息
        /// <summary>
        /// 删除通道信息
        /// </summary>
        /// <param name="CommunicationId">通道编号</param>
        /// <returns>bool型</returns>
        public bool DeleteCommunicationSetting(string CommunicationId)
        {
            string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultCommSettingTableName, " CommunicationId= " + CommunicationId );
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
        }
        #endregion

        #region 获取通道

        /// <summary>
        /// 获取所有通道信息
        /// </summary>
        /// <returns>通道信息集合</returns>
        public DataTable LoadCommunicationSetting()
        {
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, SQLItems.CreateSelectSql(SQLItems.DefaultCommSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultCommSettingTableName, null, null));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 获取通道信息
        /// </summary>
        /// <returns>通道列表</returns>
        public List<CommunicationSetting> LoadCommunicationSettingList()
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultCommSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultCommSettingTableName, null, null);

            List<CommunicationSetting> list = new List<CommunicationSetting>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(EntityAssembler.CreateCommSettingEntity(reader));
                }
            }
            return list;
        }

        /// <summary>
        /// 根据通道编号获取通道信息
        /// </summary>
        /// <param name="communicationId">通道主键</param>
        /// <returns>通道实体类</returns>
        public CommunicationSetting LoadCommunicationSetting(string communicationId)
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultCommSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultCommSettingTableName, " CommunicationId = " + communicationId, null);
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    return EntityAssembler.CreateCommSettingEntity(reader);
                }
                return null;
            }
        }

        #endregion

        #region 新增终端

        /// <summary>
        /// 新增终端信息
        /// </summary>
        /// <param name="entity">终端实体类</param>
        /// <returns>终端编号</returns>
         public bool InsertRTUSetting(RTUSetting entity)
        {
            string sqlCount = "SELECT COUNT(1) FROM RtuSetting";
            string sql = @"INSERT INTO " + SQLItems.DefaultRtuSettingTableName + "( " + SQLItems.DefaultRtuSettingFields + @") 
                    VALUES(@RTUID,@RTUName,@RTUSName,@ProductTypeId,@Manu,@InstallDate,@InstallLoca,@InstallAddr,@Analog,@Digit,@VoltAlert,@VoltClose,@CollCycle,@SendCycle,
                    @SaveCycle,@Note,@MobileNo,@CommunicationId,@CommunicationId1,@CommunicationId2,@IsOpen,@OpenTime1,@OpenTime2,@OpenTime3,@OpenTime4,@OpenTime5,
                    @ServiceCenter,@TableName,@RegionID,@PowerSupply,@Tag,@CommunicationTypeId,@Caliber,@CaliberType,@SendExceptionAlert,@SendVoltAlert,
                    @CollCycle2,@SendCycle2,@SaveCycle2,@CollCycle3,@SendCycle3,@SaveCycle3,@SafetyLevel,@AlertLevel,@SystemVoltage,@UltrasonicVoltage,
                    @LocationDevication,@WellDeep,@WellElevation,@PipeElevation,@PipeCaliber,@BackValue, @DeviceStatus, @CurrentCycle)";

             //[CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber]
            int count = Convert.ToInt32(this.AdoHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sqlCount));
            entity.TableName = RepositotyTableContentLimit.GetRepositotyTableName(count);

            entity.CurrentCycle = 0;
            entity.DeviceStatus = 1;

            SqlParameter[] para = this.CreateSqlParameters(entity);
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, para) > 0;

        }

        /// <summary>
        /// 批量新增终端信息
        /// </summary>
        /// <param name="entities">终端实体类组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertRTUSetting(IEnumerable<RTUSetting> entities)
        {
            bool result = true;

            DataTable dt = new DataTable();
            dt.CaseSensitive = true;
            dt.Columns.Add("RTUId", typeof(string));
            dt.Columns.Add("RTUName", typeof(string));
            dt.Columns.Add("RTUSName", typeof(string));
            dt.Columns.Add("ProductTypeId", typeof(string));
            dt.Columns.Add("Manu", typeof(string));
            dt.Columns.Add("InstallDate", typeof(DateTime));
            dt.Columns.Add("InstallLoca", typeof(string));
            dt.Columns.Add("InstallAddr", typeof(string));
            dt.Columns.Add("Analog", typeof(int));
            dt.Columns.Add("Digit", typeof(int));
            dt.Columns.Add("VoltAlert", typeof(decimal));
            dt.Columns.Add("VoltClose", typeof(decimal));
            dt.Columns.Add("CollCycle", typeof(int));
            dt.Columns.Add("SendCycle", typeof(int));
            dt.Columns.Add("SaveCycle", typeof(int));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("MobileNo", typeof(string));
            dt.Columns.Add("CommunicationId", typeof(string));
            dt.Columns.Add("CommunicationId1", typeof(string));
            dt.Columns.Add("CommunicationId2", typeof(string));
            dt.Columns.Add("IsOpen", typeof(bool));
            dt.Columns.Add("OpenTime1", typeof(string));
            dt.Columns.Add("OpenTime2", typeof(string));
            dt.Columns.Add("OpenTime3", typeof(string));
            dt.Columns.Add("OpenTime4", typeof(string));
            dt.Columns.Add("OpenTime5", typeof(string));
            dt.Columns.Add("ServiceCenter", typeof(string));
            dt.Columns.Add("TableName", typeof(string));
            dt.Columns.Add("RegionId", typeof(int));
            dt.Columns.Add("PowerSupply", typeof(string));
            dt.Columns.Add("Tag", typeof(byte));
            dt.Columns.Add("CommunicationTypeId", typeof(string));
            dt.Columns.Add("Caliber", typeof(string));
            dt.Columns.Add("CaliberType", typeof(int));
            dt.Columns.Add("SendExceptionAlert", typeof(bool));
            dt.Columns.Add("SendVoltAlert", typeof(bool));

            //[CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],
            //[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber]
            dt.Columns.Add("CollCycle2", typeof(int));
            dt.Columns.Add("SendCycle2", typeof(int));
            dt.Columns.Add("SaveCycle2", typeof(int));
            dt.Columns.Add("CollCycle3", typeof(int));
            dt.Columns.Add("SendCycle3", typeof(int));
            dt.Columns.Add("SaveCycle3", typeof(int));
            dt.Columns.Add("SafetyLevel", typeof(decimal));
            dt.Columns.Add("AlertLevel", typeof(decimal));
            dt.Columns.Add("SystemVoltage", typeof(decimal));
            dt.Columns.Add("UltrasonicVoltage", typeof(decimal));
            dt.Columns.Add("LocationDevication", typeof(decimal));
            dt.Columns.Add("WellDeep", typeof(decimal));
            dt.Columns.Add("WellElevation", typeof(decimal));
            dt.Columns.Add("PipeElevation", typeof(decimal));
            dt.Columns.Add("PipeCaliber", typeof(decimal));
            dt.Columns.Add("BackValue", typeof(decimal));
            dt.Columns.Add("DeviceStatus", typeof(int));
            dt.Columns.Add("CurrentCycle", typeof(int));

            foreach (RTUSetting ent in entities)
            {
                DataRow dr = dt.NewRow();
                dr["RTUId"] = ent.RTUId;
                dr["RTUName"] = ent.RTUName;
                dr["RTUSName"] = ent.RTUSName;
                dr["ProductTypeId"] = ent.ProductTypeId;
                dr["Manu"] = ent.Manu;
                dr["InstallDate"] = ent.InstallDate;
                dr["InstallLoca"] = ent.InstallLoca;
                dr["InstallAddr"] = ent.InstallAddr;
                dr["Analog"] = ent.Analog;
                dr["Digit"] = ent.Digit;
                dr["VoltAlert"] = ent.VoltAlert;
                dr["VoltClose"] = ent.VoltClose;
                dr["CollCycle"] = ent.CollCycle;
                dr["SendCycle"] = ent.SendCycle;
                dr["SaveCycle"] = ent.SaveCycle;
                dr["Note"] = ent.Note;
                dr["MobileNo"] = ent.MobileNo;
                dr["CommunicationId"] = ent.CommunicationId;
                dr["CommunicationId1"] = ent.CommunicationId1;
                dr["CommunicationId2"] = ent.CommunicationId2;
                dr["IsOpen"] = ent.IsOpen;
                dr["OpenTime1"] = ent.OpenTime1;
                dr["OpenTime2"] = ent.OpenTime2;
                dr["OpenTime3"] = ent.OpenTime3;
                dr["OpenTime4"] = ent.OpenTime4;
                dr["OpenTime5"] = ent.OpenTime5;
                dr["ServiceCenter"] = ent.ServiceCenter;
                dr["TableName"] = ent.TableName;
                dr["RegionId"] = ent.RegionId;
                dr["PowerSupply"] = ent.PowerSupply;
                dr["Tag"] = ent.Tag;
                dr["CommunicationTypeId"] = ent.CommunicationTypeId;
                dr["Caliber"] = ent.Caliber;
                dr["CaliberType"] = ent.CaliberType;
                dr["SendExceptionAlert"] = ent.SendExceptionAlert;
                dr["SendVoltAlert"] = ent.SendVoltAlert;

                dr["CollCycle2"] = ent.CollCycle2;
                dr["SendCycle2"] = ent.SendCycle2;
                dr["SaveCycle2"] = ent.SaveCycle2;
                dr["CollCycle3"] = ent.CollCycle3;
                dr["SendCycle3"] = ent.SendCycle3;
                dr["SaveCycle3"] = ent.SaveCycle3;
                dr["SafetyLevel"] = ent.SafetyLevel;
                dr["AlertLevel"] = ent.AlertLevel;
                dr["SystemVoltage"] = ent.SystemVoltage;
                dr["UltrasonicVoltage"] = ent.UltrasonicVoltage;
                dr["LocationDevication"] = ent.LocationDevication;
                dr["WellDeep"] = ent.WellDeep;
                dr["WellElevation"] = ent.WellElevation;
                dr["PipeElevation"] = ent.PipeElevation;
                dr["PipeCaliber"] = ent.PipeCaliber;
                dr["BackValue"] = ent.BackValue;
                dr["DeviceStatus"] = 1;
                dr["CurrentCycle"] = 0;

                dt.Rows.Add(dr);
            }

            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = SQLItems.DefaultRtuSettingTableName;
                    bulkCopy.ColumnMappings.Add("RTUId", "RTUId");
                    bulkCopy.ColumnMappings.Add("RTUName", "RTUName");
                    bulkCopy.ColumnMappings.Add("RTUSName", "RTUSName");
                    bulkCopy.ColumnMappings.Add("ProductTypeId", "ProductTypeId");
                    bulkCopy.ColumnMappings.Add("Manu", "Manu");
                    bulkCopy.ColumnMappings.Add("InstallDate", "InstallDate");
                    bulkCopy.ColumnMappings.Add("InstallLoca", "InstallLoca");
                    bulkCopy.ColumnMappings.Add("InstallAddr", "InstallAddr");
                    bulkCopy.ColumnMappings.Add("Analog", "Analog");
                    bulkCopy.ColumnMappings.Add("Digit", "Digit");
                    bulkCopy.ColumnMappings.Add("VoltAlert", "VoltAlert");
                    bulkCopy.ColumnMappings.Add("VoltClose", "VoltClose");
                    bulkCopy.ColumnMappings.Add("CollCycle", "CollCycle");
                    bulkCopy.ColumnMappings.Add("SendCycle", "SendCycle");
                    bulkCopy.ColumnMappings.Add("SaveCycle", "SaveCycle");
                    bulkCopy.ColumnMappings.Add("Note", "Note");
                    bulkCopy.ColumnMappings.Add("MobileNo", "MobileNo");
                    bulkCopy.ColumnMappings.Add("CommunicationId", "CommunicationId");
                    bulkCopy.ColumnMappings.Add("CommunicationId1", "CommunicationId1");
                    bulkCopy.ColumnMappings.Add("CommunicationId2", "CommunicationId2");
                    bulkCopy.ColumnMappings.Add("IsOpen", "IsOpen");
                    bulkCopy.ColumnMappings.Add("IsSendA", "IsSendA");
                    bulkCopy.ColumnMappings.Add("IsSendCA", "IsSendCA");
                    bulkCopy.ColumnMappings.Add("OpenTime1", "OpenTime1");
                    bulkCopy.ColumnMappings.Add("OpenTime2", "OpenTime2");
                    bulkCopy.ColumnMappings.Add("OpenTime3", "OpenTime3");
                    bulkCopy.ColumnMappings.Add("OpenTime4", "OpenTime4");
                    bulkCopy.ColumnMappings.Add("OpenTime5", "OpenTime5");
                    bulkCopy.ColumnMappings.Add("ServiceCenter", "ServiceCenter");
                    bulkCopy.ColumnMappings.Add("TableName", "TableName");
                    bulkCopy.ColumnMappings.Add("RegionId", "RegionId");
                    bulkCopy.ColumnMappings.Add("PowerSupply", "PowerSupply");
                    bulkCopy.ColumnMappings.Add("Tag", "Tag");
                    bulkCopy.ColumnMappings.Add("CommunicationTypeId", "CommunicationTypeId");
                    bulkCopy.ColumnMappings.Add("Caliber", "Caliber");
                    bulkCopy.ColumnMappings.Add("CaliberType", "CaliberType");
                    bulkCopy.ColumnMappings.Add("SendExceptionAlert", "SendExceptionAlert");
                    bulkCopy.ColumnMappings.Add("SendVoltAlert", "SendVoltAlert");

                    bulkCopy.ColumnMappings.Add("CollCycle2", "CollCycle2");
                    bulkCopy.ColumnMappings.Add("SendCycle2", "SendCycle2");
                    bulkCopy.ColumnMappings.Add("SaveCycle2", "SaveCycle2");
                    bulkCopy.ColumnMappings.Add("CollCycle3", "CollCycle3");
                    bulkCopy.ColumnMappings.Add("SendCycle3", "SendCycle3");
                    bulkCopy.ColumnMappings.Add("SaveCycle3", "SaveCycle3");

                    bulkCopy.ColumnMappings.Add("SafetyLevel", "SafetyLevel");
                    bulkCopy.ColumnMappings.Add("AlertLevel", "AlertLevel");
                    bulkCopy.ColumnMappings.Add("SystemVoltage", "SystemVoltage");
                    bulkCopy.ColumnMappings.Add("UltrasonicVoltage", "UltrasonicVoltage");
                    bulkCopy.ColumnMappings.Add("LocationDevication", "LocationDevication");
                    bulkCopy.ColumnMappings.Add("WellDeep", "WellDeep");
                    bulkCopy.ColumnMappings.Add("WellElevation", "WellElevation");
                    bulkCopy.ColumnMappings.Add("PipeElevation", "PipeElevation");
                    bulkCopy.ColumnMappings.Add("PipeCaliber", "PipeCaliber");
                    bulkCopy.ColumnMappings.Add("BackValue", "BackValue");
                    bulkCopy.ColumnMappings.Add("DeviceStatus", "DeviceStatus");
                    bulkCopy.ColumnMappings.Add("CurrentCycle", "CurrentCycle");

                    conn.Open();
                    bulkCopy.WriteToServer(dt);
                }
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        #endregion

        #region 获取最新终端编号

        /// <summary>
        /// 获取最新终端编号
        /// </summary>
        /// <returns>终端编号</returns>
        public string GetNewRtuId()
        {
            string newrtuid = string.Empty;
            try
            {
                #region 获取终端四位编码，序列码（编码规则：十位数字+26位小写字母+26位大写字母）
                int i = 1;
                while (string.IsNullOrEmpty(newrtuid) && i <= 3844)
                {//两位数字编码,最多轮训100次

                    //返回终端编码序列值
                    SqlParameter paranum = new SqlParameter("@NewRtuNum", DbType.Int32);
                    paranum.Direction = ParameterDirection.Output;
                    paranum.Value = 0;

                    this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.StoredProcedure, "sp_GetNewRtuId", paranum);
                    newrtuid = ConvertData.ConvertCode62((int)paranum.Value);

                    //注意修改：为dla做的特殊处理， 16进制，前两位一定是“00”
                    //if (newrtuid.Length >= 5)
                    //{//序列值超过四位，就取后两位编码
                    //    newrtuid = newrtuid.Substring(newrtuid.Length - 4, 4);
                    //}
                    //else
                    //{//不足四位补足四位
                    //    newrtuid = newrtuid.PadLeft(4, '0');
                    //}
                    if (newrtuid.Length >= 3)
                    {//序列值超过2位，就取后两位编码
                        newrtuid = newrtuid.Substring(newrtuid.Length - 2, 2);
                    }
                    newrtuid = newrtuid.PadLeft(4, '0');

                    //判断该编号是否已经使用，如果使用过就继续读取编号
                    if (this.Exists(newrtuid))
                    {
                        newrtuid = string.Empty;
                        i++;
                    }
                }
                #endregion

                return newrtuid;
            }
            catch (Exception e)
            {
                //ToDo 日志记录
                throw e;
            }
        }

        #endregion

        #region 更新终端

        /// <summary>
        /// 更新终端信息
        /// </summary>
        /// <param name="entity">终端实体类</param>
        /// <returns>bool型</returns>
        public bool UpdateRTUSetting(RTUSetting entity)
        {
            string sql = @"UPDATE " + SQLItems.DefaultRtuSettingTableName + " SET UpdateTime=GETDATE() "
                + (entity.CheckPropertyChanged("RTUName") ? ",RTUName=@RTUName" : "")
                + (entity.CheckPropertyChanged("RTUSName") ? ",RTUSName=@RTUSName" : "")
                + (entity.CheckPropertyChanged("ProductTypeId") ? ",ProductTypeId=@ProductTypeId" : "")
                + (entity.CheckPropertyChanged("Manu") ? ",Manu=@Manu" : "")
                + (entity.CheckPropertyChanged("InstallDate") ? ",InstallDate=@InstallDate" : "")
                + (entity.CheckPropertyChanged("InstallLoca") ? ",InstallLoca=@InstallLoca" : "")
                + (entity.CheckPropertyChanged("InstallAddr") ? ",InstallAddr=@InstallAddr" : "")
                + (entity.CheckPropertyChanged("Analog") ? ",Analog=@Analog" : "")
                + (entity.CheckPropertyChanged("Digit") ? ",Digit=@Digit" : "")
                + (entity.CheckPropertyChanged("VoltAlert") ? ",VoltAlert=@VoltAlert" : "")
                + (entity.CheckPropertyChanged("VoltClose") ? ",VoltClose=@VoltClose" : "")
                + (entity.CheckPropertyChanged("CollCycle") ? ",CollCycle=@CollCycle" : "")
                + (entity.CheckPropertyChanged("SendCycle") ? ",SendCycle=@SendCycle" : "")
                + (entity.CheckPropertyChanged("SaveCycle") ? ",SaveCycle=@SaveCycle" : "")
                + (entity.CheckPropertyChanged("Note") ? ",Note=@Note" : "")
                + (entity.CheckPropertyChanged("MobileNo") ? ",MobileNo=@MobileNo" : "")
                + (entity.CheckPropertyChanged("CommunicationId") ? ",CommunicationId=@CommunicationId" : "")
                + (entity.CheckPropertyChanged("CommunicationId1") ? ",CommunicationId1=@CommunicationId1" : "")
                + (entity.CheckPropertyChanged("CommunicationId2") ? ",CommunicationId2=@CommunicationId2" : "")
                + (entity.CheckPropertyChanged("IsOpen") ? ",IsOpen=@IsOpen" : "")
                + (entity.CheckPropertyChanged("OpenTime1") ? ",OpenTime1=@OpenTime1" : "")
                + (entity.CheckPropertyChanged("OpenTime2") ? ",OpenTime2=@OpenTime2" : "")
                + (entity.CheckPropertyChanged("OpenTime3") ? ",OpenTime3=@OpenTime3" : "")
                + (entity.CheckPropertyChanged("OpenTime4") ? ",OpenTime4=@OpenTime4" : "")
                + (entity.CheckPropertyChanged("OpenTime5") ? ",OpenTime5=@OpenTime5" : "")
                + (entity.CheckPropertyChanged("ServiceCenter") ? ",ServiceCenter=@ServiceCenter" : "")
                + (entity.CheckPropertyChanged("TableName") ? ",TableName=@TableName" : "")
                + (entity.CheckPropertyChanged("RegionId") ? ",RegionId=@RegionId" : "")
                + (entity.CheckPropertyChanged("PowerSupply") ? ",PowerSupply=@PowerSupply" : "")
                + (entity.CheckPropertyChanged("Tag") ? ",Tag=@Tag" : "")
                + (entity.CheckPropertyChanged("CommunicationTypeId") ? ",CommunicationTypeId=@CommunicationTypeId" : "")
                + (entity.CheckPropertyChanged("Caliber") ? ",Caliber=@Caliber" : "")
                + (entity.CheckPropertyChanged("CaliberType") ? ",CaliberType=@CaliberType" : "")
                + (entity.CheckPropertyChanged("SendExceptionAlert") ? ",SendExceptionAlert=@SendExceptionAlert" : "")
                + (entity.CheckPropertyChanged("SendVoltAlert") ? ",SendVoltAlert=@SendVoltAlert" : "")
                + (entity.CheckPropertyChanged("CollCycle2") ? ",CollCycle2=@CollCycle2" : "")
                + (entity.CheckPropertyChanged("SendCycle2") ? ",SendCycle2=@SendCycle2" : "")
                + (entity.CheckPropertyChanged("SaveCycle2") ? ",SaveCycle2=@SaveCycle2" : "")
                + (entity.CheckPropertyChanged("CollCycle3") ? ",CollCycle3=@CollCycle3" : "")
                + (entity.CheckPropertyChanged("SendCycle3") ? ",SendCycle3=@SendCycle3" : "")
                + (entity.CheckPropertyChanged("SaveCycle3") ? ",SaveCycle3=@SaveCycle3" : "")
                + (entity.CheckPropertyChanged("SafetyLevel") ? ",SafetyLevel=@SafetyLevel" : "")
                + (entity.CheckPropertyChanged("AlertLevel") ? ",AlertLevel=@AlertLevel" : "")
                + (entity.CheckPropertyChanged("SystemVoltage") ? ",SystemVoltage=@SystemVoltage" : "")
                + (entity.CheckPropertyChanged("UltrasonicVoltage") ? ",UltrasonicVoltage=@UltrasonicVoltage" : "")
                + (entity.CheckPropertyChanged("LocationDevication") ? ",LocationDevication=@LocationDevication" : "")
                + (entity.CheckPropertyChanged("WellDeep") ? ",WellDeep=@WellDeep" : "")
                + (entity.CheckPropertyChanged("WellElevation") ? ",WellElevation=@WellElevation" : "")
                + (entity.CheckPropertyChanged("PipeElevation") ? ",PipeElevation=@PipeElevation" : "")
                + (entity.CheckPropertyChanged("PipeCaliber") ? ",PipeCaliber=@PipeCaliber" : "")
                + (entity.CheckPropertyChanged("BackValue") ? ",BackValue=@BackValue" : "") 
                + (entity.CheckPropertyChanged("DeviceStatus") ? ",DeviceStatus=@DeviceStatus" : "") 
                + (entity.CheckPropertyChanged("CurrentCycle") ? ",CurrentCycle=@CurrentCycle" : "") 
                + " WHERE RTUId=@RtuId";
            SqlParameter[] para = this.CreateSqlParameters(entity);
            _logger.Debug("sql=" + sql);

            
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, para) > 0;
        }
       
        #endregion

        #region 删除终端信息
        /// <summary>
        /// 删除终端信息
        /// </summary>
        /// <param name="RtuId">终端编号</param>
        /// <returns>bool型</returns>
        public bool DeleteRTUSetting(string RtuId)
        {
            string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultRtuSettingTableName, " RtuId='" + RtuId + "'");
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
        }

        #endregion

        #region 获取终端

        /// <summary>
        /// 获取所有终端信息
        /// </summary>
        /// <returns>终端信息集合</returns>
        public DataTable LoadRTUSetting()
        {
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, 
                SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingFields + @", InsertTime, UpdateTime", 
                SQLItems.DefaultRtuSettingTableName, null, null));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 获取终端信息
        /// </summary>
        /// <returns>终端列表</returns>
        public List<RTUSetting> LoadRTUSettingList()
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingFields + @", InsertTime, UpdateTime", 
                SQLItems.DefaultRtuSettingTableName, null, null);

            List<RTUSetting> list = new List<RTUSetting>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(EntityAssembler.CreateRTUSettingEntity(reader));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取终端信息
        /// </summary>
        /// <param name="RtuId">终端主键</param>
        /// <returns>终端实体类</returns>
        public RTUSetting LoadRTUSetting(string rtuId)
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingFields + @", InsertTime, UpdateTime", 
                SQLItems.DefaultRtuSettingTableName, " RTUId = '" + rtuId + "'", null);
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    return EntityAssembler.CreateRTUSettingEntity(reader);
                }
                return null;
            }
        }
        /// <summary>
        /// 获取口径表
        /// </summary>
        /// <returns></returns>
        public DataTable LoadCaliberAndFlux()
        {
            string sql = "select * from CaliberAndFlux";

            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;

        }

        #endregion

        #region 新增检测量配置

        /// <summary>
        /// 新增检测量配置(编号MeasureId必须赋值)
        /// </summary>
        /// <param name="entity">检测量配置实体类</param>
        /// <returns>成功返回编号，失败返回null</returns>
        public string InsertMeasureSetting(MeasureSetting entity)
        {
            string sql = @"INSERT INTO " + SQLItems.DefaultMeasureSettingTableName + @"(Measureid,MeasureName, RTUId, SignalType, DataType, PortId, Unit, Range, Elevation, [Precision], DecimalDigits, Scale, Offset, Revise, 
                                                            UpperLimit, LowerLimit, Ratio, Note, Type, IsOpen, IsSet, SendOutData, SendChangeData, DataStatus, SwitchType, SendRedirectionAlert)
                        VALUES (@Measureid,@MeasureName,@RTUId,@SignalType,@Datatype,@PortId,@Unit,@Range,@Elevation,@Precision,@DecimalDigits,@Scale,@Offset,@Revise,
                                @UpperLimit,@LowerLimit,@Ratio,@Note,@Type,@IsOpen,@IsSet,@SendOutData,@SendChangeData,@DataStatus,@SwitchType,@SendRedirectionAlert)";
            //SqlParameter[] para = this.CreateSqlParameters(entity);
            SqlParameter[] para = CreateSqlParametersWithIdentity(entity);
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, para) > 0? entity.MeasureId.ToString() : null;
        }

        /// <summary>
        /// 批量新增检测量配置
        /// </summary>
        /// <param name="entities">检测量配置实体类组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertMeasureSetting(IEnumerable<MeasureSetting> entities)
        {
            bool result = true;

            DataTable dt = new DataTable();
            dt.CaseSensitive = true;
            dt.Columns.Add("MeasureId", typeof(int));
            dt.Columns.Add("MeasureName", typeof(string));
            dt.Columns.Add("RTUId", typeof(string));
            dt.Columns.Add("SignalType", typeof(string));
            dt.Columns.Add("DataType", typeof(string));
            dt.Columns.Add("PortId", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("Range", typeof(decimal));
            dt.Columns.Add("Elevation", typeof(decimal));
            dt.Columns.Add("Precision", typeof(decimal));
            dt.Columns.Add("DecimalDigits", typeof(decimal));
            dt.Columns.Add("Scale", typeof(decimal));
            dt.Columns.Add("Offset", typeof(decimal));
            dt.Columns.Add("Revise", typeof(decimal));
            dt.Columns.Add("UpperLimit", typeof(decimal));
            dt.Columns.Add("LowerLimit", typeof(decimal));
            dt.Columns.Add("Ratio", typeof(decimal));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("Type", typeof(int));
            dt.Columns.Add("IsOpen", typeof(bool));
            dt.Columns.Add("IsSet", typeof(bool));
            dt.Columns.Add("SendOutData", typeof(bool));
            dt.Columns.Add("SendChangeData", typeof(bool));
            dt.Columns.Add("DataStatus", typeof(bool));
            dt.Columns.Add("SwitchType", typeof(bool));
            dt.Columns.Add("SendRedirectionAlert", typeof(bool));

            foreach (MeasureSetting ent in entities)
            {
                DataRow dr = dt.NewRow();
                dr["MeasureId"] = ent.MeasureId;
                dr["MeasureName"] = ent.MeasureName;
                dr["RTUId"] = ent.RTUId;
                dr["SignalType"] = ent.SignalType;
                dr["DataType"] = ent.DataType;
                dr["PortId"] = ent.PortId;
                dr["Unit"] = ent.Unit;
                dr["Range"] = ent.Range;
                dr["Elevation"] = ent.Elevation;
                dr["Precision"] = ent.Precision;
                dr["DecimalDigits"] = ent.DecimalDigits;
                dr["Scale"] = ent.Scale;
                dr["Offset"] = ent.Offset;
                dr["Revise"] = ent.Revise;
                dr["UpperLimit"] = ent.UpperLimit;
                dr["LowerLimit"] = ent.LowerLimit;
                dr["Ratio"] = ent.Ratio;
                dr["Note"] = ent.Note;
                dr["Type"] = ent.Type;
                dr["IsOpen"] = ent.IsOpen;
                dr["IsSet"] = ent.IsSet;
                dr["SendOutData"] = ent.SendOutData;
                dr["SendChangeData"] = ent.SendChangeData;
                dr["DataStatus"] = ent.DataStatus;
                dr["SwitchType"] = ent.SwitchType;
                dr["SendRedirectionAlert"] = ent.SendRedirectionAlert;
                dt.Rows.Add(dr);
            }

            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = SQLItems.DefaultMeasureSettingTableName;
                    bulkCopy.ColumnMappings.Add("MeasureId", "MeasureId");
                    bulkCopy.ColumnMappings.Add("MeasureName", "MeasureName");
                    bulkCopy.ColumnMappings.Add("RTUId", "RTUId");
                    bulkCopy.ColumnMappings.Add("SignalType", "SignalType");
                    bulkCopy.ColumnMappings.Add("DataType", "DataType");
                    bulkCopy.ColumnMappings.Add("PortId", "PortId");
                    bulkCopy.ColumnMappings.Add("Unit", "Unit");
                    bulkCopy.ColumnMappings.Add("Range", "Range");
                    bulkCopy.ColumnMappings.Add("Elevation", "Elevation");
                    bulkCopy.ColumnMappings.Add("Precision", "Precision");
                    bulkCopy.ColumnMappings.Add("DecimalDigits", "DecimalDigits");
                    bulkCopy.ColumnMappings.Add("Scale", "Scale");
                    bulkCopy.ColumnMappings.Add("Offset", "Offset");
                    bulkCopy.ColumnMappings.Add("Revise", "Revise");
                    bulkCopy.ColumnMappings.Add("UpperLimit", "UpperLimit");
                    bulkCopy.ColumnMappings.Add("LowerLimit", "LowerLimit");
                    bulkCopy.ColumnMappings.Add("Ratio", "Ratio");
                    bulkCopy.ColumnMappings.Add("Note", "Note");
                    bulkCopy.ColumnMappings.Add("Type", "Type");
                    bulkCopy.ColumnMappings.Add("IsOpen", "IsOpen");
                    bulkCopy.ColumnMappings.Add("IsSet", "IsSet");
                    bulkCopy.ColumnMappings.Add("SendOutData", "SendOutData");
                    bulkCopy.ColumnMappings.Add("SendChangeData", "SendChangeData");
                    bulkCopy.ColumnMappings.Add("DataStatus", "DataStatus");
                    bulkCopy.ColumnMappings.Add("SwitchType", "SwitchType");
                    bulkCopy.ColumnMappings.Add("SendRedirectionAlert", "SendRedirectionAlert");

                    conn.Open();
                    bulkCopy.WriteToServer(dt);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #endregion

        #region 更新检测量配置

        /// <summary>
        /// 更新检测量配置
        /// </summary>
        /// <param name="entity">检测量配置实体类</param>
        /// <returns>bool型</returns>
        public bool UpdateMeasureSetting(MeasureSetting entity)
        {

            string sql = @"UPDATE " + SQLItems.DefaultMeasureSettingTableName + " SET UpdateTime=getdate() "
                + (entity.CheckPropertyChanged("MeasureName") ? ",MeasureName=@MeasureName" : "")
                + (entity.CheckPropertyChanged("RTUId") ? ",RTUId=@RTUId" : "")
                + (entity.CheckPropertyChanged("SignalType") ? ",SignalType=@SignalType" : "")
                + (entity.CheckPropertyChanged("DataType") ? ",DataType=@DataType" : "")
                + (entity.CheckPropertyChanged("PortId") ? ",PortID=@PortID" : "")
                + (entity.CheckPropertyChanged("Unit") ? ",Unit=@Unit" : "")
                + (entity.CheckPropertyChanged("Range") ? ",Range=@Range" : "")
                + (entity.CheckPropertyChanged("Elevation") ? ",Elevation=@Elevation" : "")
                + (entity.CheckPropertyChanged("Precision") ? ",[Precision]=@Precision" : "")
                + (entity.CheckPropertyChanged("DecimalDigits") ? ",DecimalDigits=@DecimalDigits" : "")
                + (entity.CheckPropertyChanged("Scale") ? ",Scale=@Scale" : "")
                + (entity.CheckPropertyChanged("Offset") ? ",Offset=@Offset" : "")
                + (entity.CheckPropertyChanged("Revise") ? ",Revise=@Revise" : "")
                + (entity.CheckPropertyChanged("UpperLimit") ? ",UpperLimit=@UpperLimit" : "")
                + (entity.CheckPropertyChanged("LowerLimit") ? ",LowerLimit=@LowerLimit" : "")
                + (entity.CheckPropertyChanged("Ratio") ? ",Ratio=@Ratio" : "")
                + (entity.CheckPropertyChanged("Note") ? ",Note=@Note" : "")
                + (entity.CheckPropertyChanged("Type") ? ",Type=@Type" : "")
                + (entity.CheckPropertyChanged("IsOpen") ? ",IsOpen=@IsOpen" : "")
                + (entity.CheckPropertyChanged("IsSet") ? ",IsSet=@IsSet" : "")
                + (entity.CheckPropertyChanged("SendOutData") ? ",SendOutData=@SendOutData" : "")
                + (entity.CheckPropertyChanged("SendChangeData") ? ",SendChangeData=@SendChangeData" : "")
                + (entity.CheckPropertyChanged("DataStatus") ? ",DataStatus=@DataStatus" : "")
                + (entity.CheckPropertyChanged("SwitchType") ? ",SwitchType=@SwitchType" : "")
                + (entity.CheckPropertyChanged("SendRedirectionAlert") ? ",SendRedirectionAlert=@SendRedirectionAlert" : "")
                + " WHERE MeasureId=@MeasureId";
             
            SqlParameter[] para = new SqlParameter[26];
            this.CreateSqlParameters(entity).CopyTo(para, 0);
            para[25] = new SqlParameter("@MeasureId", entity.MeasureId);

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, para) > 0;
        }

        #endregion

        #region 删除检测量信息
        /// <summary>
        /// 删除检测量信息
        /// </summary>
        /// <param name="entity">检测量信息</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureSetting(MeasureSetting entity)
        {
            return this.DeleteMeasureSetting(" MeasureId = " + entity.MeasureId);
        }

        /// <summary>
        /// 批量删除检测量
        /// </summary>
        /// <param name="entities">检测量实体列表</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureSetting(IEnumerable<MeasureSetting> entities)
        {
            using (SqlConnection conn = this.AdoHelper.GetConnection(this.ConnectionString) as SqlConnection)
            {
               

                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (MeasureSetting entity in entities)
                    {
                        string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultMeasureSettingTableName, " MeasureId = " + entity.MeasureId);
                        this.AdoHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    trans.Dispose();
                }
            }            
            
        }

        /// <summary>
        /// 删除检测量信息
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureSetting(string condition)
        {
            string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultMeasureSettingTableName, condition);
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
        }
        #endregion

        #region 获取检测量配置

        /// <summary>
        /// 获取所有检测量配置
        /// </summary>
        /// <returns>检测量配置集合</returns>
        public DataTable LoadMeasureSetting()
        {
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, null, null));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 获取检测量配置
        /// </summary>
        /// <returns>检测量配置</returns>
        public List<MeasureSetting> LoadMeasureSettingList()
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, null, null);

            List<MeasureSetting> list = new List<MeasureSetting>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(EntityAssembler.CreateMeasureSettingEntity(reader));
                }
            }
            return list;
        }
        
        /// <summary>
        /// 获取检测量配置
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>检测量配置集合</returns>
        public List<MeasureSetting> LoadMeasSettingByRtuId(string rtuId)
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, " RtuId ='" + rtuId + "'", null);

            List<MeasureSetting> list = new List<MeasureSetting>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(EntityAssembler.CreateMeasureSettingEntity(reader));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取检测量配置
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>检测量配置集合</returns>
        public DataTable LoadMeasureSettingByRtuId(string rtuId)
        {
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, "RtuId='" + rtuId + "'", null));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 根据编号获取检测量配置
        /// </summary>
        /// <param name="measureId">检测量编号</param>
        /// <returns>检测量配置实体类</returns>
        public MeasureSetting LoadMeasureSetting(string measureId)
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, " MeasureId = '" + measureId + "'", null);
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    return EntityAssembler.CreateMeasureSettingEntity(reader);
                }
                return null;
            }
        }

        #endregion

        #region 获取检测量模板

        /// <summary>
        /// 获取检测量模板
        /// </summary>
        /// <param name="productTypeId">产品编号</param>
        /// <returns>检测量模板集合</returns>
        public List<MeasureTemplet> LoadMeasureTempletByProductTypeId(string productTypeId)
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefalutMeasureTempletFields, SQLItems.DefaultMeasureTempletTableName, " ProductTypeId = '" + productTypeId + "'", null);
            List<MeasureTemplet> list = new List<MeasureTemplet>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(EntityAssembler.CreateMeasureTempletEntity(reader));
                }
            }
            return list;
        }

        #endregion

        #region 获取终端模板

        /// <summary>
        /// 获取终端模板
        /// </summary>
        /// <param name="productTypeId">产品类型编号</param>
        /// <returns>终端模板</returns>
        public RTUTemplet LoadRtuTemplet(string productTypeId)
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultRtuTempletFields, SQLItems.DefaultRtuTempletTableName, " ProductTypeId = '" + productTypeId + "'", null);
            RTUTemplet templet = new RTUTemplet();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    templet = EntityAssembler.CreateRtuTempletEntity(reader);
                }
            }
            return templet;
        }

        #endregion



        /// <summary>
        /// 修改终端模板
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateRtuTemplet(RTUTemplet entity)
        {

            //d

            string sql = @"UPDATE " + SQLItems.DefaultRtuTempletTableName + " SET "
              + (entity.CheckPropertyChanged("Manu") ? "Manu='{0}'" : "")
              + (entity.CheckPropertyChanged("Analog") ? ",Analog={1}" : "")
              + (entity.CheckPropertyChanged("Digit") ? ",Digit={2}" : "")
              + (entity.CheckPropertyChanged("VoltAlert") ? ",VoltAlert={3}" : "")
              + (entity.CheckPropertyChanged("VoltClose") ? ",VoltClose='{4}'" : "")
              + (entity.CheckPropertyChanged("CollCycle") ? ",CollCycle='{5}'" : "")
              + (entity.CheckPropertyChanged("SendCycle") ? ",SendCycle={6}" : "")
              + (entity.CheckPropertyChanged("SaveCycle") ? ",SaveCycle={7}" : "")
              + (entity.CheckPropertyChanged("Note") ? ",Note='{8}'" : "")
              + (entity.CheckPropertyChanged("ServiceCenter") ? ",ServiceCenter='{9}'" : "")
                // + (entity.CheckPropertyChanged("CommunicationTypeId") ? ",CommunicationTypeId='{10}'" : "")
              + (entity.CheckPropertyChanged("Tag") ? ",Tag={10}" : "")
                // + (entity.CheckPropertyChanged("Caliber") ? ",Caliber='{11}'" : "")
              + (entity.CheckPropertyChanged("CaliberType") ? ",CaliberType={11}" : "")
                //[CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber]
               + (entity.CheckPropertyChanged("CollCycle2") ? ",CollCycle2={12}" : "")
               + (entity.CheckPropertyChanged("SendCycle2") ? ",SendCycle2={13}" : "")
               + (entity.CheckPropertyChanged("SaveCycle2") ? ",SaveCycle2={14}" : "")
               + (entity.CheckPropertyChanged("CollCycle3") ? ",CollCycle3={15}" : "")
               + (entity.CheckPropertyChanged("SendCycle3") ? ",SendCycle3={16}" : "")
               + (entity.CheckPropertyChanged("SaveCycle3") ? ",SaveCycle3={17}" : "")
               + (entity.CheckPropertyChanged("SafetyLevel") ? ",SafetyLevel={18}" : "")
               + (entity.CheckPropertyChanged("AlertLevel") ? ",AlertLevel={19}" : "")
               + (entity.CheckPropertyChanged("SystemVoltage") ? ",SystemVoltage={20}" : "")
               + (entity.CheckPropertyChanged("UltrasonicVoltage") ? ",UltrasonicVoltage={21}" : "")
               + (entity.CheckPropertyChanged("LocationDevication") ? ",LocationDevication={22}" : "")
               + (entity.CheckPropertyChanged("WellDeep") ? ",WellDeep={23}" : "")
               + (entity.CheckPropertyChanged("WellElevation") ? ",WellElevation={24}" : "")
               + (entity.CheckPropertyChanged("PipeElevation") ? ",PipeElevation={25}" : "")
               + (entity.CheckPropertyChanged("PipeCaliber") ? ",PipeCaliber={26}" : "")

              + " WHERE [ProductTypeId]='{27}'";


            sql = string.Format(sql, entity.Manu, entity.Analog, entity.Digit, entity.VoltAlert,
                entity.VoltClose, entity.CollCycle, entity.SendCycle, entity.SaveCycle, entity.Note, entity.ServiceCenter,
                Convert.ToInt32(entity.Tag), entity.CaliberType, entity.CollCycle2, entity.SendCycle2, entity.SaveCycle2, entity.CollCycle3, entity.SendCycle3, entity.SaveCycle3,
               entity.SafetyLevel, entity.AlertLevel, entity.SystemVoltage, entity.UltrasonicVoltage, entity.LocationDevication, entity.WellDeep, entity.WellElevation, entity.PipeElevation, entity.PipeCaliber, entity.ProductTypeId);


            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;


        }

        #region update检测量模板
        /// <summary>
        /// 更新单条检测量模板信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateMeasureTemplet(MeasureTemplet entity)
        {


            string sql = @"UPDATE " + SQLItems.DefaultMeasureTempletTableName + " SET "
               + (entity.CheckPropertyChanged("MTempletName") ? "MTempletName='{0}'" : "")
               + (entity.CheckPropertyChanged("ProductTypeId") ? ",ProductTypeId='{1}'" : "")
               + (entity.CheckPropertyChanged("Manu") ? ",Manu='{2}'" : "")
               + (entity.CheckPropertyChanged("SignalType") ? ",SignalType='{3}'" : "")
               + (entity.CheckPropertyChanged("Unit") ? ",Unit='{4}'" : "")
               + (entity.CheckPropertyChanged("Range") ? ",[Range]={5}" : "")
               + (entity.CheckPropertyChanged("Precision") ? ",[Precision]={6}" : "")
               + (entity.CheckPropertyChanged("DecimalDigits") ? ",DecimalDigits={7}" : "")
               + (entity.CheckPropertyChanged("Offset") ? ",Offset={8}" : "")
               + (entity.CheckPropertyChanged("UpperLimit") ? ",UpperLimit={9}" : "")
               + (entity.CheckPropertyChanged("LowerLimit") ? ",LowerLimit={10}" : "")
               + (entity.CheckPropertyChanged("Ratio") ? ",Ratio={11}" : "")
               + (entity.CheckPropertyChanged("Note") ? ",Note='{12}'" : "")
               + (entity.CheckPropertyChanged("DataType") ? ",DataType='{13}'" : "")
                + (entity.CheckPropertyChanged("PortId") ? ",PortId='{14}'" : "")
                 + (entity.CheckPropertyChanged("IsSet") ? ",IsSet={15}" : "")
                 + (entity.CheckPropertyChanged("Scale") ? ",Scale={16}" : "")
               + " WHERE [ProductTypeId]='{1}' and DataType='{13}'";



            sql = string.Format(sql, entity.MTempletName, entity.ProductTypeId, entity.Manu, entity.SignalType,
                entity.Unit, entity.Range, entity.Precision, entity.DecimalDigits, entity.Offset, entity.UpperLimit,
                entity.LowerLimit, entity.Ratio, entity.Note, entity.DataType, entity.PortId, Convert.ToInt32(entity.IsSet), entity.Scale, entity.MTempletId);

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;


        }
        #endregion

        #region 删除检测量模板

        /// <summary>
        /// 删除单条检测量模板信息
        /// </summary>
        /// <param name="entity">检测量模板信息</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureTemplet(MeasureTemplet entity)
        {
            return this.DeleteMeasureTemplet(" ProductTypeId = '" + entity.ProductTypeId + "' and DataType='" + entity.DataType + "'");
        }

        /// <summary>
        /// 删除检测量模板信息
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureTemplet(string condition)
        {
            string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultMeasureTempletTableName, condition);
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
        }
        #endregion

        /// <summary>
        /// 新增检测量模板
        /// </summary>
        /// <param name="entity">检测量模板类</param>
        /// <returns>bool型</returns>
        public bool InsertMeasureTemplet(MeasureTemplet entity)
        {

            string sql = @"INSERT INTO {17} (MTempletName, ProductTypeId, Manu, SignalType, Unit, [Range], [Precision], DecimalDigits, Scale, Offset, UpperLimit, LowerLimit, Ratio, 
                                                            Note, DataType, PortId, IsSet) 
                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},'{13}','{14}','{15}','{16}')";

            sql = string.Format(sql, entity.MTempletName, entity.ProductTypeId, entity.Manu, entity.SignalType, entity.Unit, entity.Range, entity.Precision, entity.DecimalDigits, entity.Scale, entity.Offset,
                                      entity.UpperLimit, entity.LowerLimit, entity.Ratio, entity.Note, entity.DataType, entity.PortId, Convert.ToInt32(entity.IsSet), SQLItems.DefaultMeasureTempletTableName);

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
        }

        #region 获取所有模板
        /// <summary>
        /// 获取所有模板（Rtu和Measure）
        /// </summary>
        /// <returns>Rtu和Measure关联模板</returns>
        public DataTable LoadAllTemplet()
        {
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text,
                SQLItems.CreateSelectSql(SQLItems.AllTempletFields,SQLItems.AllTempletTables,null,null));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        #endregion

        #region  其余配置

        /// <summary>
        /// 获取产品类型编号集合
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable LoadProductTypeId()
        {
            string sql = SQLItems.CreateSelectSql("Distinct ProductTypeId", SQLItems.DefaultRtuTempletTableName, null, "ProductTypeId");
            DataSet ds = this.AdoHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        ///  获取产品类型编号集合
        /// </summary>
        /// <param name="communicationTypeId">通讯协议类型</param>
        /// <returns>产品类型编号</returns>
        public string LoadProductTypeId(string communicationTypeId)
        {
            string sql = SQLItems.CreateSelectSql("ProductTypeId", SQLItems.DefaultRtuTempletTableName, " CommunicationTypeId='" + communicationTypeId + "'", "ProductTypeId");
            return this.AdoHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql) as string;

        }

        /// <summary>
        /// 检索编号是否存在
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>bool型</returns>
        public bool Exists(string rtuId)
        {
            string sql = SQLItems.CreateSelectSql("COUNT(1)", SQLItems.DefaultRtuSettingTableName, " RtuId = '" + rtuId + "'", null);
            object o = this.AdoHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql);
            if (o != null && Convert.ToInt32(o) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 获取通讯协议类型
        /// <summary>
        /// 获取通讯协议类型
        /// </summary>
        /// <returns>通讯协议类型</returns>
        public List<CommunicationType> LoadCommunicationType()
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultCommunicationTypeFields, SQLItems.DefaultCommunicationTypeTableName, null, null);
            List<CommunicationType> list = new List<CommunicationType>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(EntityAssembler.CreateCommunicationTypeEntity(reader));
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 获取数据类型配置
        /// </summary>
        /// <returns>数据类型配置</returns>
        public List<DataTypeConfig> LoadDataTypeConfig()
        {
            string sql = SQLItems.CreateSelectSql(SQLItems.DefaultDataTypeFields, SQLItems.DefaultDataTypeTableName, null, null);
            List<DataTypeConfig> list = new List<DataTypeConfig>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(EntityAssembler.CreateDataTypeEntity(reader));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取端口号
        /// </summary>
        /// <param name="productTypeId">产品型号</param>
        /// <returns>端口号</returns>
        public List<string> LoadPortIdByProductType(string productTypeId)
        {
            string sql = SQLItems.CreateSelectSql("PortId", "ProductPortLink", "ProductTypeId='" + productTypeId + "'", null);
            List<string> list = new List<string>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(reader["PortId"] as string);
                }
            }
            return list;
        }
        #endregion

        #region 新增操作痕迹
        /// <summary>
        /// 新增操作痕迹
        /// </summary>
        /// <param name="entity">操作痕迹实体类</param>
        /// <returns>bool型</returns>
        public bool InsertConfigTraceLog(ConfigTrack entity)
        {
            string sql = @"INSERT INTO " + SQLItems.DefaultConfigTrackTableName + "(" + SQLItems.DefaultConfigTrackFields + @")
                        VALUES ( @OPTime, @OPType, @OPCode, @OPKeyID, @OPRemark)";
            SqlParameter[] para = this.CreateSqlParameters(entity);

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, para) > 0;
        }
        #endregion

        #region 删除操作痕迹
        /// <summary>
        /// 删除操作痕迹信息
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>bool型</returns>
        public bool DeleteConfigTraceLog(string condition)
        {
            string sql = SQLItems.CreateDeleteSql(SQLItems.DefaultConfigTrackTableName, condition);
            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql) > 0;
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
            string sql = @"select count(1) from userinfo where account=@Account and pwd=@PWD";
            SqlParameter[] para = this.CreateSqlParameters(userInfo);
            object o = this.AdoHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, para);

            if (o != null && Convert.ToInt32(o) > 0)
            {
                return true;
            }
            else return false;
        }
        #endregion

        #endregion

         #region 液位仪对应关系配置
        public Dictionary<string, int> LoadRtuObjectRelation()
        {
            string sql = SQLItems.CreateSelectSql("RtuId,ObjectId", "RtuObjectLink", string.Empty, string.Empty);
            Dictionary<string, int> dic = new Dictionary<string, int>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    dic.Add(reader["RtuId"] as string, Convert.ToInt32(reader["ObjectId"] as string));
                }
            }
            return dic;
        }

        public Dictionary<int, int> LoadMeasureTypeRelation()
        {
            string sql = SQLItems.CreateSelectSql("a.MeasureId,b.TypeId", "MeasureSetting a,PortTypeIdLink b", "a.PortId = b.PortId", string.Empty);
            Dictionary<int, int> dic = new Dictionary<int, int>();
            using (IDataReader reader = this.AdoHelper.ExecuteReader(this.ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    dic.Add(Convert.ToInt32(reader["MeasureId"] as string), Convert.ToInt32(reader["TypeId"] as string));
                }
            }
            return dic;
        }
        #endregion

        #region special methods

        /// <summary>
        /// 清理表
        /// </summary>
        /// <returns>bool型</returns>
        public void TruncateTable()
        {
            this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, SQLItems.TruncateTableSql);
        }

        /// <summary>
        /// 同步通道信息
        /// </summary>
        /// <param name="entity">通道实体类</param>
        /// <returns>通道编号</returns>
        public bool SyncCommunicationSetting(List<CommunicationSetting> list)
        {
            if (list.Count == 0)
                return true;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("SET IDENTITY_INSERT CommunicationSetting ON");
            foreach (CommunicationSetting entity in list)
            {
                builder.AppendLine(this.CreateCommSettingInsertSql(entity));
            }
            builder.AppendLine("SET IDENTITY_INSERT CommunicationSetting OFF");

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, builder.ToString()) > 0;

        }

        /// <summary>
        /// 同步终端信息
        /// </summary>
        /// <param name="list">终端实体列表</param>
        /// <returns>Bool型</returns>
        public bool SyncRTUSetting(List<RTUSetting> list)
        {
            if (list.Count == 0)
                return true;
            try
            {
                foreach (RTUSetting entity in list)
                {
                    this.InsertRTUSetting(entity);
                }
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 同步检测量配置
        /// </summary>
        /// <param name="list">检测量配置实体列表</param>
        /// <returns>bool型</returns>
        public bool SyncMeasureSetting(List<MeasureSetting> list)
        {
            if (list.Count == 0)
                return true;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("SET IDENTITY_INSERT MeasureSetting ON");
            foreach (MeasureSetting entity in list)
            {
                builder.AppendLine(this.CreateMeasureSettingInsertSql(entity));
            }
            builder.AppendLine("SET IDENTITY_INSERT MeasureSetting OFF");

            return this.AdoHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, builder.ToString()) > 0;

        }

        #endregion

        #region Create SqlParameters

        private SqlParameter[] CreateSqlParameters(CommunicationSetting entity)
        {
            SqlParameter[] para = new SqlParameter[11] { 
            new SqlParameter("CommunicationName",entity.CommunicationName ),
            new SqlParameter("MobileNo",entity.MobileNo ),
            new SqlParameter("Note",entity.Note ),
            new SqlParameter("APN",entity.APN ),
            new SqlParameter("Dns1",entity.Dns1 ),
            new SqlParameter("Dns2",entity.Dns2 ),
            new SqlParameter("IP",entity.IP ),
            new SqlParameter("ExTcp",entity.ExTcp ),
            new SqlParameter("InTcp",entity.InTcp ),
            new SqlParameter("CommunicationTypeId",entity.CommunicationTypeId ),
            new SqlParameter("Tag",entity.Tag )
            };
            return para;
        }

        private SqlParameter[] CreateSqlParametersWithIdentity(CommunicationSetting entity)
        {
            SqlParameter[] para = new SqlParameter[12];
            para[0] = new SqlParameter("@CommunicationId", entity.CommunicationId);
            this.CreateSqlParameters(entity).CopyTo(para, 1);
            return para;
        }

        private SqlParameter[] CreateSqlParameters(RTUSetting entity)
        {
            //@CollCycle2,@SendCycle2,@SaveCycle2,@CollCycle3,@SendCycle3,@SaveCycle3,@SafetyLevel,@AlertLevel,@SystemVoltage,@UltrasonicVoltage,@LocationDevication,@WellDeep,@WellElevation,@PipeElevation,@PipeCaliber
            SqlParameter[] para = new SqlParameter[54] { 
            new SqlParameter("@RTUId" , entity.RTUId),
            new SqlParameter("@RTUName" , entity.RTUName),
            new SqlParameter("@RTUSName" , entity.RTUSName),
            new SqlParameter("@ProductTypeId" , entity.ProductTypeId),
            new SqlParameter("@Manu" , entity.Manu),
            new SqlParameter("@InstallDate" , entity.InstallDate==DateTime.MinValue? DateTime.Now:entity.InstallDate),
            new SqlParameter("@InstallLoca" , entity.InstallLoca),
            new SqlParameter("@InstallAddr" , entity.InstallAddr),
            new SqlParameter("@Analog" , entity.Analog),
            new SqlParameter("@Digit" , entity.Digit),
            new SqlParameter("@VoltAlert" , entity.VoltAlert),
            new SqlParameter("@VoltClose" , entity.VoltClose),
            new SqlParameter("@CollCycle" , entity.CollCycle),
            new SqlParameter("@SendCycle" , entity.SendCycle),
            new SqlParameter("@SaveCycle" , entity.SaveCycle),
            new SqlParameter("@Note" , entity.Note),
            new SqlParameter("@MobileNo" , entity.MobileNo),
            new SqlParameter("@CommunicationId" , entity.CommunicationId),
            new SqlParameter("@CommunicationId1" , entity.CommunicationId1),
            new SqlParameter("@CommunicationId2" , entity.CommunicationId2),
            new SqlParameter("@IsOpen" , entity.IsOpen),
            new SqlParameter("@Opentime1" , entity.OpenTime1),
            new SqlParameter("@Opentime2" , entity.OpenTime2),
            new SqlParameter("@Opentime3" , entity.OpenTime3),
            new SqlParameter("@Opentime4" , entity.OpenTime4),
            new SqlParameter("@Opentime5" , entity.OpenTime5),
            new SqlParameter("@ServiceCenter" , entity.ServiceCenter),
            new SqlParameter("@TableName" , entity.TableName),
            new SqlParameter("@RegionId" , entity.RegionId),
            new SqlParameter("@PowerSupply" , entity.PowerSupply),
            new SqlParameter("@Tag" , entity.Tag),
            new SqlParameter("@CommunicationTypeId" , entity.CommunicationTypeId),
            new SqlParameter("@Caliber" , entity.Caliber),
            new SqlParameter("@CaliberType" , entity.CaliberType),
            new SqlParameter("@SendExceptionAlert", entity.SendExceptionAlert),
            new SqlParameter("@SendVoltAlert", entity.SendVoltAlert),
            new SqlParameter("@CollCycle2", entity.CollCycle2),
            new SqlParameter("@SendCycle2", entity.SendCycle2),
            new SqlParameter("@SaveCycle2", entity.SaveCycle2),
            new SqlParameter("@CollCycle3", entity.CollCycle3),
            new SqlParameter("@SendCycle3", entity.SendCycle3),
            new SqlParameter("@SaveCycle3", entity.SaveCycle3),
            new SqlParameter("@SafetyLevel", entity.SafetyLevel),
            new SqlParameter("@AlertLevel", entity.AlertLevel),
            new SqlParameter("@SystemVoltage", entity.SystemVoltage),
            new SqlParameter("@UltrasonicVoltage", entity.UltrasonicVoltage),
            new SqlParameter("@LocationDevication", entity.LocationDevication),
            new SqlParameter("@WellDeep", entity.WellDeep),
            new SqlParameter("@WellElevation", entity.WellElevation),
            new SqlParameter("@PipeElevation", entity.PipeElevation),
            new SqlParameter("@PipeCaliber", entity.PipeCaliber),
            new SqlParameter("@BackValue", entity.BackValue),
            new SqlParameter("@DeviceStatus", entity.DeviceStatus),
            new SqlParameter("@CurrentCycle", entity.CurrentCycle)
            };
            return para;
        }

        private SqlParameter[] CreateSqlParameters(MeasureSetting entity)
        {
            SqlParameter[] para = new SqlParameter[25] {  
            new SqlParameter("@MeasureName",entity.MeasureName),
            new SqlParameter("@RTUId",entity.RTUId),
            new SqlParameter("@SignalType",entity.SignalType),
            new SqlParameter("@DataType",entity.DataType),
            new SqlParameter("@PortId",entity.PortId),
            new SqlParameter("@Unit",entity.Unit),
            new SqlParameter("@Range",entity.Range),
            new SqlParameter("@Elevation",entity.Elevation),
            new SqlParameter("@Precision",entity.Precision),
            new SqlParameter("@DecimalDigits",entity.DecimalDigits),
            new SqlParameter("@Scale",entity.Scale),
            new SqlParameter("@Offset",entity.Offset),
            new SqlParameter("@Revise",entity.Revise),
            new SqlParameter("@UpperLimit",entity.UpperLimit),
            new SqlParameter("@LowerLimit",entity.LowerLimit),
            new SqlParameter("@Ratio",entity.Ratio),
            new SqlParameter("@Note",entity.Note),
            new SqlParameter("@Type",entity.Type),
            new SqlParameter("@IsOpen",entity.IsOpen),
            new SqlParameter("@IsSet",entity.IsSet),
            new SqlParameter("@SendOutData",entity.SendOutData),
            new SqlParameter("@SendChangeData",entity.SendChangeData),
            new SqlParameter("@DataStatus",entity.DataStatus),
            new SqlParameter("@SwitchType",entity.SwitchType),
            new SqlParameter("@SendRedirectionAlert", entity.SendRedirectionAlert)
            };
            return para;
        }

        private SqlParameter[] CreateSqlParametersWithIdentity(MeasureSetting entity)
        {
            SqlParameter[] para = new SqlParameter[26];
            para[0] = new SqlParameter("@MeasureId", entity.MeasureId);
            this.CreateSqlParameters(entity).CopyTo(para, 1);
            return para;
        }

        private SqlParameter[] CreateSqlParameters(ConfigTrack entity)
        {
            SqlParameter[] para = new SqlParameter[5] {                
            new SqlParameter("@OPTime",entity.OpTime== DateTime.MinValue ? DateTime.Now : entity.OpTime),
            new SqlParameter("@OPType",entity.OpType),
            new SqlParameter("@OPCode",entity.OpCode),
            new SqlParameter("@OPKeyID",entity.OpKeyId),
            new SqlParameter("@OPRemark",entity.OpRemark)
            };
            return para;
        }

        private SqlParameter[] CreateSqlParameters(UserInfo entity)
        {
            SqlParameter[] para = new SqlParameter[2] {   
            new SqlParameter("@Account",entity.Account),
            new SqlParameter("@PWD",entity.PWD)
            };
            return para;
        }
        /// <summary>
        /// 构建通道同步的执行语句
        /// </summary>
        /// <param name="entity">通道实体</param>
        /// <returns>执行语句</returns>
        private string CreateCommSettingInsertSql(CommunicationSetting entity)
        {
            string sql = @"INSERT INTO {12} (CommunicationId,CommunicationName, MobileNo, Note, APN, Dns1, Dns2, IP, ExTcp, InTcp, CommunicationTypeId, Tag) 
                        VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11})";
            return string.Format(sql, entity.CommunicationId, entity.CommunicationName, entity.MobileNo, entity.Note, entity.APN, entity.Dns1, entity.Dns2, entity.IP, entity.ExTcp, entity.InTcp, entity.CommunicationTypeId, entity.Tag,
                SQLItems.DefaultCommSettingTableName);
        }

        /// <summary>
        /// 构建检测量配置同步的执行语句
        /// </summary>
        /// <param name="entity">检测量配置实体</param>
        /// <returns>执行语句</returns>
        private string CreateMeasureSettingInsertSql(MeasureSetting entity)
        {
            string sql = @"INSERT INTO {26} (MeasureId,MeasureName, RTUId, SignalType, DataType, PortId, Unit, Range, Elevation, [Precision], DecimalDigits, Scale, Offset, Revise, 
                                                            UpperLimit, LowerLimit, Ratio, Note, Type, IsOpen, IsSet, SendOutData, SendChangeData, DataStatus, SwitchType, SendRedirectionAlert) 
                        VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},'{17}',{18},{19},{20},{21},{22},
                        {23},{24},{25})";

            return string.Format(sql, entity.MeasureId, entity.MeasureName, entity.RTUId, entity.SignalType, entity.DataType, entity.PortId, entity.Unit, entity.Range, entity.Elevation, entity.Precision, entity.DecimalDigits, entity.Scale, entity.Offset, entity.Revise,
                                      entity.UpperLimit, entity.LowerLimit, entity.Ratio, entity.Note, entity.Type, Convert.ToInt32(entity.IsOpen), Convert.ToInt32(entity.IsSet), Convert.ToInt32(entity.SendOutData), Convert.ToInt32(entity.SendChangeData),
                                      Convert.ToInt32(entity.DataStatus), Convert.ToInt32(entity.SwitchType), Convert.ToInt32(entity.SendRedirectionAlert),
                                      SQLItems.DefaultMeasureSettingTableName);
        }

        #endregion

        public bool DeleteRTULog(string rtuid)
        {
            throw new NotImplementedException();
        }

        public bool InsertRTULog(RTULog rtulogs)
        {
            throw new NotImplementedException();
        }

        public List<RTULog> LoadAllRTULog()
        { 
            throw new NotImplementedException();
        }
        public void BulkInsertRTUlog(IEnumerable<RTULog> rtulogs)
        {
            throw new NotImplementedException();
        }
    }
}
