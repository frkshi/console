using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataAccess.Helpers;
using System.Data.SQLite;
using DataEntity;
using System.IO;

namespace DataAccess
{
    public class SqliteMeasureDataPersistenceContext : IMeasureDataPersistenceContext
    {
        public string FilePath { get; set; }                            //文件路径
        public FileSplitUnit FileSplitUnit { get; private set; }        //拆分单元
        public int FreeSpace { get; set; }                              //磁盘可用空间(M)

        /// <summary>
        /// 检测量sqlite持久化参数构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="splitUnit">拆分单元</param>
        /// <param name="freeSpace">磁盘可用空间(M)</param>
        public SqliteMeasureDataPersistenceContext(string filePath, FileSplitUnit splitUnit, int freeSpace)
        {
            this.FilePath = FileHelper.ReviseFilePath(filePath);
            this.FileSplitUnit = splitUnit;
            this.FreeSpace = freeSpace;
        }


        #region IMeasureDataPersistenceContext Members

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        public IMeasureDataRepository GetRepository()
        {
            string connectionString = this.GetSqliteDbFile(this.FilePath, this.FileSplitUnit);
            return this.GetRepository(connectionString);
        }

        //// <summary>
        /// 获取存储策略
        /// </summary>
        /// <param name="prerequisite">DB文件完整路径</param>
        /// <returns>存储策略</returns>
        public IMeasureDataRepository GetRepository(string prerequisite)
        {
            string connectionString = Helpers.SqliteConnectionHelper.BuildConnectionString(prerequisite);
            return new Sqlite.SqliteMeasureDataRepository(connectionString);
        }

        #endregion      

        #region static methods

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="context">待拷贝存储参数</param>
        /// <returns>新存储参数</returns>
        public static SqliteMeasureDataPersistenceContext Copy(SqliteMeasureDataPersistenceContext context)
        {
            return new SqliteMeasureDataPersistenceContext(context.FilePath, context.FileSplitUnit, context.FreeSpace);
        }
        #endregion

        private string GetSqliteDbFile(string path, FileSplitUnit unit)
        {
            string fullFileName = FileHelper.GetFullFileNameBySplitUnit(path, "db3", unit);

            this.EnsureSqliteDbFileExist(fullFileName);

            return fullFileName;
        }

        /// <summary>
        /// 创建检测量sqlite文件
        /// </summary>
        /// <param name="fullFileName">文件全名</param>
        private void EnsureSqliteDbFileExist(string fullFileName)
        {
            if (!File.Exists(fullFileName))
            {
                FileDiskHelper.EnsureDiskSapceEnough(fullFileName, this.FreeSpace);

                SQLiteConnection.CreateFile(fullFileName);
                string sqlCreateTB = @"CREATE TABLE [MeasureData]
                                    (
	                                    [ID] integer primary key,
	                                    [MeasureID] integer,
                                        [RTUId] varchar(20),
	                                    [CollDatetime] datetime,
	                                    [CollNum] float(22,4),
	                                    [Tag] integer,
	                                    [Sign] integer,
                                        [InsertTime] datetime default (datetime('now','localtime')),
                                        [UpdateTime] datetime default (datetime('now','localtime'))
                                    )";
                string connectionString = SqliteConnectionHelper.BuildConnectionString(fullFileName);

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlCreateTB, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
