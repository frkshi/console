using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;

namespace DataAccess.Interfaces
{
    public interface ICollectionDataRepository
    {
        /// <summary>
        /// 新增单个实体
        /// </summary>
        /// <param name="entity">采集数据实体</param>
        /// <returns>Bool型</returns>
        bool Insert(CollectionData entity);

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities">采集数据实体集合</param>
        /// <returns>Bool型</returns>
        bool BulkInsert(IEnumerable<CollectionData> entities);


        bool BulkInsert(IEnumerable<SendData> entities);

    }
}
