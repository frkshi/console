using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataEntity
{
    /// <summary>
    /// 后门信息
    /// </summary>
    public  class SecreatDoor
    {
        public string RtuID
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }
        public int Signal
        {
            get;
            set;
        }
        public int ConncetSpend
        {
            get; set;
        }
        public bool ConnectSuccessful
        {
            get;
            set;
        }
        public string ConnectStep
        {
            get;
            set;
        }
        public string ErrCode
        {
            get;
            set;
        }


    }
}
