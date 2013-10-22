using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public static class RepositotyTableContentLimit
    {
        public static readonly int TableContentLimite = 5;

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="RtuCount">Rtu的数量</param>
        /// <returns>数据库表名</returns>
        public static string GetRepositotyTableName(int rtuCount)
        {
            int num = rtuCount / TableContentLimite + 1;
            return string.Format("MeasureData{0}", num.ToString().PadLeft(3, '0'));
        }
    }
}
