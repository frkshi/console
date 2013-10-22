using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;

namespace DataAccess
{
    internal interface ICollectionDataPersistenceContext
    {
        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        ICollectionDataRepository GetRepository();
    }
}
