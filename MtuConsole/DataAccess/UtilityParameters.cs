using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class UtilityParameters
    {
        /// <summary>
        /// 采集文件回溯月份
        /// 注：重解包命令时，选择重解包的采集数据文件的范围。
        /// </summary>
        public static readonly int FileMonthFilter = 12;

        /// <summary>
        /// 默认磁盘可用空间（M）
        /// </summary>
        public static readonly int DefaultFreeSpace = 100; 
    }
}
