using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;

namespace DataAccess.Sqlite
{
    public class SqliteRepositoryBase
    {
        protected string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqliteRepositoryBase(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
  
    }
}
