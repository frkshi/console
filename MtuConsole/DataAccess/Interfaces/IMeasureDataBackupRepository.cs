
using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;

namespace DataAccess.Interfaces
{
    internal interface IMeasureDataBackupRepository
    {
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns>检测数据集合</returns>
        IEnumerable<MeasureData> LoadAll();
    }
}
