
using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    public class AirPressure : EntityBase
    {
        private DateTime _collecttime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectTime
        {
            get { return _collecttime; }
            set { _collecttime = value; }
        }

        private decimal _airpressurevalue;
        /// <summary>
        /// 气压值
        /// </summary>
        public decimal AirPressureValue
        {
            get { return _airpressurevalue; }
            set { _airpressurevalue = value; }
        }

    }
}
