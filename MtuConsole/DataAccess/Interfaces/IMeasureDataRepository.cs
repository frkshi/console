using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;

namespace DataAccess.Interfaces
{
    public interface IMeasureDataRepository
    {
        /// <summary>
        /// 新增单个实体
        /// </summary>
        /// <param name="entity">检测量数据实体</param>
        /// <returns>Bool型</returns>
        bool Insert(MeasureData entity);

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities">检测量数据实体集合</param>
        /// <returns>Bool型</returns>
        bool BulkInsert(IEnumerable<MeasureData> entities);
    }
}
