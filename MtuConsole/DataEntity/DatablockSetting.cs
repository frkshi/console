
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataEntity
{
    public class DatablockSetting:EntityBase
    {

        /// <summary>
        /// 将各property 值列出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";
            Type type = this.GetType();
            foreach (System.Reflection.PropertyInfo PInfo in type.GetProperties())
            {
                //用PInfo.GetValue获得值 
                string val = Convert.ToString(PInfo.GetValue(this, null));
                //获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作 
                string name = PInfo.Name;

                result += name + "=" + val + ";" + Environment.NewLine;
            }



            return result;
        }
        private int _datablockid;
        /// <summary>
        /// datablockid
        /// </summary>
        public int DataBlockID
        {
            set { _datablockid = value; }
            get { return _datablockid; }
        }
        private int _measureid;
        /// <summary>
        /// measureid
        /// </summary>
        public int Measureid
        {
            get
            {
                return _measureid;
            }
            set
            {
                _measureid = value;
                this.ChangedProperties.Add("Measureid");
            }
        }

        private int _deviceid;
        /// <summary>
        /// deviceid
        /// </summary>
        public int DeviceID
        {
            get { return _deviceid; }
            set { _deviceid = value;
            this.ChangedProperties.Add("DeviceID");
            }
        }

        public int _digit;
        public int Digit
        {
            get { return _digit; }
            set {
                _digit = value;
                this.ChangedProperties.Add("Digit");
            }
        }

        private string _datatypeid;
        /// <summary>
        /// datatypeid
        /// </summary>
        public string DatatypeID
        {
            get { return _datatypeid; }
            set { _datatypeid = value;
            this.ChangedProperties.Add("DatatypeID");
            }
        }
       
        private int _swap;
        /// <summary>
        /// swap
        /// </summary>
        public int Swap
        {
            get { return _swap; }
            set { _swap = value;
            this.ChangedProperties.Add("Swap");
            }
        }
        private int _start;
        /// <summary>
        /// start
        /// </summary>
        public int Start
        {
            get { return _start; }
            set { _start = value;
            this.ChangedProperties.Add("Start");
            }
        }
        private int _length;
        public int Length
        {
            get { return _length; }
            set { _length = value;
                this.ChangedProperties.Add("Length");
            }
        }
    }
}
