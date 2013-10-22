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
    public class SqlServerMeasureDataExportRepository : SqlServerRepositoryBase, IMeasureDataRepository, ICheckConnection
    {
        private MtuLog _logger = null;

        #region Constructors

        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerMeasureDataExportRepository(string connectionString)
            : base(connectionString) 
        {
            _logger = new MtuLog();
        }

        #endregion

        #region IMeasureDataRepository Members

        /// <summary>
        /// 单个检测量保存至数据库
        /// </summary>
        /// <param name="entity">检测量</param>
        /// <returns>是否保存成功</returns>
        public bool Insert(MeasureData entity)
        {            
            bool result = true;
            try
            {
                using (SqlConnection conn = this.AdoHelper.GetConnection(this.ConnectionString) as SqlConnection)
                {
                    if (entity.CollNum > 9E15m)
                    {
                        return true;   //超大数据直接抛弃，不存
                    }
                    SqlParameter[] para = this.CreateSqlParameters(entity);
                    this.AdoHelper.ExecuteNonQuery(conn, "usp_ExportDataLogRealData", para);
                }
            }
            catch (Exception e)
            {
                _logger.Error("MeasureDataExport Error Message: ", e);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 批量检测量保存至数据库
        /// </summary>
        /// <param name="entities">检测量列表</param>
        /// <returns>是否保存成功</returns>
        public bool BulkInsert(IEnumerable<MeasureData> entities)
        {
            bool result = true;
            try
            {
                _logger.Debug("bulk insert to " + base.ConnectionString);
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                {
                    foreach (MeasureData entity in entities)
                    {
                        if (Math.Abs( entity.CollNum) > 9E15m)
                        {
                            continue;
                        }
                        SqlParameter[] para = this.CreateSqlParameters(entity);
                        _logger.Debug("mark dataaccess gogo");
                        this.AdoHelper.ExecuteNonQuery(conn, "usp_ExportDataLogRealData", para);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.Error("MeasureDataExport Error Message: ", e);
                result = false;
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
            catch (Exception ex)
            {
                _logger.Error("CheckConnection, 连接不通畅", ex);
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
        private SqlParameter[] CreateSqlParameters(MeasureData entity)
        {
            SqlParameter[] para = new SqlParameter[4]
            {
                new SqlParameter("@RtuId",entity.RTUId),
                new SqlParameter("@MeasureId",entity.MeasureId),
                new SqlParameter("@CollDateTime",entity.CollDatetime), 
                new SqlParameter("@CollNum",entity.CollNum)
            };
            return para;
        }
        #endregion

        #endregion

    }
}
