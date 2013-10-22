using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    public class AlertData
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
        /// 终端编号
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
        /// 采集开始时间
        /// </summary>
        public DateTime CollStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 突变开始数据
        /// </summary>
        public decimal StartNum
        {
            get;
            set;
        }

        /// <summary>
        /// 采集结束时间
        /// </summary>
        public DateTime CollEndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 突变结束数据
        /// </summary>
        public decimal EndNum
        {
            get;
            set;
        }

        /// <summary>
        /// 变化值
        /// </summary>
        public decimal Ratio
        {
            get;
            set;
        }        

        /// <summary>
        /// 报警类型
        /// </summary>
        public int AlertTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// 标志
        /// </summary>
          public byte Tag
        {
            get;
            set;
        }

        /// <summary>
          /// 标识
        /// </summary>
        public int Sign
        {
            get;
            set;
        }

        /// <summary>
        /// 重载ToString方法
        /// </summary>
        /// <returns>字符串</returns>
        public override string  ToString()
        {
            return Convert.ToString(Id) + "," + Convert.ToString(MeasureId) + "," + Convert.ToString(RTUId) + "," 
                + Convert.ToString(CollStartTime) + "," + Convert.ToString(StartNum) + ","
                + Convert.ToString(CollEndTime) + "," + Convert.ToString(EndNum) + ","
                + Convert.ToString(Ratio) + "," + Convert.ToString(AlertTypeId) + "," 
                + Convert.ToString(Tag) + ","  + Convert.ToString(Sign) ;
                
        }

    }
    /// <summary>
    /// alertdatadetail结构
    /// </summary>
    public class AlertDataDetail
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
        /// 终端编号
        /// </summary>
        public string RTUId
        {
            get;
            set;
        }

        /// <summary>
        /// 采集开始时间
        /// </summary>
        public string CollTimes
        {
            get;
            set;
        }

        /// <summary>
        /// 突变开始数据
        /// </summary>
        public string CollNums
        {
            get;
            set;
        }

        /// <summary>
        /// 报警类型
        /// </summary>
        public int AlertTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// 标志
        /// </summary>
        public byte Tag
        {
            get;
            set;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public int Sign
        {
            get;
            set;
        }
        /// <summary>
        /// 重载ToString方法
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return Convert.ToString(Id) + "," + Convert.ToString(MeasureId) + "," + Convert.ToString(RTUId) + ","
                + Convert.ToString(CollTimes) + Convert.ToString(CollNums)  +  Convert.ToString(AlertTypeId) + ","
                + Convert.ToString(Tag) + "," + Convert.ToString(Sign);

        }


    }

    
}
