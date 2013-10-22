using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataEntity
{
    public class RTULog : EntityBase
    {
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _ip;
        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
                this.ChangedProperties.Add("IP");
            }
        }

        private string _rtuID;
        public string RTUID
        {
            get { return _rtuID; }
            set
            {
                _rtuID = value;
                this.ChangedProperties.Add("RTUID");
            }
        }

        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                this.ChangedProperties.Add("Time");
            }
        }

        private string _data;
        public string Data
        {
            get { return _data; }
            set { 
                _data = value;
                this.ChangedProperties.Add("Data");
            }
        }
    }
}
