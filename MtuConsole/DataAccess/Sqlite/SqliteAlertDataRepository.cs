using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using DataEntity;
using DataAccess.Interfaces;
using DataAccess.Helpers;

namespace DataAccess.Sqlite
{
    public class SqliteAlertDataRepository : SqliteRepositoryBase, IAlertDataRepository, IAlertDataBackupRepository
    {

        #region Constructors

        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqliteAlertDataRepository(string connectionString) : base(connectionString) { }

        #endregion

        #region IAlertDataRepository Members

        public bool InsertSecretDoor(SecreatDoor[] datas)
        {
            return false;
        }
        /// <summary>
        /// 单个报警量保存
        /// </summary>
        /// <param name="entity">报警量类</param>
        /// <returns>bool型</returns>
        public bool Insert(AlertData entity)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = this.CreateAlertSqliteInsertSql(entity);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 报警量批量保存
        /// </summary>
        /// <param name="entities">报警量列表</param>
        /// <returns>bool 型</returns>
        public bool BulkInsert(IEnumerable<AlertData> entities)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (AlertData entity in entities)
                    {
                        cmd.CommandText = this.CreateAlertSqliteInsertSql(entity);
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
        /// <summary>
        /// 插入alertdetail 数据，往alertdetail一表插入主从数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool InsertAlertDetail(AlertDataDetail[] datas)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (AlertDataDetail data in datas)
                    {
                        string sql = this.CreateAlertDetailSqliteInsertSql(data);
                        cmd.CommandText = sql;
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

        #region IAlertDataBackupRepository Members

        /// <summary>
        /// 获取所有报警数据
        /// </summary>
        /// <returns>报警数据集合</returns>
        public IEnumerable<AlertData> LoadAll()
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultAlertDataFields, SQLItems.DefaultAlertDataTableName, null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    List<AlertData> list = new List<AlertData>();
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateAlertDataEntity(reader));
                    }
                    return list;
                }
            }
        }

        public IEnumerable<AlertDataDetail> LoadAllDetail()
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultAlertDataDetailFields, "AlertDataDetail", null, null);

                conn.Open();

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    List<AlertDataDetail> list = new List<AlertDataDetail>();
                    while (reader.Read())
                    {
                        list.Add(EntityAssembler.CreateAlertDataDetailEntity(reader));
                    }
                    return list;
                }
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 构建报警量sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">报警量实体</param>
        /// <returns>执行语句</returns>
        private string CreateAlertSqliteInsertSql(AlertData entity)
        {
            string sql = @"INSERT INTO {10} ({11}) 
                            VALUES({0},'{1}',{2},'{3}',{4},{5},{6},{7},{8},'{9}')";
            return string.Format(sql, entity.MeasureId,
                entity.CollStartTime == DateTime.MinValue ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : entity.CollStartTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), 
                entity.StartNum, 
                entity.CollEndTime == DateTime.MinValue ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : entity.CollEndTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), 
                entity.EndNum,
                entity.Ratio, entity.AlertTypeId, entity.Tag, entity.Sign, entity.RTUId,
                SQLItems.DefaultAlertDataTableName, SQLItems.DefaultAlertDataFields);
        }

        private string CreateAlertDetailSqliteInsertSql(AlertDataDetail data)
        {
            string result = "";
         
            // sql语句组成
            result = "insert into alertdatadetail (rtuid,measureid,CollDatetimes,collnums,alerttypeid,inserttime) values ('{0}',{1},'{2}','{3}',{4},'{5}')";
            result = string.Format(result, data.RTUId, data.MeasureId, data.CollTimes,data.CollNums,data.AlertTypeId, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            return result;
        }
       
        #endregion
    }
}
