using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataAccess.Helpers;

namespace DataAccess
{
    public class SqliteSettingPersistenceContext : ISettingPersistenceContext
    {
        public string FilePath { get; private set; }        //文件路径

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public SqliteSettingPersistenceContext(string filePath)
        {
            this.FilePath = FileHelper.ReviseFilePath(filePath);
        }

        #region ISettingPersistenceContext Members

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        public ISettingRepository GetRepository()
        {
            string connectionString = SqliteConnectionHelper.BuildConnectionString(this.FilePath + "DataLogConfig.db3");
            return new Sqlite.SqliteSettingRepository(connectionString);
        }

        #endregion
    }
}
