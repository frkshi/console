using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataEntity
{
    public class DeviceSetting:EntityBase
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
        private int _deviceid;
        /// <summary>
        /// Deviceid
        /// </summary>
        public int DeviceID
        {
            get { return _deviceid; }
            set { _deviceid = value; }
        }

        private string _rtuid;
        /// <summary>
        /// rtuid
        /// </summary>
        public string RtuID
        {
            get { return _rtuid; }
            set { _rtuid = value;
            this.ChangedProperties.Add("RtuID");
            }
        }

        private string _devicename;
        /// <summary>
        /// devicename
        /// </summary>
        public string DeviceName
        {
            get { return _devicename; }
            set { _devicename = value;

            this.ChangedProperties.Add("DeviceName");
            }
            
        }

        private string _commandcount;

        public string CommandCount
        {
            get { return _commandcount; }
            set
            {
                _commandcount = value;
                this.ChangedProperties.Add("CommandCount");
            }
        }
    }
}
