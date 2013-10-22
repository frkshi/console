using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataEntity;
using DataAccess.Interfaces;
using MtuConsole.Common;


namespace DataAccess.SqlServer
{
    public class SqlServerAlertDataRepository : SqlServerRepositoryBase, IAlertDataRepository, ICheckConnection
    {
        private MtuLog _logger = null;

        #region Constructors
        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerAlertDataRepository(string connectionString) : base(connectionString) {
            _logger = new MtuLog();
        }

        #endregion

        #region IMeasureDataRepository Members

        /// <summary>
        /// 单个报警量保存至数据库
        /// </summary>
        /// <param name="entity">报警量</param>
        /// <returns>是否保存成功</returns>
        public bool Insert(AlertData entity)
        {
            bool result = true;
            using (SqlConnection conn = this.AdoHelper.GetConnection(this.ConnectionString) as SqlConnection)
            {
                string sql = @"INSERT INTO " + SQLItems.DefaultAlertDataTableName + " (" + SQLItems.DefaultAlertDataFields + @") 
                            VALUES(@MeasureId,@CollStartTime,@StartNum,@CollEndTime,@EndNum,@Ratio,@AlertTypeId,@Tag,@Sign,@RTUId)";
                SqlParameter[] para = this.CreateSqlParameters(entity);

                result = this.AdoHelper.ExecuteNonQuery(conn, CommandType.Text, sql, para) > 0;
                //执行统计newdata数据逻辑
                SqlParameter[] para2 = this.CreateSqlParametersForNewData(entity);
                this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_newdata", para2);
                
            }
            return result;
        }

        /// <summary>
        /// 批量报警量保存至数据库
        /// </summary>
        /// <param name="entities">报警量列表</param>
        /// <returns>是否保存成功</returns>
        public bool BulkInsert(IEnumerable<AlertData> entities)
        {
            bool result = true;

            DataTable dt = new DataTable();
            dt.Columns.Add("MeasureId",typeof(string));
            dt.Columns.Add("CollStartTime",typeof(DateTime));
            dt.Columns.Add("StartNum", typeof(decimal));
            dt.Columns.Add("CollEndTime",typeof(DateTime));
            dt.Columns.Add("EndNum",typeof(decimal));
            dt.Columns.Add("Ratio", typeof(decimal));
            dt.Columns.Add("AlertTypeId", typeof(Int32));
            dt.Columns.Add("Tag",typeof(Int32));
            dt.Columns.Add("Sign",typeof(byte));
            dt.Columns.Add("RTUId", typeof(string));

            foreach (AlertData entity in entities)
            {
                DataRow dr = dt.NewRow();
                dr["MeasureId"] = entity.MeasureId;
                dr["CollStartTime"] = entity.CollStartTime;
                dr["StartNum"] = entity.StartNum;
                dr["CollEndTime"] = entity.CollEndTime==DateTime.MinValue? DateTime.Now : entity.CollEndTime;
                dr["EndNum"] = entity.EndNum;
                dr["Ratio"] = entity.Ratio;
                dr["AlertTypeId"] = entity.AlertTypeId;
                dr["Tag"] = entity.Tag;
                dr["Sign"] = entity.Sign;
                dr["RTUId"] = entity.RTUId;
                dt.Rows.Add(dr);
            }

            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName = "AlertData";
                        bulkCopy.ColumnMappings.Add("MeasureId", "MeasureId");
                        bulkCopy.ColumnMappings.Add("CollStartTime", "CollStartTime");
                        bulkCopy.ColumnMappings.Add("StartNum", "StartNum");
                        bulkCopy.ColumnMappings.Add("CollEndTime", "CollEndTime");
                        bulkCopy.ColumnMappings.Add("EndNum", "EndNum");
                        bulkCopy.ColumnMappings.Add("Ratio", "Ratio");
                        bulkCopy.ColumnMappings.Add("AlertTypeId", "AlertTypeId");
                        bulkCopy.ColumnMappings.Add("Tag", "Tag");
                        bulkCopy.ColumnMappings.Add("Sign", "Sign");
                        bulkCopy.ColumnMappings.Add("RTUId", "RTUId");

                        conn.Open();

                        bulkCopy.WriteToServer(dt);
                    }
                    //执行统计newdata数据逻辑
                    foreach (AlertData entity in entities)
                    {
                        SqlParameter[] para = this.CreateSqlParametersForNewData(entity);
                        this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_newdata", para);

                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error("DataBase Error Message: ", e);
                result = false;
            }

            return result;
        }


