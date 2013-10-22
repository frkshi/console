using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataEntity;
using System.IO;

namespace DataAccess.Text
{
    internal class TextMeasureDataRepository : IMeasureDataRepository
    {

        private string _fullFileName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fullFileName">完整文件路径</param>
        public TextMeasureDataRepository(string fullFileName)
        {
            _fullFileName = fullFileName;
        }

        #region IMeasureDataRepository Members

        /// <summary>
        /// 单个检测量保存至文本
        /// </summary>
        /// <param name="entity">检测量</param>
        /// <returns>是否保存成功</returns>
        public bool Insert(MeasureData entity)
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
        /// 批量检测量保存至文本
        /// </summary>
        /// <param name="entities">检测量列表</param>
        /// <returns>是否保存成功</returns>
        public bool BulkInsert(IEnumerable<MeasureData> entities)
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
