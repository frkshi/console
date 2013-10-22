using System;
using System.Collections.Generic;
using System.Text;
using GotDotNet.ApplicationBlocks.Data;

namespace DataAccess
{
    internal static class CacheContainer
    {
        /// <summary>
        /// 检测量对应表
        /// </summary>
        private static Dictionary<string, string> _measureTableNames;

        /// <summary>
        /// 构造函数
        /// </summary>
        static CacheContainer()
        {
            _measureTableNames = new Dictionary<string, string>();
        }

        /// <summary>
        /// 获取检测量对应表的缓存信息
        /// </summary>
        /// <param name="rtuId">终端Id</param>
        /// <returns>表名</returns>
        public static string GetMeasureTableName(string rtuId)
        {
            string tableName;
            if (_measureTableNames.TryGetValue(rtuId, out tableName))
            {
                return tableName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存检测量和表的对应关系到缓存中
        /// </summary>
        /// <param name="rtuId">终端Id</param>
        /// <param name="tableName">表名</param>
        public static void SaveMeasureTableName(string rtuId, string tableName)
        {
            string val;
            if (_measureTableNames.TryGetValue(rtuId, out val))
            {
                _measureTableNames[rtuId] = tableName;
            }
            else
            {
                _measureTableNames.Add(rtuId, tableName);
            }
        }
    }
}
