using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using DataEntity;
using DataAccess.Interfaces;
using DataAccess.Helpers;

namespace DataAccess.Sqlite
{
    public class SqliteMeasureDataRepository : SqliteRepositoryBase, IMeasureDataRepository, IMeasureDataBackupRepository
    {

        #region Constructors
        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqliteMeasureDataRepository(string connectionString) : base(connectionString) { }

        #endregion

        #region IMeasureDataRepository Members

        /// <summary>
        /// 单个检测量保存
        /// </summary>
        /// <param name="entity">检测量类</param>
        /// <returns>bool型</returns>
        public bool Insert(MeasureData entity)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = this.CreateMeasureSqliteInsertSql(entity);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 检测量批量保存
        /// </summary>
        /// <param name="entities">检测量列表</param>
        /// <returns>bool 型</returns>
        public bool BulkInsert(IEnumerable<MeasureData> entities)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (MeasureData entity in entities)
                    {
                        cmd.CommandText = this.CreateMeasureSqliteInsertSql(entity);
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    cmd.Dispose();
                    trans.Dispose();
                }

            }
        }

        #endregion

        #region IMeasureDataBackupRepository Members
        /// <summary>
        /// 获取所有检测量数据
        /// </summary>
        /// <returns>检测量数据集合</returns>
        public IEnumerable<MeasureData> LoadAll()
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultMeasureDataFields + ",RtuId ", SQLItems.DefaultMeasureDataTableName, null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    List<MeasureData> list = new List<MeasureData>();
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateMeasureDataEntity(reader));
                    }
                    return list;
                }        
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 构建检测量sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">检测量实体</param>
        /// modified: wendy 2010-4-12 本地文件需保存RtuId，方便异常数据恢复。
        /// <returns>执行语句</returns>
        private string CreateMeasureSqliteInsertSql(MeasureData entity)
        {
            string sql = @"INSERT INTO {5} ({6},RtuId) 
                            VALUES({0},'{1}',{2},{3},{4},'{7}')";
            return string.Format(sql, entity.MeasureId,entity.CollDatetime.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), entity.CollNum,entity.Tag, entity.Sign,
                SQLItems.DefaultMeasureDataTableName, SQLItems.DefaultMeasureDataFields,entity.RTUId);
        }
        #endregion
    }
}
