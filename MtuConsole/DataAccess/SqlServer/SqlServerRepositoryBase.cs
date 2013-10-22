using System;
using System.Collections.Generic;
using System.Text;
using GotDotNet.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using DataEntity;

namespace DataAccess.SqlServer
{
    public class SqlServerRepositoryBase
    {

        #region Properties

        private AdoHelper _adoHelper;
        protected AdoHelper AdoHelper
        {
            get
            {
                if (_adoHelper == null)
                {
                    _adoHelper = new GotDotNet.ApplicationBlocks.Data.SqlServer();
                }
                return _adoHelper;
            }
        }

        protected string ConnectionString { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerRepositoryBase(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion        
  

    }
}
