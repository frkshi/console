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
    public class SqlServerAlertDataExportRepository : SqlServerRepositoryBase, IAlertDataRepository, ICheckConnection
    {
        private MtuLog _logger = null;

        #region Constructors
        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerAlertDataExportRepository(string connectionString)
            : base(connectionString)
        {
            _logger = new MtuLog();
        }

        #endregion

        #region IMeasureDataRepository Members
        public bool InsertSecretDoor(SecreatDoor[] datas)
        {
            return false;
        }
        /// <summary>
        /// 单个报警量保存至数据库
        /// </summary>
        /// <param name="entity">报警量</param>
        /// <returns>是否保存成功</returns>
        public bool Insert(AlertData entity)
        {
            bool result = true;
            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                {
                    SqlParameter[] para = this.CreateSqlParameters(entity);
                    this.AdoHelper.ExecuteNonQuery(conn, "usp_ExportDataLogAlarmData", para);
                }
            }
            catch (Exception e)
            {
                _logger.Error("AlertDataExport Error Message: ", e);
                result = false;
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
            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                {                    
                    foreach (AlertData entity in entities)
                    {
                        SqlParameter[] para = this.CreateSqlParameters(entity);
                        this.AdoHelper.ExecuteNonQuery(conn, "usp_ExportDataLogAlarmData", para);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("AlertDataExport Error Message: ", e);
                result = false;
            }

            return result;
        }

         /// <summary>
        /// 插入alertdetail 数据，望alertdata表和alertdetail表分别插入主从数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool InsertAlertDetail(AlertDataDetail[] entities)
        {

            bool result = true;
            try
            {
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                {
                    foreach (AlertDataDetail entity in entities)
                    {
                        SqlParameter[] para = this.CreateSqlParametersForAlertDetail(entity);
                        this.AdoHelper.ExecuteNonQuery(conn, "usp_ExportDataLogAlarmData", para);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("AlertDataExport Error Message: ", e);
                result = false;
            }

            return result;
            
        }

        /// <summary>
        /// 留空
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="leftdatas"></param>
        /// <returns></returns>
        public bool InsertAlertDetail(AlertDataDetail[] datas, out List<AlertDataDetail> leftdatas)
        {
            leftdatas = new List<AlertDataDetail>();
            return false;
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
        private SqlParameter[] CreateSqlParameters(AlertData entity)
        {
            SqlParameter[] para = new SqlParameter[7]
            {
                new SqlParameter("@RtuId",entity.RTUId),
                new SqlParameter("@MeasureId",entity.MeasureId), 
                new SqlParameter("@CollStartTime",entity.CollStartTime), 
                new SqlParameter("@StartNum",entity.StartNum), 
                new SqlParameter("@CollEndTime",entity.CollEndTime==DateTime.MinValue ? DateTime.Now:entity.CollEndTime), 
                new SqlParameter("@EndNum",entity.EndNum),  
                new SqlParameter("@AlertTypeId",entity.AlertTypeId)
            };
            return para;
        }
        /// <summary>
        /// 创建parameters for alertdetail,参数群为:measureid,rtuid,colldatetimes,collnums,alerttypeid
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private SqlParameter[] CreateSqlParametersForAlertDetail(AlertDataDetail entity)
        {
            SqlParameter[] para = new SqlParameter[7]
            {
               new SqlParameter("@RtuId",entity.RTUId),
                new SqlParameter("@MeasureId",entity.MeasureId), 
                new SqlParameter("@CollStartTime",GetStartFromStr(entity.CollTimes)), 
                new SqlParameter("@StartNum",GetStartFromStr(entity.CollNums)), 
                new SqlParameter("@CollEndTime",GetStartFromStr(entity.CollTimes)), 
                new SqlParameter("@EndNum",GetStartFromStr(entity.CollNums)),  
                new SqlParameter("@AlertTypeId",entity.AlertTypeId)
                
            };

            return para;
        }

        private string GetStartFromStr(string times)
        {
            string result = "";
            string[] strarr = times.Split(',');
            if (strarr.Length > 0)
            {
                return strarr[0];
            }

            return result;
        }
        #endregion

        #endregion

    }
}