        /// <summary>
        /// 插入alertdetail 数据，望alertdata表和alertdetail表分别插入主从数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool InsertAlertDetail(AlertDataDetail[] datas, out List<AlertDataDetail> leftdatas)
        { 
            bool result=true;

            leftdatas = new List<AlertDataDetail>();
            try
            {
                using (SqlConnection conn = this.AdoHelper.GetConnection(this.ConnectionString) as SqlConnection)
                {
                    foreach (AlertDataDetail data in datas)
                    {

                        try
                        {
                            SqlParameter[] para = this.CreateSqlParametersForAlertDetail(data);
                            if (para != null)
                            {
                                this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_alertdetail", para) ;
                                //执行统计newdata数据逻辑
                                SqlParameter[] para2 = this.CreateSqlParametersForNewData(data);
                                this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_newdata", para2);
                            }
                        }
                        catch(Exception e)
                        {
                            leftdatas.Add(data);
                            _logger.Error(e.Message, e);
                            result = false;
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                _logger.Error(e.Message, e);
            }
            return result;
        }

        public bool InsertAlertDetail(AlertDataDetail[] datas)
        {
            bool result = true;

       
            try
            {
                using (SqlConnection conn = this.AdoHelper.GetConnection(this.ConnectionString) as SqlConnection)
                {
                    foreach (AlertDataDetail data in datas)
                    {

                        try
                        {
                            SqlParameter[] para = this.CreateSqlParametersForAlertDetail(data);
                            if (para != null)
                            {
                                 this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_alertdetail", para);
                                //执行统计newdata数据逻辑
                                SqlParameter[] para2 = this.CreateSqlParametersForNewData(data);
                                this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_newdata", para2);
                            }
                        }
                        catch (Exception e)
                        {
                           
                            _logger.Error(e.Message, e);
                            result = false;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                _logger.Error(e.Message, e);
            }
            return result;
        }
        /// <summary>
        /// 插入后门信息
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool InsertSecretDoor(SecreatDoor[] datas)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = this.AdoHelper.GetConnection(this.ConnectionString) as SqlConnection)
                {
                    foreach (SecreatDoor data in datas)
                    {
                       
                            string sql=CreateInsertSecretDoorSql(data);
                            if (!string.IsNullOrEmpty( sql))
                            {
                                result = this.AdoHelper.ExecuteNonQuery(conn, CommandType.Text, sql)>0;
                                
                            }
                        
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                _logger.Error(e.Message, e);
            }
            return result;
        }
        #endregion 

        #region ICheckConnection Members

        /// <summary>
        /// 检测数据库连接是否通畅
        /// </summary>
        /// <returns>bool值</returns>
        public bool CheckConnection()
        {
            IDbConnection conn = null;
            try
            {
                conn = this.AdoHelper.GetConnection(this.ConnectionString);
                conn.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region private Methods

        #region Create SqlParameters
        private string CreateInsertSecretDoorSql(SecreatDoor data)
        {
            string result = string.Empty;
            try
            {
                string sql = "insert into SecretDoorInfo (rtuid,time,signal,ConncetSpend,ConnectSuccessful,ConnectStep,ErrCode,inserttime) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
                result=string.Format(sql,data.RtuID,data.Time.ToString("yyyy-MM-dd hh:mm:ss"),data.Signal.ToString(),data.ConncetSpend,data.ConnectSuccessful?1:0,data.ConnectStep,data.ErrCode,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        private SqlParameter[] CreateSqlParameters(AlertData entity)
        {
            SqlParameter[] para = new SqlParameter[10]
            {
                new SqlParameter("@MeasureId",entity.MeasureId), 
                new SqlParameter("@CollStartTime",entity.CollStartTime), 
                new SqlParameter("@StartNum",entity.StartNum), 
                new SqlParameter("@CollEndTime",entity.CollEndTime==DateTime.MinValue ? DateTime.Now:entity.CollEndTime), 
                new SqlParameter("@EndNum",entity.EndNum),                 
                new SqlParameter("@Ratio",entity.Ratio), 
                new SqlParameter("@AlertTypeId",entity.AlertTypeId), 
                new SqlParameter("@Tag",entity.Tag), 
                new SqlParameter("@Sign",entity.Sign),
                new SqlParameter("@RTUId",entity.RTUId)
            };
            return para;
        }
        private SqlParameter[] CreateSqlParametersForNewData(AlertData entity)
        {
            SqlParameter[] para = new SqlParameter[5]
            {
                new SqlParameter("@MeasureId",entity.MeasureId),
                new SqlParameter("@RtuId",entity.RTUId),
                new SqlParameter("@CollDatetime",entity.CollStartTime), 
                new SqlParameter("@CollNum",entity.StartNum), 
                new SqlParameter("@AlertTypeId",entity.AlertTypeId)
            };
            if (entity.AlertTypeId == 3)//突变报警
            {
                para[2].SqlValue = entity.CollEndTime;
                para[3].SqlValue = entity.EndNum;
            }
            
            return para;
        }

        private SqlParameter[] CreateSqlParametersForNewData(AlertDataDetail entity)
        {
            SqlParameter[] para = new SqlParameter[5]
            {
                new SqlParameter("@MeasureId",entity.MeasureId),
                new SqlParameter("@RtuId",entity.RTUId),
                new SqlParameter("@CollDatetime",GetStartFromStr(entity.CollTimes)), 
                new SqlParameter("@CollNum",GetStartFromStr(entity.CollNums)), 
                new SqlParameter("@AlertTypeId",entity.AlertTypeId)
            };
           

            return para;
        }

        private string GetStartFromStr(string times)
        {
            string result="";
            string[] strarr = times.Split(',');
            if (strarr.Length > 0)
            {
                return strarr[0];
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 创建parameters for alertdetail,参数群为:measureid,rtuid,colldatetimes,collnums,alerttypeid
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private SqlParameter[] CreateSqlParametersForAlertDetail(AlertDataDetail data)
        {            
            SqlParameter[] para = new SqlParameter[5]
            {
                new SqlParameter("@MeasureId",data.MeasureId),
                new SqlParameter("@RtuId",data.RTUId),
                new SqlParameter("@CollDatetimes",data.CollTimes), 
                new SqlParameter("@CollNums",data.CollNums), 
                new SqlParameter("@AlertTypeId",data.AlertTypeId)
                
            };

            return para;
        }
       
        
        
        #endregion

    }
}
