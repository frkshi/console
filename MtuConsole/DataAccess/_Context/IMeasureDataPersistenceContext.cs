using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;

namespace DataAccess
{
    internal interface IMeasureDataPersistenceContext
    {
        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        IMeasureDataRepository GetRepository();

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <param name="prerequisite">连接字符串</param>
        /// <returns>存储策略</returns>
        IMeasureDataRepository GetRepository(string prerequisite);
    }
}
