using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataAccess.Interfaces;
using DataEntity;
using System.Data.SQLite;
using DataAccess;
using FunctionLib;

namespace DataAccess.Sqlite
{
    public class SqliteSettingRepository : SqliteRepositoryBase, ISettingRepository
    {
        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqliteSettingRepository(string connectionString) : base(connectionString) { }

        #region ICommunicationSettingRepository Members

        #region NewDLA
        /// <summary>
        /// 插入rtusetting_device
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertRtuSetting_Device(Rtusetting_Device entity)
        {
            bool result = false;

            string sql = "select * from rtusetting_device where rtuid='" + entity.RTUId + "'";


            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                conn.Open();
                cmd.CommandText = sql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows) //已有该rtu纪录，update
                {
                    //sql=
                    sql = CreateUpdateRtusetting_Device(entity);
                }
                else//插入新rtu
                {
                    sql = CreateInsertRtuSetting_Device(entity);
                }
                reader.Close();
                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    cmd.CommandText = sql;
                   result= cmd.ExecuteNonQuery() > 0 ? true : false; 
                    
                    trans.Commit();
                    
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
            return result;
        }


        public Rtusetting_Device LoadRtuSetting_Device(string rtuid)
        {
            Rtusetting_Device result = new Rtusetting_Device();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingDeviceFields, "rtusetting_device", " rtuid = '" + rtuid + "'", null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = EntityAssembler.CreateRtuSettingDeviceEntity(reader);
                    }
                }
            }
            return result;
        }

        public List<Rtusetting_Device> LoadRtuSetting_Device()
        {
            List<Rtusetting_Device> list = new List<Rtusetting_Device>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingDeviceFields ,
                    "rtusetting_device", null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateRtuSettingDeviceEntity(reader));
                    }
                }
            }
            return list;
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
            string sqlstr;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (ServiceNote entity in entities)
                    {
                        sqlstr = "insert into servicenote (notetime,note,notetype,state) values('{0}','{1}',{2},{3})";
                        sqlstr = string.Format(sqlstr, entity.NoteTime.ToString(), entity.Note, entity.DataType.ToString(), "0");
                        cmd.CommandText = sqlstr;
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
        }

        /// <summary>
        /// load service note
        /// </summary>
        /// <returns></returns>
        public DataTable LoadServiceNote()
        {
                       
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from ServiceNote";

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;

      
        
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
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    cmd.CommandText = this.CreateAirPressureSqliteInsertSql(entity);
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    return true;

                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
         
        }


        /// <summary>
        /// 读取airpressure
        /// </summary>
        /// <returns></returns>
        public DataTable LoadAirPressure()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select *,cast(strftime('%s',collecttime) as int) as intcollecttime from " + SQLItems.DefaultAirPressureTableName + " order by collecttime desc limit 0,2000";

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        #endregion

        #region 新增通道
        /// <summary>
        /// 新增单个通道信息
        /// </summary>
        /// <param name="entity">通道信息类</param>
        /// <returns>通道编号</returns>
        public string InsertCommunicationSetting(CommunicationSetting entity)
        {
            Object obj = null;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    cmd.CommandText = this.CreateCommSettingSqliteInsertSql(entity);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = SQLItems.CreateSelectSql("MAX(CommunicationId)", SQLItems.DefaultCommSettingTableName, null, null);
                    obj = cmd.ExecuteScalar();
                    trans.Commit();
                    return Convert.ToString(obj);
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
           
        }

        /// <summary>
        /// 批量通道信息新增(编号获取逻辑未加)
        /// </summary>
        /// <param name="entity">通道信息组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertCommunicationSetting(IEnumerable<CommunicationSetting> entity)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (CommunicationSetting commEntity in entity)
                    {
                        cmd.CommandText = this.CreateCommSettingSqliteInsertSql(commEntity);
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
        }

        #endregion

        #region 更新通道

        /// <summary>
        /// 单个通道信息更新
        /// </summary>
        /// <param name="entity">通道信息类</param>
        /// <returns>bool型</returns>
        public bool UpdateCommunicationSetting(CommunicationSetting entity)
        {
            string sql = @"UPDATE " + SQLItems.DefaultCommSettingTableName + " SET UpdateTime=datetime('now','localtime') "
                + (entity.CheckPropertyChanged("CommunicationName") ? ",CommunicationName='{0}'" : "")
                + (entity.CheckPropertyChanged("MobileNo") ? ",MobileNo='{1}'" : "")
                + (entity.CheckPropertyChanged("Note") ? ",Note='{2}'" : "")
                + (entity.CheckPropertyChanged("APN") ? ",APN='{3}'" : "")
                + (entity.CheckPropertyChanged("Dns1") ? ",Dns1='{4}'" : "")
                + (entity.CheckPropertyChanged("Dns2") ? ",Dns2='{5}'" : "")
                + (entity.CheckPropertyChanged("IP") ? ",IP='{6}'" : "")
                + (entity.CheckPropertyChanged("ExTcp") ? ",ExTcp='{7}'" : "")
                + (entity.CheckPropertyChanged("InTcp") ? ",InTcp={8}" : "")
                + (entity.CheckPropertyChanged("CommunicationTypeId") ? ",CommunicationTypeId='{9}'" : "")
                + (entity.CheckPropertyChanged("Tag") ? ",Tag={10}" : "")
                + " WHERE CommunicationId={11}";

            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {

                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = string.Format(sql, entity.CommunicationName, entity.MobileNo, entity.Note, entity.APN, entity.Dns1,
                    entity.Dns2, entity.IP, entity.ExTcp, entity.InTcp, entity.CommunicationTypeId, entity.Tag, entity.CommunicationId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
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
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultCommSettingTableName, " CommunicationId = " + CommunicationId );

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region 获取通道

        /// <summary>
        /// 通道信息获取
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable LoadCommunicationSetting()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultCommSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultCommSettingTableName, null, null);

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 通道信息获取
        /// </summary>
        /// <returns>通道列表</returns>
        public List<CommunicationSetting> LoadCommunicationSettingList()
        {
            List<CommunicationSetting> list = new List<CommunicationSetting>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultCommSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultCommSettingTableName, null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateCommSettingEntity(reader));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 通道信息获取
        /// </summary>
        /// <param name="communicationId">通道信息主键</param>
        /// <returns>通道信息类</returns>
        public CommunicationSetting LoadCommunicationSetting(string communicationId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultCommSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultCommSettingTableName, " CommunicationId = '" + communicationId + "'", null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return EntityAssembler.CreateCommSettingEntity(reader);
                    }
                    return null;
                }
            }
        }

        #endregion

        #region 新增终端

        /// <summary>
        ///  新增单个终端
        /// </summary>
        /// <param name="entity">终端信息</param>
        /// <returns>bool</returns>
        public bool InsertRTUSetting(RTUSetting entity)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = this.CreateRTUSettingSqliteInsertSql(entity);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 批量新增终端(编号获取逻辑未加)
        /// </summary>
        /// <param name="entities">终端信息集合</param>
        /// <returns>bool型</returns>
        public bool BulkInsertRTUSetting(IEnumerable<RTUSetting> entities)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (RTUSetting rtuEntity in entities)
                    {
                        cmd.CommandText = this.CreateRTUSettingSqliteInsertSql(rtuEntity);
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
        }

        #endregion

        #region 获取最新终端编号
        /// <summary>
        /// 获取最新终端编号
        /// </summary>
        /// <returns></returns>
        public string GetNewRtuId()
        {
            string newrtuid = string.Empty;
            Object obj = null;
            try
            {
                #region 获取终端四位编码，序列码（编码规则：十位数字+26位小写字母+26位大写字母）
                int i = 1;
                while (string.IsNullOrEmpty(newrtuid) && i <= 3844)
                {
                    //两位数字编码,最多轮训100次

                    //返回终端编码序列值
                    using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand(conn);

                        conn.Open();

                        SQLiteTransaction trans = conn.BeginTransaction();
                        try
                        {
                            cmd.CommandText = "delete from RtuNewId where num < (select max(num) from RtuNewId)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "insert into RtuNewID(Str)values('')  ";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "select max(Num)  from RtuNewId";
                            obj = cmd.ExecuteScalar();
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                        finally
                        {
                            cmd.Dispose();
                            trans.Dispose();
                        }

                    }

                    if (obj != null)
                    {
                        
                        newrtuid = ConvertData.ConvertCode62(Convert.ToInt32(obj));
                    }
                  
                    if (newrtuid.Length >= 3)
                    {//序列值超过四位，就取后两位编码
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
            catch(Exception e)
            {
                //ToDo 日志记录
                throw e;
            }
        }

        #endregion

        #region 更新终端

        /// <summary>
        /// 更新终端
        /// </summary>
        /// <param name="entity">终端实体</param>
        /// <returns>bool型</returns>
        public bool UpdateRTUSetting(RTUSetting entity)
        {
            string sql = @"UPDATE " + SQLItems.DefaultRtuSettingTableName + " SET UpdateTime=datetime('now','localtime') "
                + (entity.CheckPropertyChanged("RTUName") ? ",RTUName='{0}'" : "")
                + (entity.CheckPropertyChanged("RTUSName") ? ",RTUSName='{1}'" : "")
                + (entity.CheckPropertyChanged("ProductTypeId") ? ",ProductTypeId='{2}'" : "")
                + (entity.CheckPropertyChanged("Manu") ? ",Manu='{3}'" : "")
                + (entity.CheckPropertyChanged("InstallDate") ? ",InstallDate='{4}'" : "")
                + (entity.CheckPropertyChanged("InstallLoca") ? ",InstallLoca='{5}'" : "")
                + (entity.CheckPropertyChanged("InstallAddr") ? ",InstallAddr='{6}'" : "")
                + (entity.CheckPropertyChanged("Analog") ? ",Analog={7}" : "")
                + (entity.CheckPropertyChanged("Digit") ? ",Digit={8}" : "")
                + (entity.CheckPropertyChanged("VoltAlert") ? ",VoltAlert={9}" : "")
                + (entity.CheckPropertyChanged("VoltClose") ? ",VoltClose={10}" : "")
                + (entity.CheckPropertyChanged("CollCycle") ? ",CollCycle={11}" : "")
                + (entity.CheckPropertyChanged("SendCycle") ? ",SendCycle={12}" : "")
                + (entity.CheckPropertyChanged("SaveCycle") ? ",SaveCycle={13}" : "")
                + (entity.CheckPropertyChanged("Note") ? ",Note='{14}'" : "")
                + (entity.CheckPropertyChanged("MobileNo") ? ",MobileNo='{15}'" : "")
                + (entity.CheckPropertyChanged("CommunicationId") ? ",CommunicationId={16}" : "")
                + (entity.CheckPropertyChanged("CommunicationId1") ? ",CommunicationId1={17}" : "")
                + (entity.CheckPropertyChanged("CommunicationId2") ? ",CommunicationId2={18}" : "")
                + (entity.CheckPropertyChanged("IsOpen") ? ",IsOpen={19}" : "")
                + (entity.CheckPropertyChanged("OpenTime1") ? ",OpenTime1='{20}'" : "")
                + (entity.CheckPropertyChanged("OpenTime2") ? ",OpenTime2='{21}'" : "")
                + (entity.CheckPropertyChanged("OpenTime3") ? ",OpenTime3='{22}'" : "")
                + (entity.CheckPropertyChanged("OpenTime4") ? ",OpenTime4='{23}'" : "")
                + (entity.CheckPropertyChanged("OpenTime5") ? ",OpenTime5='{24}'" : "")
                + (entity.CheckPropertyChanged("ServiceCenter") ? ",ServiceCenter='{25}'" : "")
                + (entity.CheckPropertyChanged("TableName") ? ",TableName='{26}'" : "")
                + (entity.CheckPropertyChanged("RegionId") ? ",RegionId={27}" : "")
                + (entity.CheckPropertyChanged("PowerSupply") ? ",PowerSupply={28}" : "")
                + (entity.CheckPropertyChanged("Tag") ? ",Tag={29}" : "")
                + (entity.CheckPropertyChanged("CommunicationTypeId") ? ",CommunicationTypeId='{30}'" : "")
                + (entity.CheckPropertyChanged("Caliber") ? ",Caliber='{31}'" : "")
                + (entity.CheckPropertyChanged("CaliberType") ? ",CaliberType={32}" : "")
                + (entity.CheckPropertyChanged("SendExceptionAlert") ? ",SendExceptionAlert={33}" : "")
                + (entity.CheckPropertyChanged("SendVoltAlert") ? ",SendVoltAlert={34}" : "")
                + (entity.CheckPropertyChanged("CollCycle2") ? ",CollCycle2={35}" : "")
                + (entity.CheckPropertyChanged("SendCycle2") ? ",SendCycle2={36}" : "")
                + (entity.CheckPropertyChanged("SaveCycle2") ? ",SaveCycle2={37}" : "")
                + (entity.CheckPropertyChanged("CollCycle3") ? ",CollCycle3={38}" : "")
                + (entity.CheckPropertyChanged("SendCycle3") ? ",SendCycle3={39}" : "")
                + (entity.CheckPropertyChanged("SaveCycle3") ? ",SaveCycle3={40}" : "")
                + (entity.CheckPropertyChanged("SafetyLevel") ? ",SafetyLevel={41}" : "")
                + (entity.CheckPropertyChanged("AlertLevel") ? ",AlertLevel={42}" : "")
                + (entity.CheckPropertyChanged("SystemVoltage") ? ",SystemVoltage={43}" : "")
                + (entity.CheckPropertyChanged("UltrasonicVoltage") ? ",UltrasonicVoltage={44}" : "")
                + (entity.CheckPropertyChanged("LocationDevication") ? ",LocationDevication={45}" : "")
                + (entity.CheckPropertyChanged("WellDeep") ? ",WellDeep={46}" : "")
                + (entity.CheckPropertyChanged("WellElevation") ? ",WellElevation={47}" : "")
                + (entity.CheckPropertyChanged("PipeElevation") ? ",PipeElevation={48}" : "")
                + (entity.CheckPropertyChanged("PipeCaliber") ? ",PipeCaliber={49}" : "")
                + (entity.CheckPropertyChanged("BackValue") ? ",BackValue={51}" : "")
                + (entity.CheckPropertyChanged("DeviceStatus") ? ",DeviceStatus={52}" : "")
                + (entity.CheckPropertyChanged("CurrentCycle") ? ",CurrentCycle={53}" : "")
                + " WHERE RTUId='{50}'";

            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = string.Format(sql, entity.RTUName, entity.RTUSName, entity.ProductTypeId, entity.Manu,
                    entity.InstallDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), entity.InstallLoca, entity.InstallAddr,
                    entity.Analog, entity.Digit, entity.VoltAlert, entity.VoltClose, entity.CollCycle, entity.SendCycle,
                    entity.SaveCycle, entity.Note, entity.MobileNo, entity.CommunicationId, entity.CommunicationId1,
                    entity.CommunicationId2, Convert.ToInt32(entity.IsOpen), entity.OpenTime1, entity.OpenTime2,
                    entity.OpenTime3, entity.OpenTime4, entity.OpenTime5, entity.ServiceCenter, entity.TableName,
                    entity.RegionId, entity.PowerSupply, entity.Tag, entity.CommunicationTypeId, entity.Caliber,
                    entity.CaliberType,Convert.ToInt32(entity.SendExceptionAlert),Convert.ToInt32(entity.SendVoltAlert), 
                    entity.CollCycle2,entity.SendCycle2,entity.SaveCycle2,entity.CollCycle3,entity.SendCycle3,entity.SaveCycle3,
                    entity.SafetyLevel,entity.AlertLevel,entity.SystemVoltage,entity.UltrasonicVoltage,entity.LocationDevication,
                    entity.WellDeep,entity.WellElevation,entity.PipeElevation,entity.PipeCaliber,
                    entity.RTUId, entity.BackValue, entity.DeviceStatus, entity.CurrentCycle);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
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
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultRtuSettingTableName, " RtuId='" + RtuId + "' ");
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region 获取终端

        /// <summary>
        /// 终端信息获取
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable LoadRTUSetting()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingFields + @", InsertTime, UpdateTime", 
                    SQLItems.DefaultRtuSettingTableName, null, null);

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 终端信息获取
        /// </summary>
        /// <returns>终端列表</returns>
        public List<RTUSetting> LoadRTUSettingList()
        {
            List<RTUSetting> list = new List<RTUSetting>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingFields + @", InsertTime, UpdateTime", 
                    SQLItems.DefaultRtuSettingTableName, null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateRTUSettingEntity(reader));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取终端信息
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>终端实体</returns>
        public RTUSetting LoadRTUSetting(string rtuId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultRtuSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultRtuSettingTableName, " RtuId = '" + rtuId + "'", null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return EntityAssembler.CreateRTUSettingEntity(reader);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取口径表
        /// </summary>
        /// <returns></returns>
        public DataTable LoadCaliberAndFlux()
        {


            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string sql = "select * from CaliberAndFlux";
                cmd.CommandText = sql;

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
        #endregion

        #region 新增检测量配置
        /// <summary>
        /// 新增检测量配置
        /// </summary>
        /// <param name="entity">检测量配置类</param>
        /// <returns>检测量编号</returns>
        public string InsertMeasureSetting(MeasureSetting entity)
        {
            Object obj = null;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = this.CreateMeasureSettingSqliteInsertSql(entity);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    cmd.CommandText = this.CreateMeasureSettingSqliteInsertSql(entity);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = SQLItems.CreateSelectSql("MAX(MeasureId)", SQLItems.DefaultMeasureSettingTableName, null, null);
                    obj = cmd.ExecuteScalar();
                    trans.Commit();
                    return Convert.ToString(obj);
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }
            }
        }

        /// <summary>
        /// 批量检测量配置信息新增
        /// 注：如果成功，传入参数将携带编号返回。否则，不做修改。
        /// </summary>
        /// <param name="entity">检测量配置信息组</param>
        /// <returns>bool型</returns>
        public bool BulkInsertMeasureSetting(IEnumerable<MeasureSetting> entities)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (MeasureSetting entity in entities)
                    {
                        cmd.CommandText = this.CreateMeasureSettingSqliteInsertSql(entity);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = SQLItems.CreateSelectSql("MAX(MeasureId)", SQLItems.DefaultMeasureSettingTableName, null, null);
                        entity.MeasureId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    trans.Commit();
                    return true;
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    foreach (MeasureSetting entity in entities)
                    {
                        entity.MeasureId = 0;
                    }
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
        }

        #endregion

        #region 更新检测量配置

        /// <summary>
        /// 检测量配置更新
        /// </summary>
        /// <param name="entity">检测量配置类</param>
        /// <returns>bool型</returns>
        public bool UpdateMeasureSetting(MeasureSetting entity)
        {
            string sql = @"UPDATE " + SQLItems.DefaultMeasureSettingTableName + " SET UpdateTime=datetime('now','localtime') "
                + (entity.CheckPropertyChanged("MeasureName") ? ",MeasureName='{0}'" : "")
                + (entity.CheckPropertyChanged("RTUId") ? ",RTUId='{1}'" : "")
                + (entity.CheckPropertyChanged("SignalType") ? ",SignalType='{2}'" : "")
                + (entity.CheckPropertyChanged("DataType") ? ",DataType='{3}'" : "")
                + (entity.CheckPropertyChanged("PortId") ? ",PortID='{4}'" : "")
                + (entity.CheckPropertyChanged("Unit") ? ",Unit='{5}'" : "")
                + (entity.CheckPropertyChanged("Range") ? ",Range={6}" : "")
                + (entity.CheckPropertyChanged("Elevation") ? ",Elevation={7}" : "")
                + (entity.CheckPropertyChanged("Precision") ? ",Precision={8}" : "")
                + (entity.CheckPropertyChanged("DecimalDigits") ? ",DecimalDigits={9}" : "")
                + (entity.CheckPropertyChanged("Scale") ? ",Scale={10}" : "")
                + (entity.CheckPropertyChanged("Offset") ? ",Offset={11}" : "")
                + (entity.CheckPropertyChanged("Revise") ? ",Revise={12}" : "")
                + (entity.CheckPropertyChanged("UpperLimit") ? ",UpperLimit={13}" : "")
                + (entity.CheckPropertyChanged("LowerLimit") ? ",LowerLimit={14}" : "")
                + (entity.CheckPropertyChanged("Ratio") ? ",Ratio={15}" : "")
                + (entity.CheckPropertyChanged("Note") ? ",Note='{16}'" : "")
                + (entity.CheckPropertyChanged("Type") ? ",Type={17}" : "")
                + (entity.CheckPropertyChanged("IsOpen") ? ",IsOpen={18}" : "")
                + (entity.CheckPropertyChanged("IsSet") ? ",IsSet={19}" : "")
                + (entity.CheckPropertyChanged("SendOutData") ? ",SendOutData={20}" : "")
                + (entity.CheckPropertyChanged("SendChangeData") ? ",SendChangeData={21}" : "")
                + (entity.CheckPropertyChanged("DataStatus") ? ",DataStatus={22}" : "")
                + (entity.CheckPropertyChanged("SwitchType") ? ",SwitchType={23}" : "")
                + (entity.CheckPropertyChanged("SendRedirectionAlert") ? ",SendRedirectionAlert={24}" : "")
                + " WHERE MeasureId={25}";

            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = string.Format(sql, entity.MeasureName, entity.RTUId, entity.SignalType, entity.DataType,
                    entity.PortId, entity.Unit, entity.Range, entity.Elevation, entity.Precision, entity.DecimalDigits,
                    entity.Scale, entity.Offset, entity.Revise, entity.UpperLimit, entity.LowerLimit, entity.Ratio, entity.Note,
                    entity.Type, Convert.ToInt32(entity.IsOpen), Convert.ToInt32(entity.IsSet), Convert.ToInt32(entity.SendOutData),
                    Convert.ToInt32(entity.SendChangeData), Convert.ToInt32(entity.DataStatus), Convert.ToInt32(entity.SwitchType),
                    Convert.ToInt32(entity.SendRedirectionAlert), entity.MeasureId);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        #endregion

        #region 删除检测量信息
        /// <summary>
        /// 删除检测量信息（编号MeasureId必须赋值）
        /// </summary>
        /// <param name="entity">检测量信息</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureSetting(MeasureSetting entity)
        {
            return this.DeleteMeasureSetting(" MeasureId = " + entity.MeasureId.ToString());
        }

        /// <summary>
        /// 删除检测量信息
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureSetting(string condition)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultMeasureSettingTableName, condition);
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 批量删除检测量
        /// </summary>
        /// <param name="entities">检测量实体列表（编号MeasureId必须赋值）</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureSetting(IEnumerable<MeasureSetting> entities)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString)) {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (MeasureSetting entity in entities)
                    {
                        cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultMeasureSettingTableName, " MeasureId = " + entity.MeasureId.ToString());
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally {
                    cmd.Dispose();
                    trans.Dispose();
                }
            }
        }

        #endregion

        #region 获取检测量配置

        /// <summary>
        /// 检测量配置信息获取
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable LoadMeasureSetting()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                //cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, null, null);
                cmd.CommandText = "select a.*,b.alerttypeid,b.type as alertsign from measuresetting as a left join datatypeconfig as b on a.datatype=b.id";
                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 检测量配置信息获取
        /// </summary>
        /// <returns>检测量列表</returns>
        public List<MeasureSetting> LoadMeasureSettingList()
        {
            List<MeasureSetting> list = new List<MeasureSetting>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateMeasureSettingEntity(reader));
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// 检测量配置信息获取
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>DataTable</returns>
        public List<MeasureSetting> LoadMeasSettingByRtuId(string rtuId)
        {
            List<MeasureSetting> list = new List<MeasureSetting>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, " RtuId='" + rtuId + "'", null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateMeasureSettingEntity(reader));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 检测量配置信息获取
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns>DataTable</returns>
        public DataTable LoadMeasureSettingByRtuId(string rtuId)
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, " RtuId='" + rtuId + "'", null);

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 检测量配置信息获取
        /// </summary>
        /// <param name="measureId">检测量编号</param>
        /// <returns>通道信息类</returns>
        public MeasureSetting LoadMeasureSetting(string measureId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureSettingFields + @", InsertTime, UpdateTime", SQLItems.DefaultMeasureSettingTableName, " MeasureId = " + measureId, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return EntityAssembler.CreateMeasureSettingEntity(reader);
                    }
                    return null;
                }
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
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql("Distinct ProductTypeId", SQLItems.DefaultRtuTempletTableName, null, "ProductTypeId");

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        ///  获取产品类型编号集合
        /// </summary>
        /// <param name="communicationTypeId">通讯协议类型</param>
        /// <returns>产品类型编号</returns>
        public string LoadProductTypeId(string communicationTypeId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql("Distinct ProductTypeId", SQLItems.DefaultRtuTempletTableName, " CommunicationTypeId='" + communicationTypeId + "'", "ProductTypeId");

                conn.Open();
                return cmd.ExecuteScalar() as string;
            }
        }

        /// <summary>
        /// 获取检测量模板
        /// </summary>
        /// <param name="productTypeId">产品类型编号</param>
        /// <returns>检测量模板集合</returns>
        public List<MeasureTemplet> LoadMeasureTempletByProductTypeId(string productTypeId)
        {
            List<MeasureTemplet> list = new List<MeasureTemplet>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefalutMeasureTempletFields, SQLItems.DefaultMeasureTempletTableName, " ProductTypeId = '" + productTypeId + "'", null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateMeasureTempletEntity(reader));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取终端模板
        /// </summary>
        /// <param name="productTypeId">产品类型编号</param>
        /// <returns>终端模板</returns>
        public RTUTemplet LoadRtuTemplet(string productTypeId)
        {
            RTUTemplet templet = new RTUTemplet();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultRtuTempletFields, SQLItems.DefaultRtuTempletTableName, " ProductTypeId = '" + productTypeId + "'", null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        templet = EntityAssembler.CreateRtuTempletEntity(reader);
                    }
                }
            }
            return templet;
        }
        /// <summary>
        /// 修改终端模板
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateRtuTemplet(RTUTemplet entity)
        {



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
                + (entity.CheckPropertyChanged("BackValue") ? ",BackValue={27}" : "")
               
               + " WHERE [ProductTypeId]='{28}'";

            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = string.Format(sql, entity.Manu, entity.Analog, entity.Digit, entity.VoltAlert,
                    entity.VoltClose, entity.CollCycle, entity.SendCycle, entity.SaveCycle, entity.Note, entity.ServiceCenter,
                    Convert.ToInt32(entity.Tag),  entity.CaliberType,entity.CollCycle2,entity.SendCycle2,entity.SaveCycle2,entity.CollCycle3,entity.SendCycle3,entity.SaveCycle3,
                   entity.SafetyLevel,entity.AlertLevel,entity.SystemVoltage,entity.UltrasonicVoltage,entity.LocationDevication,entity.WellDeep,entity.WellElevation,entity.PipeElevation,entity.PipeCaliber,entity.BackValue, entity.ProductTypeId);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }


        }

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
               + (entity.CheckPropertyChanged("Range") ? ",Range={5}" : "")
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
               + " WHERE [MTempletId]={17}";


            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = string.Format(sql, entity.MTempletName, entity.ProductTypeId, entity.Manu, entity.SignalType,
                entity.Unit, entity.Range, entity.Precision, entity.DecimalDigits, entity.Offset, entity.UpperLimit,
                entity.LowerLimit, entity.Ratio, entity.Note, entity.DataType, entity.PortId, Convert.ToInt32(entity.IsSet), entity.Scale, entity.MTempletId);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }

        }

        #region 删除检测量模板

        /// <summary>
        /// 删除单条检测量模板信息
        /// </summary>
        /// <param name="entity">检测量模板信息</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureTemplet(MeasureTemplet entity)
        {
            return this.DeleteMeasureTemplet(" MTempletId = " + entity.MTempletId);
        }

        /// <summary>
        /// 删除检测量模板信息
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>bool型</returns>
        public bool DeleteMeasureTemplet(string condition)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultMeasureTempletTableName, condition);
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
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
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = sql;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }



        /// <summary>
        /// 获取所有模板（Rtu和Measure）
        /// </summary>
        /// <returns>Rtu和Measure关联模板</returns>
        public DataTable LoadAllTemplet()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.AllTempletFields, SQLItems.AllTempletTables, null, null);

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }


        /// <summary>
        /// 检索编号是否存在
        /// </summary>
        /// <param name="rtuId">终端编号</param>
        /// <returns></returns>
        public bool Exists(string rtuId)
        {
            object o = null;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql("COUNT(1)", SQLItems.DefaultRtuSettingTableName, " RtuId = '" + rtuId + "'", null); ;

                conn.Open();
                o = cmd.ExecuteScalar();
            }
            if (o != null && Convert.ToInt32(o) > 0)
            {
                return true;
            }
            else return false;
        }

        #region 获取通讯协议类型
        /// <summary>
        /// 获取通讯协议类型
        /// </summary>
        /// <returns>通讯协议类型</returns>
        public List<CommunicationType> LoadCommunicationType()
        {
            List<CommunicationType> list = new List<CommunicationType>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultCommunicationTypeFields, SQLItems.DefaultCommunicationTypeTableName, null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateCommunicationTypeEntity(reader));
                    }
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
            List<DataTypeConfig> list = new List<DataTypeConfig>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultDataTypeFields, SQLItems.DefaultDataTypeTableName, null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateDataTypeEntity(reader));
                    }
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
            List<string> list = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql("PortId", "ProductPortLink", "ProductTypeId='" + productTypeId + "'", null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader["PortId"] as string);
                    }
                }
            }
            return list;
        }

        #endregion

        #region 操作痕迹
        /// <summary>
        /// 新增操作痕迹
        /// </summary>
        /// <param name="entity">操作痕迹实体类</param>
        /// <returns>bool型</returns>
        public bool InsertConfigTraceLog(ConfigTrack entity)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = this.CreateConfigTrackSqliteInsertSql(entity);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
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
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultConfigTrackTableName, condition);
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
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
            object o = null;
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select count(1) from userinfo where account='" + userInfo.Account + "' and pwd='" + userInfo.PWD + "'";

                conn.Open();

                o = cmd.ExecuteScalar();
            }
            if (o != null && Convert.ToInt32(o) > 0)
            {
                return true;
            }
            else return false;
        }
        #endregion

        #endregion     
        #region DLG 附加
        /// <summary>
        /// load devicetable
        /// </summary>
        /// <returns></returns>
      public  DataTable LoadDeviceTable(string rtuid)
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultDeviceFields,
                    SQLItems.DefaultDeviceTableName,"rtuid='" + rtuid + "'", null);

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
        /// <summary>
        /// load datablock
        /// </summary>
        /// <returns></returns>
       public DataTable LoadDatablock(string rtuid)
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                string sql = "select * from {0} where deviceid in (select deviceid from {1} where rtuid='{2}')";
                sql = string.Format(sql, SQLItems.DefaultDatablockTableName, SQLItems.DefaultDeviceTableName, rtuid);
                cmd.CommandText = sql;

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
       public DataTable LoadDLGDataType()
       {
           DataTable dt = new DataTable();
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);
               cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultDLGDatatypeFields,
                   SQLItems.DefaultDLGDatatypeTableName, null, null);

               conn.Open();

               SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
               adapter.Fill(dt);
           }
           return dt;
       }
        /// <summary>
        /// insert new rtu to unconfiguredrtu table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool InsertUnconfiguredRtu(UnConfiguredRTU entity)
       {
           bool result = false;
           Object obj = null;
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);
               DataTable dt = new DataTable();
               
               cmd.CommandText = "select * from UnConfiguredRtu where rtuid='" + entity.RtuID + "'";

               conn.Open();
               SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
               adapter.Fill(dt);
               //已有该记录
               if (dt.Rows.Count > 0)
               {
                   return true;
               }

               SQLiteTransaction trans = conn.BeginTransaction();
               try
               {
                   string strsql = "insert into UnConfiguredRtu(rtuid,producttypeid,inserttime) values ('{0}','{1}',{2})";
                   strsql = string.Format(strsql, entity.RtuID, entity.ProductTypeID, entity.InsertTime);
                   cmd.ExecuteNonQuery();
                   trans.Commit();
                   result = true;
               }
               catch (Exception e)
               {
                   trans.Rollback();
                   throw e;
               }
               finally
               {
                   cmd.Dispose();
                   trans.Dispose();
               }
           }


           return result;
       }

       public UnConfiguredRTU LoadUnconfiguredRtu(string rtuid)
       {
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);
               cmd.CommandText = "select rtuid,ProductTypeId,InsertTime,Tag from unconfiguredrtu where rtuid='" + rtuid + "'";

               conn.Open();

               using (SQLiteDataReader reader = cmd.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       return EntityAssembler.CreateUnconfiguredRtu(reader);
                   }
                   return null;
               }
           }
       }


        /// <summary>
        /// 批量插入datablocksetting
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
       public bool BulkInsertDatablock(IEnumerable<DatablockSetting> entitys)
       {
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);

               conn.Open();

               SQLiteTransaction trans = conn.BeginTransaction();
               try
               {
                   foreach (DatablockSetting datablocksetting in entitys)
                   {
                       cmd.CommandText = this.CreateDatablockSettingSqliteInsertSql(datablocksetting);
                       cmd.ExecuteNonQuery();
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
                   cmd.Dispose();
                   trans.Dispose();
               }

           }
       }


        /// <summary>
        /// update datablock setting
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool UpdateDatablock(DatablockSetting entity)
       {

           //BlockID,MeasureId,DeviceId,DataTypeId,Swap,Start,Length
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
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);

               cmd.CommandText =sql;
               conn.Open();

               return cmd.ExecuteNonQuery() > 0;
           }
       }
        /// <summary>
        /// 删除datalblcok
        /// </summary>
        /// <param name="blockid"></param>
        /// <returns></returns>
       public bool DeleteDataBlock(string rtuid)
       {
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);
               string sql = "delete from {0} where deviceid in (select deviceid from {1} where rtuid='{2}')";
               sql = string.Format(sql, SQLItems.DefaultDatablockTableName, SQLItems.DefaultDeviceTableName, rtuid);
               cmd.CommandText = sql;
               conn.Open();

               return cmd.ExecuteNonQuery() > 0;
           }
       }
       public bool DeleteDataBlock(int blockid)
       {
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);
               string sql = "delete from {0} where blockid='{1}'";
               sql = string.Format(sql, SQLItems.DefaultDatablockTableName, blockid.ToString());
               cmd.CommandText = sql;
               conn.Open();

               return cmd.ExecuteNonQuery() > 0;
           }
       }
       /// <summary>
       /// 批量新增devicesetting
       /// </summary>
       /// <param name="entitys"></param>
       /// <returns></returns>
        public bool BulkInsertDeviceTable(IEnumerable<DeviceSetting> entitys)
       {
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);

               conn.Open();

               SQLiteTransaction trans = conn.BeginTransaction();
               try
               {
                   foreach (DeviceSetting devicesetting in entitys)
                   {
                       cmd.CommandText = this.CreateDeviceSettingSqliteInsertSql(devicesetting);
                       cmd.ExecuteNonQuery();
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
                   cmd.Dispose();
                   trans.Dispose();
               }

           }
       }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool UpdateDeviceSetting(DeviceSetting entity)
       {
           //BlockID,MeasureId,DeviceId,DataTypeId,Swap,Start,Length
           string sql = @"UPDATE " + SQLItems.DefaultDeviceTableName + " SET "
               + (entity.CheckPropertyChanged("RtuID") ? "RtuID='{1}'" : "")
               + (entity.CheckPropertyChanged("DeviceName") ? ",DeviceName='{2}'" : "")
               + (entity.CheckPropertyChanged("CommandCount") ? ",CommandCount='{3}'" : "")
               + " WHERE DeviceID={0}";
           sql = string.Format(sql, entity.DeviceID, entity.RtuID, entity.DeviceName,entity.CommandCount);
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);

               cmd.CommandText = sql;
               conn.Open();

               return cmd.ExecuteNonQuery() > 0;
           }
       }
       public bool DeleteDeviceSetting(string rtuid)
       {
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);
               cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultDeviceTableName, " rtuId='" + rtuid + "' ");
               conn.Open();

               return cmd.ExecuteNonQuery() > 0;
           }
       }
       public bool DeleteDeviceSetting(int deviceid)
       {
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);
               cmd.CommandText = SQLItems.CreateDeleteSql(SQLItems.DefaultDeviceTableName, " deviceid='" + deviceid.ToString() + "' ");
               conn.Open();

               return cmd.ExecuteNonQuery() > 0;
           }
       }
       public string InsertDatablock(DatablockSetting entity)
       {
           Object obj = null;
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);

               string sql = this.CreateDatablockSettingSqliteInsertSql(entity);

               conn.Open();

               SQLiteTransaction trans = conn.BeginTransaction();
               try
               {
                   cmd.CommandText = sql;
                   cmd.ExecuteNonQuery();
                   cmd.CommandText = SQLItems.CreateSelectSql("MAX(BlockID)", SQLItems.DefaultDatablockTableName, null, null);
                   obj = cmd.ExecuteScalar();
                   trans.Commit();
                   return Convert.ToString(obj);
               }
               catch (Exception e)
               {
                   trans.Rollback();
                   throw e;
               }
               finally
               {
                   cmd.Dispose();
                   trans.Dispose();
               }
           }
       }
       public string InsertDevice(DeviceSetting entity)
       {
           Object obj = null;
           using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
           {
               SQLiteCommand cmd = new SQLiteCommand(conn);

               string sql = this.CreateDeviceSettingSqliteInsertSql(entity);

               conn.Open();

               SQLiteTransaction trans = conn.BeginTransaction();
               try
               {
                   cmd.CommandText = sql;
                   cmd.ExecuteNonQuery();
                   cmd.CommandText = SQLItems.CreateSelectSql("MAX(Deviceid)", SQLItems.DefaultDeviceTableName, null, null);
                   obj = cmd.ExecuteScalar();
                   trans.Commit();
                   return Convert.ToString(obj);
               }
               catch (Exception e)
               {
                   trans.Rollback();
                   throw e;
               }
               finally
               {
                   cmd.Dispose();
                   trans.Dispose();
               }
           }
       }


        #endregion



        #region 液位仪对应关系配置
        /// <summary>
        /// 获取RtuId和ObjectId的对应关系
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> LoadRtuObjectRelation()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql("RtuId,ObjectId", "RtuObjectLink", string.Empty, string.Empty);

                conn.Open();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dic.Add(reader["RtuId"] as string, Convert.ToInt32(reader["ObjectId"] as string));
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取MeasureId和TypeId的对应关系
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> LoadMeasureTypeRelation()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql("a.MeasureId,b.TypeId", "MeasureSetting a,PortTypeIdLink b", "a.PortId = b.PortId", string.Empty);

                conn.Open();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dic.Add(Convert.ToInt32(reader["MeasureId"] as string), Convert.ToInt32(reader["TypeId"] as string));
                    }
                }
            }
            return dic;
        }
        #endregion

        #region Sqlite Insert Sql
        private string CreateAirPressureSqliteInsertSql(AirPressure entity)
        {
            string sql = @"insert into {0} (CollectTime,AirPressure) values('{1}','{2}')";
            sql = string.Format(sql, SQLItems.DefaultAirPressureTableName, entity.CollectTime.ToString("yyyy-MM-dd HH:mm:ss"), entity.AirPressureValue.ToString());

            return sql;
        }



        /// <summary>
        /// 构建通道sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">通道实体</param>
        /// <returns>执行语句</returns>
        private string CreateCommSettingSqliteInsertSql(CommunicationSetting entity)
        {
            string sql = @"INSERT INTO {11} (CommunicationName, MobileNo, Note, APN, Dns1, Dns2, IP, ExTcp, InTcp, CommunicationTypeId, Tag) 
                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10})";
            return string.Format(sql, entity.CommunicationName, entity.MobileNo, entity.Note, entity.APN, entity.Dns1, entity.Dns2, entity.IP, entity.ExTcp, entity.InTcp, entity.CommunicationTypeId, entity.Tag,
                SQLItems.DefaultCommSettingTableName);
        }

        /// <summary>
        /// 构建终端sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">终端实体</param>
        /// <returns>执行语句</returns>
        private string CreateRTUSettingSqliteInsertSql(RTUSetting entity)
        {
//            string sql = @"INSERT INTO {36} ({37}) 
//                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10},{11},{12},{13},{14},'{15}','{16}','{17}','{18}','{19}',{20},'{21}','{22}',
//                        '{23}','{24}','{25}','{26}','{27}',{28},{29},{30},'{31}','{32}',{33},{34},{35})";

            string sql = @"INSERT INTO {51} ({52}) 
                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10},{11},{12},{13},{14},'{15}','{16}','{17}','{18}','{19}',{20},'{21}','{22}',
                        '{23}','{24}','{25}','{26}','{27}',{28},{29},{30},'{31}','{32}',{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{53},{54},{55})";
