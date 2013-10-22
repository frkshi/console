
using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    /// <summary>
    /// 终端设置模板
    /// </summary>
    [Serializable]
    public class RTUTemplet : EntityBase
    {        
        private string _producttypeid;
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductTypeId
        {
            get
            {
                return this._producttypeid;
            }
            set
            {
                this._producttypeid = value;
                this.ChangedProperties.Add("ProductTypeId");
            }
        }
       
        private string _manu;
        /// <summary>
        /// 厂商
        /// </summary>
        public string Manu
        {
            get
            {
                return this._manu;
            }
            set
            {
                this._manu = value;
                this.ChangedProperties.Add("Manu");
            }
        }        

        private int _analog;
        /// <summary>
        /// 模拟量数
        /// </summary>
        public int Analog
        {
            get
            {
                return this._analog;
            }
            set
            {
                this._analog = value;
                this.ChangedProperties.Add("Analog");
            }
        }

        private int _digit;
        /// <summary>
        /// 脉冲数字量数
        /// </summary>
        public int Digit
        {
            get
            {
                return this._digit;
            }
            set
            {
                this._digit = value;
                this.ChangedProperties.Add("Digit");
            }
        }

        private decimal _voltAlert;
        /// <summary>
        /// 电池低电压报警线
        /// </summary>
        public decimal VoltAlert
        {
            get 
            {
                return this._voltAlert;
            }
            set
            {
                this._voltAlert = value;
                this.ChangedProperties.Add("VoltAlert");
            }
        }

        private decimal _voltClose;
        /// <summary>
        /// 电池低电压关闭线
        /// </summary>
        public decimal VoltClose
        {
            get
            {
                return this._voltClose;
            }
            set
            {
                this._voltClose = value;
                this.ChangedProperties.Add("VoltClose");
            }
        }

        private int _collCycle;
        /// <summary>
        /// 默认采样周期
        /// </summary>
        public int CollCycle
        {
            get
            {
                return this._collCycle;
            }
            set
            {
                this._collCycle = value;
                this.ChangedProperties.Add("CollCycle");
            }
        }

        private int _sendCycle;
        /// <summary>
        /// 默认上报周期
        /// </summary>
        public int SendCycle
        {
            get
            {
                return this._sendCycle;
            }
            set
            {
                this._sendCycle = value;
                this.ChangedProperties.Add("SendCycle");
            }
        }

        private int _saveCycle;
        /// <summary>
        /// 默认保存周期
        /// </summary>
        public int SaveCycle
        {
            get
            {
                return this._saveCycle;
            }
            set
            {
                this._saveCycle = value;
                this.ChangedProperties.Add("SaveCycle");
            }
        }


        #region 为液位仪所加字段
        private int _collCycle2;
        /// <summary>
        /// 采样周期2
        /// </summary>
        public int CollCycle2
        {
            get
            {
                return this._collCycle2;
            }
            set
            {
                this._collCycle2 = value;
                this.ChangedProperties.Add("CollCycle2");
            }
        }

        private int _sendCycle2;
        /// <summary>
        /// 上报周期
        /// </summary>
        public int SendCycle2
        {
            get
            {
                return this._sendCycle2;
            }
            set
            {
                this._sendCycle2 = value;
                this.ChangedProperties.Add("SendCycle2");
            }
        }

        private int _saveCycle2;
        /// <summary>
        /// 保存周期
        /// </summary>
        public int SaveCycle2
        {
            get
            {
                return this._saveCycle2;
            }
            set
            {
                this._saveCycle2 = value;
                this.ChangedProperties.Add("SaveCycle2");
            }
        }



        private int _collCycle3;
        /// <summary>
        /// 采样周期
        /// </summary>
        public int CollCycle3
        {
            get
            {
                return this._collCycle3;
            }
            set
            {
                this._collCycle3 = value;
                this.ChangedProperties.Add("CollCycle3");
            }
        }

        private int _sendCycle3;
        /// <summary>
        /// 上报周期
        /// </summary>
        public int SendCycle3
        {
            get
            {
                return this._sendCycle3;
            }
            set
            {
                this._sendCycle3 = value;
                this.ChangedProperties.Add("SendCycle3");
            }
        }

        private int _saveCycle3;
        /// <summary>
        /// 保存周期
        /// </summary>
        public int SaveCycle3
        {
            get
            {
                return this._saveCycle3;
            }
            set
            {
                this._saveCycle3 = value;
                this.ChangedProperties.Add("SaveCycle3");
            }
        }


        private decimal _safetylevel;
        /// <summary>
        /// 安全水位
        /// </summary>
        public decimal SafetyLevel
        {
            get
            {
                return this._safetylevel;
            }
            set
            {
                this._safetylevel = value;
                this.ChangedProperties.Add("SafetyLevel");
            }
        }

        private decimal _alertlevel;
        /// <summary>
        /// 警戒水位
        /// </summary>
        public decimal AlertLevel
        {
            get
            {
                return this._alertlevel;
            }
            set
            {
                this._alertlevel = value;
                this.ChangedProperties.Add("AlertLevel");
            }
        }

        private decimal _BackValue;
        /// <summary>
        /// 回差值
        /// </summary>
        public decimal BackValue
        {
            get
            {
                return this._BackValue;
            }
            set
            {
                this._BackValue = value;
                this.ChangedProperties.Add("BackValue");
            }
        }

        //LocationDevication
        //WellDeep
        //WellElevation
        //PipeElevation
        //PipeCaliber


        private decimal _locationdevication;
        /// <summary>
        /// 安装偏差
        /// </summary>
        public decimal LocationDevication
        {
            get
            {
                return this._locationdevication;
            }
            set
            {
                this._locationdevication = value;
                this.ChangedProperties.Add("LocationDevication");
            }
        }


        private decimal _welldeep;
        /// <summary>
        /// 井深
        /// </summary>
        public decimal WellDeep
        {
            get
            {
                return this._welldeep;
            }
            set
            {
                this._welldeep = value;
                this.ChangedProperties.Add("WellDeep");
            }
        }



        private decimal _wellelevation;
        /// <summary>
        /// 井海拔
        /// </summary>
        public decimal WellElevation
        {
            get
            {
                return this._wellelevation;
            }
            set
            {
                this._wellelevation = value;
                this.ChangedProperties.Add("WellElevation");
            }
        }

        private decimal _pipeelevation;
        /// <summary>
        /// 排水管底海拔
        /// </summary>
        public decimal PipeElevation
        {
            get
            {
                return this._pipeelevation;
            }
            set
            {
                this._pipeelevation = value;
                this.ChangedProperties.Add("PipeElevation");
            }
        }

        private decimal _pipecaliber;
        /// <summary>
        /// 排水管径
        /// </summary>
        public decimal PipeCaliber
        {
            get
            {
                return this._pipecaliber;
            }
            set
            {
                this._pipecaliber = value;
                this.ChangedProperties.Add("PipeCaliber");
            }
        }



        private decimal _systemvoltage;

        /// <summary>
        /// 系统电压
        /// </summary>
        public decimal SystemVoltage
        {
            get { return this._systemvoltage; }
            set
            {
                this._systemvoltage = value;
                this.ChangedProperties.Add("SystemVoltage");
            }
        }


        private decimal _ultrasonicvoltage;
        /// <summary>
        /// 超声波电压
        /// </summary>
        public decimal UltrasonicVoltage
        {
            get { return this._ultrasonicvoltage; }
            set
            {
                this._ultrasonicvoltage = value;
                this.ChangedProperties.Add("UltrasonicVoltage");
            }
        }
        #endregion 


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

        private string _servicecenter;
        /// <summary>
        /// 服务中心号
        /// </summary>
        public string ServiceCenter
        {
            get
            {
                return this._servicecenter;
            }
            set
            {
                this._servicecenter = value;
                this.ChangedProperties.Add("ServiceCenter");
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

        private string _caliber;
        /// <summary>
        ///口径 
        /// </summary>
        public string Caliber
        {
            get
            {
                return this._caliber;
            }
            set
            {
                this._caliber = value;
                this.ChangedProperties.Add("Caliber");
            }
        }

        private int _caliberType;
        /// <summary>
        /// 口径标准
        /// </summary>
        public int CaliberType
        {
            get
            {
                return this._caliberType;
            }
            set
            {
                this._caliberType = value;
                this.ChangedProperties.Add("CaliberType");
            }
        }
    }
}
