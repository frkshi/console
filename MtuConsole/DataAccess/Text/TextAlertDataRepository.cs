using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataEntity;
using System.IO;

namespace DataAccess.Text
{
    internal class TextAlertDataRepository : IAlertDataRepository
    {

        private string _fullFileName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fullFileName">完整文件路径</param>
        public TextAlertDataRepository(string fullFileName)
        {
            _fullFileName = fullFileName;
        }

        #region IAlertDataRepository Members
        public bool InsertSecretDoor(SecreatDoor[] datas)
        {
            return false;
        }
        /// <summary>
        /// 单个报警量保存至文本
        /// </summary>
        /// <param name="entity">报警量</param>
        /// <returns>是否保存成功</returns>
        public bool Insert(AlertData entity)
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
        /// 批量报警量保存至文本
        /// </summary>
        /// <param name="entities">报警量列表</param>
        /// <returns>是否保存成功</returns>
        public bool BulkInsert(IEnumerable<AlertData> entities)
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
        /// 插入alertdetail 数据，望alertdata表和alertdetail表分别插入主从数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool InsertAlertDetail(AlertDataDetail[] datas)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_fullFileName, true))
                {
                    foreach (var item in datas)
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
        /// 留空
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="leftdatas"></param>
        /// <returns></returns>
        public bool InsertAlertDetail(AlertDataDetail[] datas, out List<AlertDataDetail> leftdatas)
        {
            leftdatas = new List<AlertDataDetail>();
            return false;
        }
        #endregion
    }
}
