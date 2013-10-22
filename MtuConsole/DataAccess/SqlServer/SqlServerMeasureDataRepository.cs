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
    public class SqlServerMeasureDataRepository : SqlServerRepositoryBase, IMeasureDataRepository, ICheckConnection
    {
        private MtuLog _logger = null;

        private static readonly object _loadTableNameLock = new object();

        #region Constructors

        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerMeasureDataRepository(string connectionString)
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
            string tableName = this.GetTableName(entity);
            bool result = true;
            using (SqlConnection conn = this.AdoHelper.GetConnection(this.ConnectionString) as SqlConnection)
            {
                string sql = @"INSERT INTO " + tableName + @" (" + SQLItems.DefaultMeasureDataFields + @") 
                            VALUES(@MeasureId,@CollDatetime,@CollNum,@Tag,@Sign)";
                SqlParameter[] para = this.CreateSqlParameters(entity);

                conn.Open();
                this.EnsureTableExists(conn, tableName);
                result = this.AdoHelper.ExecuteNonQuery(conn, CommandType.Text, sql, para) > 0;
                //执行统计newdata数据逻辑
                SqlParameter[] para2 = this.CreateSqlParametersForNewData(entity);
                this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_newdata", para2);
                _logger.Debug("save to sql success : value=" + entity.CollNum.ToString());
            }
            return result;
        }

        private static DataTable _BulkTable = null;
        private static DataTable BuilkTableForBulk()
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("MeasureId",typeof(int));
            //dt.Columns.Add("CollDatetime",typeof(DateTime)); 
            //dt.Columns.Add("CollNum",typeof(decimal)); 
            //dt.Columns.Add("Tag", typeof(int));
            //dt.Columns.Add("Sign", typeof(byte));             
            if (_BulkTable != null) return _BulkTable;
            _BulkTable = new DataTable();
            _BulkTable.Columns.Add("MeasureId", typeof(int));
            _BulkTable.Columns.Add("CollDatetime", typeof(DateTime));
            _BulkTable.Columns.Add("CollNum", typeof(decimal));
            _BulkTable.Columns.Add("Tag", typeof(int));
            _BulkTable.Columns.Add("Sign", typeof(byte));
            return _BulkTable;
        }

        private SqlConnection _SqlConnection = null;
        private SqlConnection BuildSqlConnection()
        {
            if (_SqlConnection == null)
            {
                this._SqlConnection = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString);
                this._SqlConnection.Open();
            }
            return this._SqlConnection;
        }


        /// <summary>
        /// 批量检测量保存至数据库
        /// </summary>
        /// <param name="entities">检测量列表</param>
        /// <returns>是否保存成功</returns>
        public bool BulkInsert(IEnumerable<MeasureData> entities)
        {
            bool result = true;

            DataTable dt = SqlServerMeasureDataRepository.BuilkTableForBulk();
            try
            {
                //SqlConnection conn = this.BuildSqlConnection();
                using (SqlConnection conn = (SqlConnection)base.AdoHelper.GetConnection(base.ConnectionString))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.ColumnMappings.Add("MeasureId", "MeasureId");
                        bulkCopy.ColumnMappings.Add("CollDatetime", "CollDatetime");
                        bulkCopy.ColumnMappings.Add("CollNum", "CollNum");
                        bulkCopy.ColumnMappings.Add("Tag", "Tag");
                        bulkCopy.ColumnMappings.Add("Sign", "Sign");

                        conn.Open();

                        dt.Clear();
                        MeasureData entityOfTable = null;
                        foreach (MeasureData entity in entities)
                        {
                            if (Math.Abs( entity.CollNum) > 9E15m) // 过大数据不存
                            {
                                continue;
                            }
                            DataRow dr = dt.NewRow();
                            dr["MeasureId"] = entity.MeasureId;
                            dr["CollDatetime"] = entity.CollDatetime;
                            dr["CollNum"] = Math.Round(entity.CollNum,4);
                            dr["Tag"] = entity.Tag;
                            dr["Sign"] = entity.Sign;
                            dt.Rows.Add(dr);
                            entityOfTable = entity;
                        }
                        string tableName = this.GetTableName(entityOfTable);
                        this.EnsureTableExists(conn, tableName); //确保该表存在
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(dt);
                        _logger.Debug("save to sql success : BulkInsert ");

                        

                        SqlParameter parameter = new SqlParameter();
                        parameter.SqlDbType = SqlDbType.VarChar;
                        parameter.Direction = ParameterDirection.Input;
                        parameter.ParameterName = "TableName";
                        parameter.Size = 20;
                        parameter.SqlValue = tableName;

                        SqlParameter[] parameters = new SqlParameter[] { parameter };
                        this.AdoHelper.ExecuteNonQuery(conn, "SP_MTU_UPDATE_NEWDATA", parameters);

                        CommonMemory.AddCount(dt.Rows.Count);

                        Console.WriteLine("[" +DateTime.Now.ToString() +"] saved measure data : " + CommonMemory.Count);
                    }
                    //执行统计newdata数据逻辑
                    //foreach (MeasureData entity in entities)
                    //{
                    //    SqlParameter[] para = this.CreateSqlParametersForNewData(entity);
                    //    this.AdoHelper.ExecuteNonQuery(conn, "sp_mtu_opr_newdata", para);
                    //}

                }
            }
            catch (Exception e)
            {
                _logger.Error("DataBase Error Message: ", e);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 获取检测量存储目标表
        /// </summary>
        /// <param name="entity">检测量实体类</param>
        /// <returns>目标表名称</returns>
        public string GetTableName(MeasureData entity)
        {
            string tableNamePrefix = this.GetTableNamePrefix(entity.RTUId);
            return tableNamePrefix + "_" + entity.CollDatetime.ToString("yy");
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

        /// <summary>
        /// 从数据库获取终端目标表的前缀
        /// </summary>
        private string LoadTableNamePrefix(string rtuId)
        {
            string result = null;

            string sql = string.Format("SELECT TableName FROM DataLogConfig..RtuSetting WHERE RtuId = '{0}'", rtuId);
            object obj = this.AdoHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql);

            result = obj as string;

            return result;
        }

        private string GetTableNamePrefix(string rtuId)
        {
            string tableName = CacheContainer.GetMeasureTableName(rtuId);

            if (string.IsNullOrEmpty(tableName))
            {
                lock (_loadTableNameLock)
                {
                    //double check: read from cache, and check again!
                    tableName = CacheContainer.GetMeasureTableName(rtuId);

                    if (string.IsNullOrEmpty(tableName))
                    {
                        tableName = this.LoadTableNamePrefix(rtuId);
                        CacheContainer.SaveMeasureTableName(rtuId, tableName);
                    }
                }
            }

            return tableName;
        }

        private void EnsureTableExists(SqlConnection conn, string tableName)
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sysobjects  WHERE [name] ='{0}' AND xtype='U')  
                           BEGIN
	                            CREATE TABLE [dbo].[{1}](
	                            [ID] [int] IDENTITY(1,1) NOT NULL,
	                            [MeasureId] [int] NULL,
	                            [CollDatetime] [datetime] NULL,
	                            [CollNum] [numeric](22, 4) NULL,
	                            [Tag] [int] NULL,
	                            [Sign] [tinyint] NULL Default (1),
	                            [InsertTime] [datetime] NOT NULL DEFAULT (getdate()),
	                            [UpdateTime] [datetime] NOT NULL DEFAULT (getdate())
                            ) ON [PRIMARY]
                           END";
            this.AdoHelper.ExecuteNonQuery(conn, CommandType.Text, string.Format(sql, tableName, tableName));

        }

        #region Create SqlParameters
        private SqlParameter[] CreateSqlParameters(MeasureData entity)
        {
            SqlParameter[] para = new SqlParameter[5]
            {
                new SqlParameter("@MeasureId",entity.MeasureId), 
                new SqlParameter("@CollDatetime",entity.CollDatetime), 
                new SqlParameter("@CollNum",entity.CollNum), 
                new SqlParameter("@Tag",entity.Tag), 
                new SqlParameter("@Sign",entity.Sign)
            };
            return para;
        }
        private SqlParameter[] CreateSqlParametersForNewData(MeasureData entity)
        {
            SqlParameter[] para = new SqlParameter[5]
            {
                new SqlParameter("@MeasureId",entity.MeasureId),
                new SqlParameter("@RtuId",entity.RTUId),
                new SqlParameter("@CollDatetime",entity.CollDatetime), 
                new SqlParameter("@CollNum",entity.CollNum), 
                new SqlParameter("@AlertTypeId",int.Parse("0"))
            };
            return para;
        }
        #endregion

        #endregion

    }
}
