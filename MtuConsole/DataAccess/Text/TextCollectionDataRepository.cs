using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataEntity;
using System.IO;

namespace DataAccess.Text
{
    internal class TextCollectionDataRepository : ICollectionDataRepository
    {

        private string _fullFileName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fullFileName">完整文件路径</param>
        public TextCollectionDataRepository(string fullFileName)
        {
            _fullFileName = fullFileName;
        }

        #region ICollectionDataRepository Members

        /// <summary>
        /// 单个采集量保存至文本
        /// </summary>
        /// <param name="entity">检测量</param>
        /// <returns>是否保存成功</returns>
        public bool Insert(CollectionData entity)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_fullFileName, true))
                {
                    sw.WriteLine(entity.ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 批量采集量保存至文本
        /// </summary>
        /// <param name="entities">采集量列表</param>
        /// <returns>是否保存成功</returns>
        public bool BulkInsert(IEnumerable<CollectionData> entities)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_fullFileName, true))
                {
                    foreach (var item in entities)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 批量senddata保存至文本
        /// </summary>
        /// <param name="entities">senddata列表</param>
        /// <returns>是否保存成功</returns>
        public bool BulkInsert(IEnumerable<SendData> entities)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_fullFileName, true))
                {
                    foreach (var item in entities)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion
    }
}
