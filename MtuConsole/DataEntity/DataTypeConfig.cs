using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    public class DataTypeConfig
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }       
        
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }        

        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        public string Class
        {
            get;
            set;
        }

        /// <summary>
        /// 精度
        /// </summary>
        public int  Precision
        {
            get;
            set;
        }     

    }
}
