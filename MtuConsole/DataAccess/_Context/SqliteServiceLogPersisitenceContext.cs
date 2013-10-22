using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataAccess.Helpers;
using System.IO;
using System.Data.SQLite;

namespace DataAccess
{
    public class SqliteMtuLogPersisitenceContext : IMtuLogPersistenceContext
    {

        public string FilePath { get; private set; }                    //文件路径
                            

        /// <summary>
        /// 检测量sqlite持久化参数构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="splitUnit">拆分单元</param>
        /// <param name="freeSpace">磁盘可用空间(M)</param>
        public SqliteMtuLogPersisitenceContext(string filePath)
        {
            this.FilePath = FileHelper.ReviseFilePath(filePath) + FolderNames.LogDBFolderName + "\\";
           
        }

        #region IMtuLogRepsitoryContext Members

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        public IMtuLogRepsitory GetRepository()
        {
           
            //this.EnsureSqliteDbFileExist(this.FilePath);

            string connectionString = SqliteConnectionHelper.BuildConnectionString(FilePath + "Log.db3");

            return new Sqlite.SqliteMtuLogRepository(connectionString);
        }

        #endregion

//        /// <summary>
//        /// 创建采集数据sqlite文件
//        /// </summary>
//        /// <param name="fullFileName">文件全名</param>
//        private void EnsureSqliteDbFileExist(string fullFileName)
//        {
//            if (!File.Exists(fullFileName))
//            {
//                FileDiskHelper.EnsureDiskSapceEnough(fullFileName, this.FreeSpace);

//                SQLiteConnection.CreateFile(fullFileName);
//                string sqlCreateTB = @"CREATE TABLE [CollectionData](
//	                                    [NoteId] [integer] primary key,
//                                        [RtuID] [varchar](50) null,
//	                                    [PhoneNumber] [varchar](16) NULL,
//	                                    [MessageCenterNo] [varchar](50) NULL,
//                                        [RemoteIp] [varchar](50) NULL,
//	                                    [SendTime] [datetime] ,	
//	                                    [MessageContent] [varchar](500) NULL,
//	                                    [status] [integer] ,
//	                                    [BccResult] [integer] , 
//                                        [FrameMark] [float](18,0),
//	                                    [TransformMark] [integer] , 
//	                                    [InsertTime] [datetime] NULL
//                                    ) ";


//                string sqlCreateSendTB = @"CREATE TABLE [SendData](
//	                                    [NoteId] [integer] primary key,
//                                        [RtuID] [varchar](50) null,
//                                        [RemoteIp] [varchar](50) NULL,
//	                                    [SendTime] [datetime] ,	
//	                                    [MessageContent] [varchar](500) NULL,
//	                                    [status] [integer] ,
//	                                    [TransformMark] [integer] , 
//	                                    [InsertTime] [datetime] NULL
//                                    ) ";
//                string connectionString = SqliteConnectionHelper.BuildConnectionString(fullFileName);

//                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
//                {
//                    conn.Open();
//                    SQLiteCommand cmd = new SQLiteCommand(sqlCreateTB, conn);
//                    cmd.ExecuteNonQuery();
//                    cmd.CommandText = sqlCreateSendTB;
//                    // cmd = new SQLiteCommand(sqlCreateSendTB, conn);
//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }
    }
}
