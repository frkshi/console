using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class SqlServerSettingPersistenceContext : ISettingPersistenceContext
    {

        public string ConnectionString { get; private set; }        //连接字符串

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerSettingPersistenceContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        #region ISettingPersistenceContext Members

        public ISettingRepository GetRepository()
        {
            return new SqlServer.SqlServerSettingRepository(this.ConnectionString);
        }

        #endregion
    }
}
