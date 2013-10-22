using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class SqlServerAlertDataExportPersistenceContext : IAlertDataPersistenceContext
    {
        public string ConnectionString { get; private set; }        //连接字符串
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerAlertDataExportPersistenceContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #region IMeasureDataPersistenceContext Members

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        public IAlertDataRepository GetRepository()
        {
            return this.GetRepository(this.ConnectionString);
        }

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <param name="prerequisite">连接字符串</param>
        /// <returns>存储策略</returns>
        public IAlertDataRepository GetRepository(string prerequisite)
        {
            return new SqlServer.SqlServerAlertDataExportRepository(prerequisite);
        }

        #endregion
    }
}
