using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    /// <summary>
    /// 检测量类
    /// </summary>
    [Serializable]
    public class MeasureData
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 检测量编号
        /// </summary>
        public int MeasureId
        {
            get;
            set;
        }


        /// <summary>
        /// 终端编号（用于判断目标存储表）
        /// </summary>
        public string RTUId
        {
            get;
            set;

        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataType
        {
            get;
            set;
        }

        /// <summary>
        /// 采集日期时间
        /// </summary>
        public DateTime CollDatetime
        {
            get;
            set;
        }

       
        /// <summary>
        /// 采集数据
        /// </summary>
        public decimal CollNum
        {
            get;
            set;
        }

        /// <summary>
        /// 标志
        /// </summary>
        public int Tag
        {
            get;
            set;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public byte Sign
        {
            get;
            set;
        }

        /// <summary>
        /// 输入时间
        /// </summary>
        public DateTime InsertTime
        {
            get;
            set;
        }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime UpdateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 重载Tostring方法
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return Convert.ToString(Id) + "," + MeasureId.ToString() + "," 
                + Convert.ToString(CollDatetime) + "," + Convert.ToString(CollNum) + ","
                + Convert.ToString(Tag) + "," + Convert.ToString(Sign) + ","
                + Convert.ToString(InsertTime) + "," + Convert.ToString(UpdateTime);
        }
    }
}
