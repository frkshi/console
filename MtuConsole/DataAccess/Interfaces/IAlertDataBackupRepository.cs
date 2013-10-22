using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;

namespace DataAccess.Interfaces
{
    internal interface IAlertDataBackupRepository
    {
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns>报警数据集合</returns>
        IEnumerable<AlertData> LoadAll();
        /// <summary>
        /// 加载全部alertdetail
        /// </summary>
        /// <returns></returns>
        IEnumerable<AlertDataDetail> LoadAllDetail();
    }
}
