using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    /// <summary>
    /// 通讯设置
    /// </summary>
    [Serializable]
    public class CommunicationSetting : EntityBase
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

        private int _communicationId;
        /// <summary>
        /// 通讯编号
        /// </summary>
        public int CommunicationId
        {
            get
            {
                return this._communicationId;
            }
            set
            {
                this._communicationId = value;
            }
        }

        private string _communicationName;
        /// <summary>
        /// 通讯名称
        /// </summary>
        public string CommunicationName
        {
            get
            {
                return this._communicationName;
            }
            set
            {
                this._communicationName = value;
                this.ChangedProperties.Add("CommunicationName");
            }
        }

        private string _mobileNo;
        /// <summary>
        /// 移动通讯号
        /// </summary>
        public string MobileNo
        {
            get
            {
                return this._mobileNo;
            }
            set
            {
                this._mobileNo = value;
                this.ChangedProperties.Add("MobileNo");
            }
        }

        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
                this.ChangedProperties.Add("Note");
            }
        }

        private string _apn;
        /// <summary>
        /// APN名
        /// </summary>
        public string APN
        {
            get
            {
                return this._apn;
            }
            set
            {
                this._apn = value;
                this.ChangedProperties.Add("APN");
            }
        }

        private string _dns1;
        /// <summary>
        /// DNS地址1
        /// </summary>
        public string Dns1
        {
            get
            {
                return this._dns1;
            }
            set
            {
                this._dns1 = value;
                this.ChangedProperties.Add("Dns1");
            }
        }

        private string _dns2;
        /// <summary>
        /// DNS地址2
        /// </summary>
        public string Dns2
        {
            get
            {
                return this._dns2;
            }
            set
            {
                this._dns2 = value;
                this.ChangedProperties.Add("Dns2");
            }
        }

        private string _ip;
        /// <summary>
        /// 域名或IP地址
        /// </summary>
        public string IP
        {
            get
            {
                return this._ip;
            }
            set
            {
                this._ip = value;
                this.ChangedProperties.Add("IP");
            }
        }

        private string _extcp;
        /// <summary>
        /// 外部TCP端口号
        /// </summary>
        public string ExTcp
        {
            get
            {
                return this._extcp;
            }
            set
            {
                this._extcp = value;
                this.ChangedProperties.Add("ExTcp");
            }
        }

        private string _intcp;
        /// <summary>
        /// 内部TCP端口号
        /// </summary>
        public string InTcp
        {
            get
            {
                return this._intcp;
            }
            set
            {
                this._intcp = value;
                this.ChangedProperties.Add("InTcp");
            }
        }

        private string _communicationtypeid;
        /// <summary>
        /// 通讯协议类型
        /// </summary>
        public string CommunicationTypeId
        {
            get
            {
                return this._communicationtypeid;
            }
            set
            {
                this._communicationtypeid = value;
                this.ChangedProperties.Add("CommunicationTypeId");
            }
        }

        private byte _tag;
        /// <summary>
        /// 标志位
        /// </summary>
        public byte Tag
        {
            get
            {
                return this._tag;
            }
            set
            {
                this._tag = value;
                this.ChangedProperties.Add("Tag");
            }
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

       
    }
}
