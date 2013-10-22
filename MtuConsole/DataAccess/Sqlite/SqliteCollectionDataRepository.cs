using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;
using DataAccess.Interfaces;
using System.Data.SQLite;
using DataAccess.Helpers;


namespace DataAccess.Sqlite
{
    public class SqliteCollectionDataRepository : SqliteRepositoryBase, ICollectionDataRepository, IRedoCollectionData
    {
        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqliteCollectionDataRepository(string connectionString) : base(connectionString) { }

        #region ICollectionDataBackup Members

        /// <summary>
        /// 单个采集数据保存
        /// </summary>
        /// <param name="entity">采集数据类</param>
        /// <returns>bool型</returns>
        public bool Insert(CollectionData entity) {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = this.CreateCDSqliteInsertSql(entity);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 采集数据保存
        /// </summary>
        /// <param name="entities">采集数据组</param>
        /// <returns>bool型</returns>
        public bool BulkInsert(IEnumerable<CollectionData> entities)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (CollectionData collectionEntity in entities)
                    {
                        cmd.CommandText = this.CreateCDSqliteInsertSql(collectionEntity);
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
        /// 发送数据保存
        /// </summary>
        /// <param name="entities">采集数据组</param>
        /// <returns>bool型</returns>
        public bool BulkInsert(IEnumerable<SendData> entities)
        {
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);

                conn.Open();

                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (SendData collectionEntity in entities)
                    {
                        cmd.CommandText = this.CreateSDSqlInsertSql(collectionEntity);
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

        #region 重解包逻辑
        /// <summary>
        /// 获取源码文件信息
        /// </summary>
        /// <returns></returns>
        public List<CollectionFile> LoadFileInfo()
        {
            List<string> fileNames = FileHelper.GetFileName(this.ConnectionString, UtilityParameters.FileMonthFilter);
            List<CollectionFile> listReturn = new List<CollectionFile>();
            foreach (string filename in fileNames)
            {
                CollectionFile cf = new CollectionFile();
                cf.FileName = filename;
                using (SQLiteConnection conn = new SQLiteConnection(
                    SqliteConnectionHelper.BuildCustomerConnString(filename,this.ConnectionString)))
                {
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandText = "select status,count(1) as c from collectiondata group by status";                  
                    conn.Open();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["status"] as string == "0" && !reader.IsDBNull(1))
                                cf.UnDecodeNum = reader.GetInt32(1);
                            else if (reader["status"] as string == "1" && !reader.IsDBNull(1))
                                cf.DecodeNum = reader.GetInt32(1);
                        }
                    }        
                }
                listReturn.Add(cf);
            }
            return listReturn;
        }

