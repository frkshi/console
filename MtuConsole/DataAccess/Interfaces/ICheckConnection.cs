using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
    internal interface ICheckConnection
    {
        /// <summary>
        /// 检测数据库连接是否通畅
        /// </summary>
        /// <returns>bool型</returns>
        bool CheckConnection();
    }
}
