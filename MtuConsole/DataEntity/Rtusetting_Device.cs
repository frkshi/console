using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataEntity
{
    [Serializable]
    public class Rtusetting_Device : EntityBase
    {
        private string _rtuId;
        /// <summary>
        /// 终端编号
        /// </summary>
        public string RTUId
        {
            get
            {
                return this._rtuId;
            }
            set
            {
                this._rtuId = value;
                ChangedProperties.Add("RTUId");
            }
        }
        #region 000
     
        private string _producttype;
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductType
        {
            get { return _producttype; }
            set { 
                _producttype = value;
                ChangedProperties.Add("ProductType");
            }
        }

        private string _softwareVersion;
        /// <summary>
        /// 软件版本号
        /// </summary>
        public string SoftwareVersion
        {
            get
            {
                return _softwareVersion;
            }
            set
            {
                _softwareVersion = value;
                ChangedProperties.Add("SoftwareVersion");
            }
        }

        private string _hardwareVersion;
        /// <summary>
        /// 硬件版本号
        /// </summary>
        public string HardwareVersion
        {
            get
            {
                return _hardwareVersion;
            }
            set
            {
                _hardwareVersion = value;
                ChangedProperties.Add("HardwareVersion");
            }
        }
        #endregion

        #region 010 gprs信息
        private string _apn;
        /// <summary>
        /// APN
        /// </summary>
        public string APN
        {
            get { return _apn; }
            set { _apn = value;
            ChangedProperties.Add("APN");
            }
        }

        private string _ip1;
        /// <summary>
        /// IP1
        /// </summary>
        public string IP1
        {
            get { return _ip1; }
            set { _ip1 = value;
            ChangedProperties.Add("IP1");
            }
        }

        private string _ip2;
        /// <summary>
        /// ip2
        /// </summary>
        public string IP2
        {
            get { return _ip2; }
            set { _ip2 = value;
            ChangedProperties.Add("IP2");
            }
        }

        private string _port1;
        /// <summary>
        /// port1
        /// </summary>
        public string Port1
        {
            get { return _port1; }
            set { _port1 = value;
            ChangedProperties.Add("Port1");
            }
        }
        private string _port2;
        /// <summary>
        /// port2
        /// </summary>
        public string Port2
        {
            get { return _port2; }
            set { _port2 = value;
            ChangedProperties.Add("Port2");
            }
        }
        #endregion
        #region 020 后门信息
        private string _secretip;
        /// <summary>
        /// 暗门ip
        /// </summary>
        public string SecretIP
        {
            get { return _secretip; }
            set { _secretip = value;
            ChangedProperties.Add("SecretIP");
            }
        }

        private string _secretport;
        /// <summary>
        /// 暗门port
        /// </summary>
        public string SecretPort
        {
            get { return _secretport; }
            set { _secretport = value;
            ChangedProperties.Add("SecretPort");
            }
        }
        #endregion
        #region 030 DLA周期
        private int _savecycle1;
        /// <summary>
        /// 压力保存周期，秒
        /// </summary>
        public int Savecycle1
        {
            get { return _savecycle1; }
            set { _savecycle1 = value;
            ChangedProperties.Add("Savecycle1");
            }
        }
        private int _sendcycle1;

        /// <summary>
        /// 压力发送周期，秒
        /// </summary>
        public int Sendcycle1
        {
            get { return _sendcycle1; }
            set { _sendcycle1 = value;
            ChangedProperties.Add("Sendcycle1");
            }
        }

        #endregion

        #region 040 DLC周期 
        private int _savecycle2;
        public int SaveCycle2
        {
            get { return _savecycle2; }
            set { _savecycle2 = value;
            ChangedProperties.Add("SaveCycle2");
            }
        }
        private int _sendcycle2;
        public int Sendcycle2
        {
            get { return _sendcycle2; }
            set { _sendcycle2 = value;
            ChangedProperties.Add("Sendcycle2");
            }
        }
        #endregion
        #region 090 回差值 ,0a0 气压偏移 ,0b0 压力比例系数
        private decimal _backvalue;
        /// <summary>
        /// 回差值
        /// </summary>
        public decimal BackValue
        {
            get { return _backvalue; }
            set {
                _backvalue = value;
                ChangedProperties.Add("BackValue");
                }
        }
        private decimal _airpressure;
        /// <summary>
        /// 气压偏移
        /// </summary>
        public decimal AirPressure
        {
            get { return _airpressure; }
            set { _airpressure = value;
            ChangedProperties.Add("AirPressure");
            }
        }
        private decimal _scale;
        /// <summary>
        /// 压力比例系数
        /// </summary>
        public decimal Scale
        {
            get { return _scale; }
            set { _scale = value;
            ChangedProperties.Add("Scale");
            }
        }

        #endregion

        #region 240 报警设置

        private int _pressureSendChangeAlert;
        
        /// <summary>
        /// 压力，突变报警发送
        /// </summary>
        public int PressureSendChangeAlert
        {
            get { return _pressureSendChangeAlert; }
            set {
                _pressureSendChangeAlert = value;
                ChangedProperties.Add("PressureSendChangeAlert");
            }
        }
        private int _pressureSendOutDataAlert;
        /// <summary>
        /// 压力，越界报警发送
        /// </summary>
        public int PressureSendOutDataAlert
        {
            get { return _pressureSendOutDataAlert; }
            set
            {
                _pressureSendOutDataAlert = value;
                ChangedProperties.Add("PressureSendOutDataAlert");
            }
        }


        private decimal _uplimit;
        /// <summary>
        /// 压力上限值
        /// </summary>
        public decimal Uplimit
        {
            get { return _uplimit; }
            set
            {
                _uplimit = value;
                ChangedProperties.Add("Uplimit");
            }
        }

        private decimal _lowLimit;
        /// <summary>
        /// 压力下限值
        /// </summary>
        public decimal LowLimit
        {
            get { return _lowLimit; }
            set
            {
                _lowLimit = value;
                ChangedProperties.Add("LowLimit");
            }
        }

        private decimal _increaseInterval;
        /// <summary>
        /// 突变阀值
        /// </summary>
        public decimal IncreaseInterval
        {
            get { return _increaseInterval; }
            set
            {
                _increaseInterval = value;
                ChangedProperties.Add("IncreaseInterval");
            }
        }
             
        #endregion

    }
}