        /// <summary>
        /// 获取某文件中未解码的记录
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>未解码的记录</returns>
        public List<CollectionData> LoadUnDecodeData(string fileName)
        {
            List<CollectionData> listCD = new List<CollectionData>();
            using (SQLiteConnection conn = new SQLiteConnection(
                  SqliteConnectionHelper.BuildCustomerConnString(fileName, this.ConnectionString)))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = SQLItems.CreateSelectSql(SQLItems.DefaultCollectionDataFields, SQLItems.DefaultCollectionDataTableName, "status<>0", string.Empty);
                conn.Open();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listCD.Add(EntityAssembler.CreateCollectionDataEntity(reader));
                    }
                }
            }
            return listCD;
        }


        public List<SourceCodeData> LoadSourceCode(string[] rtus, DateTime begintime, DateTime endtime)
        {
            List<SourceCodeData> result = new List<SourceCodeData>();
            List<string> filenames= FileHelper.GetFileNameByDateRange(begintime, endtime);
            string path = FileHelper.GetFilePathFromConnectStr(this.ConnectionString);
            foreach (string filename in filenames)
            {
                //Console.WriteLine("filename=" + filename);
                //Console.WriteLine("path=" + path );
                //Console.WriteLine("file=" +path + filename + ".db3");
                if (System.IO.File.Exists(path + filename + ".db3"))
                {
                 result.AddRange(LoadSourceCodeByFilename( filename,rtus,begintime,endtime));
                }
            }

            //Console.WriteLine("loadsourecode count=" + result.Count.ToString());
            return result;
        }

        /// <summary>
        /// 获取指定文件，指定时间，指定rtus的上下行数据
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private List<SourceCodeData> LoadSourceCodeByFilename(string filename,string[] rtus,DateTime begintime,DateTime endtime)
        {

            
            List<SourceCodeData> result = new List<SourceCodeData>();
            
            using (SQLiteConnection conn =  new SQLiteConnection(
                    SqliteConnectionHelper.BuildCustomerConnString(filename,this.ConnectionString)))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string rtustr =EntityAssembler.ConvertToSqlInnerstr(rtus);
                string sqlstr;
                if (rtustr != "")
                {

                    sqlstr = @"select * from
(
select rtuid,remoteip,sendtime,messagecontent,'receive' as direction from collectiondata
union
select rtuid,remoteip,sendtime,messagecontent,'send' as direction from senddata) where 
sendtime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endtime.ToString("yyyy-MM-dd HH:mm:ss") + "' and rtuid in (" + rtustr + ") order by sendtime desc";
                }
                else
                {
                    sqlstr = @"select * from
(
select rtuid,remoteip,sendtime,messagecontent,'receive' as direction from collectiondata
union
select rtuid,remoteip,sendtime,messagecontent,'send' as direction from senddata) where 
sendtime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endtime.ToString("yyyy-MM-dd HH:mm:ss") + "'  order by sendtime desc";
                }
                cmd.CommandText = sqlstr;//SQLItems.CreateSelectSql(SQLItems.DefaultCollectionDataFields, SQLItems.DefaultCollectionDataTableName, "status<>0", string.Empty);
                conn.Open();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(EntityAssembler.CreateSourceCodeData(reader));
                    }
                }
            }
            return result;


        }
        
        /// <summary>
        /// 更新某文件中的记录
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="noteId">编号</param>
        /// <param name="status">状态</param>
        /// <returns>Bool型</returns>
        public bool Update(string fileName, string noteId, string status)
        {
            using (SQLiteConnection conn = new SQLiteConnection(
                SqliteConnectionHelper.BuildCustomerConnString(fileName, this.ConnectionString)))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "UPDATE CollectionData SET status = " + status + " WHERE noteid=" + noteId;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 构建采集量sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">采集量实体</param>
        /// <returns>执行语句</returns>
        private string CreateCDSqliteInsertSql(CollectionData entity)
        {
            string sql = @"INSERT INTO CollectionData (PhoneNumber,MessageCenterNo,SendTime,MessageContent,Status,BccResult,FrameMark,TransformMark,remoteip,rtuid) 
                            VALUES('{0}','{1}','{2}','{3}',{4},{5},{6},{7},'{8}','{9}')";
            return string.Format(sql, entity.PhoneNumber, entity.MessageCenterNo, (entity.SendTime == DateTime.MinValue ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : entity.SendTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")), entity.MessageContent, entity.Status, entity.BccResult, entity.FrameMark, entity.TransformMark,entity.RemoteIP,entity.RtuID);
        }

        /// <summary>
        /// 构建senddata sqlite保存的执行语句
        /// </summary>
        /// <param name="entity">senddata实体</param>
        /// <returns>执行语句</returns>
        private string CreateSDSqlInsertSql(SendData entity)
        {

            string sql = @"INSERT INTO SendData (SendTime,MessageContent,Status,TransformMark,remoteip,rtuid) 
                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}')";
            sql = string.Format(sql, (entity.SendTime == DateTime.MinValue ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff") : entity.SendTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")), entity.MessageContent, entity.Status, entity.TransformMark,entity.RemoteIP,entity.RtuID);
            return sql;
        }
        #endregion

       
    }
}