//[CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber]
            return string.Format(sql, entity.RTUId, entity.RTUName, entity.RTUSName, entity.ProductTypeId, entity.Manu,
                (entity.InstallDate == DateTime.MinValue ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : entity.InstallDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff")),
                entity.InstallLoca, entity.InstallAddr, entity.Analog, entity.Digit, entity.VoltAlert, entity.VoltClose, entity.CollCycle, entity.SendCycle,
                entity.SaveCycle, entity.Note, entity.MobileNo, entity.CommunicationId, entity.CommunicationId1, entity.CommunicationId2, Convert.ToInt32(entity.IsOpen), entity.OpenTime1,
                entity.OpenTime2, entity.OpenTime3, entity.OpenTime4, entity.OpenTime5, entity.ServiceCenter,
                entity.TableName, entity.RegionId, entity.PowerSupply, entity.Tag, entity.CommunicationTypeId,
                entity.Caliber, entity.CaliberType, Convert.ToInt32(entity.SendExceptionAlert), Convert.ToInt32(entity.SendVoltAlert), entity.CollCycle2, entity.SendCycle2, entity.SaveCycle2,
                entity.CollCycle3, entity.SendCycle3, entity.SaveCycle3, entity.SafetyLevel, entity.AlertLevel, entity.SystemVoltage, entity.UltrasonicVoltage, entity.LocationDevication,
                entity.WellDeep, entity.WellElevation, entity.PipeElevation, entity.PipeCaliber,
                SQLItems.DefaultRtuSettingTableName, SQLItems.DefaultRtuSettingFields, entity.BackValue, 1, 0);
        }


        private string CreateDatablockSettingSqliteInsertSql(DatablockSetting entity)
        {
            string sql;
            if (entity.DataBlockID > 0)
            {
                sql = @"insert into {0}  (MeasureId,DeviceId,DataTypeId,Swap,Start,Length,BlockID,Digit) values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
                sql = string.Format(sql, SQLItems.DefaultDatablockTableName, entity.Measureid,
           entity.DeviceID, entity.DatatypeID, entity.Swap, entity.Start, entity.Length,entity.DataBlockID,entity.Digit);
            }
            else
            { 
            sql = @"insert into {0}  (MeasureId,DeviceId,DataTypeId,Swap,Start,Length,Digit) values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
            sql = string.Format(sql, SQLItems.DefaultDatablockTableName, entity.Measureid,
            entity.DeviceID, entity.DatatypeID, entity.Swap, entity.Start, entity.Length,entity.Digit);
            }
            //BlockID,MeasureId,DeviceId,DataTypeId,Swap,Start,Length
            
            return sql;

        }

      
        private string CreateDeviceSettingSqliteInsertSql(DeviceSetting entity)
        {
            string sql;
            if (entity.DeviceID > 0)
            {
                sql = @"insert into {0}  (RtuID,DeviceName,deviceid,CommandCount) values ('{1}','{2}','{3}','{4}')";
                sql = string.Format(sql, SQLItems.DefaultDeviceTableName, entity.RtuID, entity.DeviceName, entity.DeviceID,entity.CommandCount);
            }
            else
            {
                sql = @"insert into {0}  (RtuID,DeviceName,CommandCount) values ('{1}','{2}','{3}')";
                sql = string.Format(sql, SQLItems.DefaultDeviceTableName, entity.RtuID, entity.DeviceName,entity.CommandCount);
            }
            //BlockID,MeasureId,DeviceId,DataTypeId,Swap,Start,Length
           
                
            return sql;

        }
        /// <summary>
        /// 构建检测量配置sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">检测量配置实体</param>
        /// <returns>执行语句</returns>
        private string CreateMeasureSettingSqliteInsertSql(MeasureSetting entity)
        {
            string sql = @"INSERT INTO {25} (MeasureName, RTUId, SignalType, DataType, PortId, Unit, Range, Elevation, Precision, DecimalDigits, Scale, Offset, Revise, 
                                                            UpperLimit, LowerLimit, Ratio, Note, Type, IsOpen, IsSet, SendOutData, SendChangeData, DataStatus, SwitchType, SendRedirectionAlert) 
                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},'{16}',{17},{18},{19},{20},{21},
                        {22},{23},{24})";

            return string.Format(sql, entity.MeasureName, entity.RTUId, entity.SignalType, entity.DataType, entity.PortId, entity.Unit, entity.Range, entity.Elevation, entity.Precision, entity.DecimalDigits, entity.Scale, entity.Offset, entity.Revise,
                                      entity.UpperLimit, entity.LowerLimit, entity.Ratio, entity.Note, entity.Type, Convert.ToInt32(entity.IsOpen), Convert.ToInt32(entity.IsSet), Convert.ToInt32(entity.SendOutData), Convert.ToInt32(entity.SendChangeData),
                                      Convert.ToInt32(entity.DataStatus), Convert.ToInt32(entity.SwitchType), Convert.ToInt32(entity.SendRedirectionAlert),
                                      SQLItems.DefaultMeasureSettingTableName);
        }

        /// <summary>
        /// 构建配置痕迹sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">配置痕迹实体</param>
        /// <returns>执行语句</returns>
        private string CreateConfigTrackSqliteInsertSql(ConfigTrack entity)
        {
            string sql = @"INSERT INTO {5} ({6}) 
                        VALUES('{0}','{1}','{2}','{3}','{4}')";

            return string.Format(sql, (entity.OpTime == DateTime.MinValue ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : entity.OpTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")), entity.OpType, entity.OpCode, entity.OpKeyId, entity.OpRemark, SQLItems.DefaultConfigTrackTableName, SQLItems.DefaultConfigTrackFields);
        }
        private string CreateUpdateRtusetting_Device(Rtusetting_Device entity)
        {
            string result = string.Empty;
           
            result = @"update rtusetting_device set rtuid='{0}' "
                + (entity.CheckPropertyChanged("ProductType") ? ",ProductType='{1}'" : "")
                + (entity.CheckPropertyChanged("SoftwareVersion") ? ",SoftwareVersion='{2}'" : "")
                + (entity.CheckPropertyChanged("HardwareVersion") ? ",HardwareVersion='{3}'" : "")
                + (entity.CheckPropertyChanged("APN") ? ",APN='{4}'" : "")
                + (entity.CheckPropertyChanged("IP1") ? ",IP1='{5}'" : "")
                + (entity.CheckPropertyChanged("IP2") ? ",IP2='{6}'" : "")
                + (entity.CheckPropertyChanged("Port1") ? ",Port1='{7}'" : "")
                + (entity.CheckPropertyChanged("Port2") ? ",Port2='{8}'" : "")
                + (entity.CheckPropertyChanged("SecretIP") ? ",SecretIP='{9}'" : "")
                + (entity.CheckPropertyChanged("SecretPort") ? ",SecretPort='{10}'" : "")
                + (entity.CheckPropertyChanged("Savecycle1") ? ",Savecycle1='{11}'" : "")
                + (entity.CheckPropertyChanged("Sendcycle1") ? ",Sendcycle1='{12}'" : "")
                + (entity.CheckPropertyChanged("SaveCycle2") ? ",SaveCycle2='{13}'" : "")
                + (entity.CheckPropertyChanged("Sendcycle2") ? ",Sendcycle2='{14}'" : "")
                + (entity.CheckPropertyChanged("BackValue") ? ",BackValue='{15}'" : "")
                + (entity.CheckPropertyChanged("AirPressure") ? ",AirPressure='{16}'" : "")
                + (entity.CheckPropertyChanged("Scale") ? ",Scale='{17}'" : "")

                + (entity.CheckPropertyChanged("PressureSendChangeAlert") ? ",PressureSendChangeAlert='{18}'" : "")
                + (entity.CheckPropertyChanged("PressureSendOutDataAlert") ? ",PressureSendOutDataAlert='{19}'" : "")
                + (entity.CheckPropertyChanged("Uplimit") ? ",Uplimit='{20}'" : "")
                + (entity.CheckPropertyChanged("LowLimit") ? ",LowLimit='{21}'" : "")
                + (entity.CheckPropertyChanged("IncreaseInterval") ? ",IncreaseInterval='{22}'" : "")

                + " where rtuid='{0}'";

            result = string.Format(result, entity.RTUId, entity.ProductType, entity.SoftwareVersion, entity.HardwareVersion,
                entity.APN, entity.IP1, entity.IP2, entity.Port1, entity.Port2, entity.SecretIP, entity.SecretPort, entity.Savecycle1,
                entity.Sendcycle1, entity.SaveCycle2, entity.Sendcycle2, entity.BackValue, entity.AirPressure, entity.Scale,
                entity.PressureSendChangeAlert,entity.PressureSendOutDataAlert,entity.Uplimit,entity.LowLimit,entity.IncreaseInterval);

           

            return result;
        }
        /// <summary>
        /// 组建insert语句，rtusetting_device
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string CreateInsertRtuSetting_Device(Rtusetting_Device entity)
        {
            string result = string.Empty;
            result = "insert into rtusetting_device ({0}) values ({1})";

            string fields = "rtuid";
            string values = "'" + entity.RTUId + "'";
            if(entity.CheckPropertyChanged("ProductType"))
            {
                fields = fields + ",ProductType";
                values = values + ",'" + entity.ProductType + "'";
            }

            if (entity.CheckPropertyChanged("SoftwareVersion"))
            {
                fields = fields + ",SoftwareVersion";
                values = values + ",'" + entity.SoftwareVersion + "'";
            }
            if (entity.CheckPropertyChanged("HardwareVersion"))
            {
                fields = fields + ",HardwareVersion";
                values = values + ",'" + entity.HardwareVersion + "'";
            }
            if (entity.CheckPropertyChanged("APN"))
            {
                fields = fields + ",APN";
                values = values + ",'" + entity.APN + "'";
            }
            if (entity.CheckPropertyChanged("IP1"))
            {
                fields = fields + ",IP1";
                values = values + ",'" + entity.IP1 + "'";
            }
            if (entity.CheckPropertyChanged("IP2"))
            {
                fields = fields + ",IP2";
                values = values + ",'" + entity.IP2 + "'";
            }
            if (entity.CheckPropertyChanged("Port1"))
            {
                fields = fields + ",Port1";
                values = values + ",'" + entity.Port1 + "'";
            }
            if (entity.CheckPropertyChanged("Port2"))
            {
                fields = fields + ",Port2";
                values = values + ",'" + entity.Port2 + "'";
            }
            if (entity.CheckPropertyChanged("SecretIP"))
            {
                fields = fields + ",SecretIP";
                values = values + ",'" + entity.SecretIP + "'";
            }
            if (entity.CheckPropertyChanged("SecretPort"))
            {
                fields = fields + ",SecretPort";
                values = values + ",'" + entity.SecretPort + "'";
            }
            if (entity.CheckPropertyChanged("Savecycle1"))
            {
                fields = fields + ",Savecycle1";
                values = values + ",'" + entity.Savecycle1 + "'";
            }
            if (entity.CheckPropertyChanged("Sendcycle1"))
            {
                fields = fields + ",Sendcycle1";
                values = values + ",'" + entity.Sendcycle1 + "'";
            }
            if (entity.CheckPropertyChanged("Sendcycle1"))
            {
                fields = fields + ",SaveCycle2";
                values = values + ",'" + entity.SaveCycle2 + "'";
            }
            if (entity.CheckPropertyChanged("Sendcycle2"))
            {
                fields = fields + ",Sendcycle2";
                values = values + ",'" + entity.Sendcycle2 + "'";
            }
            if (entity.CheckPropertyChanged("BackValue"))
            {
                fields = fields + ",BackValue";
                values = values + ",'" + entity.BackValue + "'";
            }
            if (entity.CheckPropertyChanged("AirPressure"))
            {
                fields = fields + ",AirPressure";
                values = values + ",'" + entity.AirPressure + "'";
            }
            if (entity.CheckPropertyChanged("Scale"))
            {
                fields = fields + ",Scale";
                values = values + ",'" + entity.Scale + "'";
            }
            if (entity.CheckPropertyChanged("PressureSendChangeAlert"))
            {
                fields = fields + ",PressureSendChangeAlert";
                values = values + ",'" + entity.PressureSendChangeAlert + "'";

            }
            if (entity.CheckPropertyChanged("PressureSendOutDataAlert"))
            {
                fields = fields + ",PressureSendOutDataAlert";
                values = values + ",'" + entity.PressureSendOutDataAlert + "'";

            }
            if (entity.CheckPropertyChanged("Uplimit"))
            {
                fields = fields + ",Uplimit";
                values = values + ",'" + entity.Uplimit + "'";

            }
            if (entity.CheckPropertyChanged("LowLimit"))
            {
                fields = fields + ",LowLimit";
                values = values + ",'" + entity.LowLimit + "'";

            }
            if (entity.CheckPropertyChanged("IncreaseInterval"))
            {
                fields = fields + ",IncreaseInterval";
                values = values + ",'" + entity.IncreaseInterval + "'";

            }

            result = string.Format(result,fields,values);

            return result;
        }
        #endregion

        public bool DeleteRTULog(string rtuid)
        {
            string sql = "delete from RTULOG where rtuid = '@rtuid'";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@rtuid", DbType.String);
                        cmd.Parameters["@rtuid"].Value = rtuid;
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        

        public bool InsertRTULog(RTULog item)
        {
            string sql_s = "select count(0) from RTULog where RTUID = '{0}'";
            string sql_i = @"insert into RTULog (IP, RTUID, Time, Data) values('{0}', '{1}', '{2}', '{3}')";
            string sql_u = "update RTULog set IP='{0}', Time='{2}', Data='{3}' where RTUID='{1}'";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
                {
                    conn.Open();

                    using (SQLiteTransaction trans = conn.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {
                            cmd.Connection = conn;

                            cmd.CommandText = string.Format(sql_s, item.RTUID);
                            int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                            string sql = rowCount > 0 ? sql_u : sql_i;

                            cmd.CommandText = string.Format(sql, item.IP, item.RTUID, item.Time.ToString(), item.Data);
                            int result = cmd.ExecuteNonQuery();
                            if (result <= 0)
                            {
                                trans.Rollback();
                                return false;
                            }

                            trans.Commit();
                        }
                    }
                    conn.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<RTULog> LoadAllRTULog()
        {
            string sql = "select RTUID, IP, Time, Data from RTULog";

            List<RTULog> result = new List<RTULog>();

            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString)) 
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn)) 
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read()) 
                        {
                            result.Add(new RTULog() { 
                                RTUID = reader[0].ToString(),
                                IP = reader[1].ToString(),
                                Time = Convert.ToDateTime(reader.GetString(2)),
                                Data = reader[3].ToString()
                            });
                        }

                        reader.Close();
                    }
                }
                conn.Close();
            }

            return result;
        }
        public void BulkInsertRTUlog(IEnumerable<RTULog> rtulogs)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    string sql_s = "select count(0) from RTULog where RTUID = '{0}'";
                    string sql_i = @"insert into RTULog (IP, RTUID, Time, Data) values('{0}', '{1}', '{2}', '{3}')";
                    string sql_u = "update RTULog set IP='{0}', Time='{2}', Data='{3}' where RTUID='{1}'";
                    cmd.Connection = conn;                   

                    foreach (RTULog rtulog in rtulogs)
                    {
                        cmd.CommandText = string.Format(sql_s, rtulog.RTUID);
                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                        string sql = rowCount > 0 ? sql_u : sql_i;
                        cmd.CommandText = string.Format(sql, rtulog.IP, rtulog.RTUID, rtulog.Time.ToString(), rtulog.Data);
                        cmd.ExecuteNonQuery();
                       
                    }
                    trans.Commit();
                   
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
        }
    }
}
