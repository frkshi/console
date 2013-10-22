using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    /// <summary>
    /// 终端设置
    /// </summary>
    [Serializable]
    public class RTUSetting : EntityBase
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
            }
        }

        private string _rtuName;
        /// <summary>
        /// 终端名称
        /// </summary>
        public string RTUName
        {
            get
            {
                return this._rtuName;
            }
            set
            {
                this._rtuName = value;
                this.ChangedProperties.Add("RTUName");
            }
        }

        private string _rtuSName;
        /// <summary>
        /// 终端简称
        /// </summary>
        public string RTUSName
        {
            get
            {
                return this._rtuSName;
            }
            set
            {
                this._rtuSName = value;
                this.ChangedProperties.Add("RTUSName");
            }
        }

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

        private DateTime _installDate;
        /// <summary>
        /// 安装日期
        /// </summary>
        public DateTime InstallDate
        {
            get
            {
                return this._installDate;
            }
            set
            {
                this._installDate = value;
                this.ChangedProperties.Add("InstallDate");
            }
        }

        private string _installLoca;
        /// <summary>
        /// 安装位置
        /// </summary>
        public string InstallLoca
        {
            get
            {
                return this._installLoca;
            }
            set
            {
                this._installLoca = value;
                this.ChangedProperties.Add("InstallLoca");
            }
        }

        private string _installAddr;
        /// <summary>
        /// 安装地址
        /// </summary>
        public string InstallAddr
        {
            get
            {
                return this._installAddr;
            }
            set
            {
                this._installAddr = value;
                this.ChangedProperties.Add("InstallAddr");
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
        /// 采样周期
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
        /// 上报周期
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
        /// 保存周期
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

        private int _deviceStatus = 1;
        /// <summary>
        /// 设备状态
        /// </summary>
        public int DeviceStatus
        {
            get { return _deviceStatus; }
            set
            { 
                _deviceStatus = value;
                this.ChangedProperties.Add("DeviceStatus");
            }
        }

        private int _currentCycle = 1;
        /// <summary>
        /// 当前使用的周期
        /// </summary>
        public int CurrentCycle
        {
            get { return _currentCycle; }
            set
            {
                _currentCycle = value;
                this.ChangedProperties.Add("CurrentCycle");
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

        private string _mobileNo;
        /// <summary>
        /// 终端移动设备号
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
                this.ChangedProperties.Add("CommunicationId");
            }
        }

        private int _communicationId1;
        /// <summary>
        /// 通讯编号1
        /// </summary>
        public int CommunicationId1
        {
            get
            {
                return this._communicationId1;
            }
            set
            {
                this._communicationId1 = value;
                this.ChangedProperties.Add("CommunicationId1");
            }
        }

        private int _communicationId2;
        /// <summary>
        /// 通讯编号2
        /// </summary>
        public int CommunicationId2
        {
            get
            {
                return this._communicationId2;
            }
            set
            {
                this._communicationId2 = value;
                this.ChangedProperties.Add("CommunicationId2");
            }
        }

        private bool _isOpen;
        /// <summary>
        /// 手机是否常开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return this._isOpen;
            }
            set
            {
                this._isOpen = value;
                this.ChangedProperties.Add("IsOpen");
            }
        }

        private string _openTime1;
        /// <summary>
        /// 第一次开机时间
        /// </summary>
        public string OpenTime1
        {
            get
            {
                return this._openTime1;
            }
            set
            {
                this._openTime1 = value;
                this.ChangedProperties.Add("OpenTime1");
            }
        }

        private string _openTime2;
        /// <summary>
        /// 第二次开机时间
        /// </summary>
        public string OpenTime2
        {
            get
            {
                return this._openTime2;
            }
            set
            {
                this._openTime2 = value;
                this.ChangedProperties.Add("OpenTime2");
            }
        }

        private string _openTime3;
        /// <summary>
        /// 第三次开机时间
        /// </summary>
        public string OpenTime3
        {
            get
            {
                return this._openTime3;
            }
            set
            {
                this._openTime3 = value;
                this.ChangedProperties.Add("OpenTime3");
            }
        }

        private string _openTime4;
        /// <summary>
        /// 第四次开机时间
        /// </summary>
        public string OpenTime4
        {
            get
            {
                return this._openTime4;
            }
            set
            {
                this._openTime4 = value;
                this.ChangedProperties.Add("OpenTime4");
            }
        }

        private string _openTime5;
        /// <summary>
        /// 第五次开机时间
        /// </summary>
        public string OpenTime5
        {
            get
            {
                return this._openTime5;
            }
            set
            {
                this._openTime5 = value;
                this.ChangedProperties.Add("OpenTime5");
            }
        }

        private string _serviceCenter;
        /// <summary>
        /// 通讯服务中心号码
        /// </summary>
        public string ServiceCenter
        {
            get
            {
                return this._serviceCenter;
            }
            set
            {
                this._serviceCenter = value;
                this.ChangedProperties.Add("ServiceCenter");
            }
        }

        private string _tablename;
        /// <summary>
        /// 目标表
        /// </summary>
        public string TableName
        {
            get
            {
                return this._tablename;
            }
            set
            {
                this._tablename = value;
                this.ChangedProperties.Add("TableName");
            }
        }


        private int _regionId;
        /// <summary>
        /// 区域编号
        /// </summary>
        public int RegionId
        {
            get
            {
                return this._regionId;
            }
            set
            {
                this._regionId = value;
                this.ChangedProperties.Add("RegionId");
            }
        }

        private int _powerSupply;
        /// <summary>
        /// 供电方式
        /// </summary>
        public int PowerSupply
        {
            get
            {
                return this._powerSupply;
            }
            set
            {
                this._powerSupply = value;
                this.ChangedProperties.Add("PowerSupply");
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

        private string _caliber;
        /// <summary>
        /// 口径
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
        /// 口径标准类型
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

        private bool _sendexceptionalert;
        /// <summary>
        /// 是否发送异常报警
        /// </summary>
        public bool SendExceptionAlert
        {
            get { return _sendexceptionalert; }
            set
            {
                _sendexceptionalert = value;
                this.ChangedProperties.Add("SendExceptionAlert");
            }
        }

        private bool _sendvoltalert;
        /// <summary>
        /// 是否发送电池低压报警
        /// </summary>
        public bool SendVoltAlert
        {
            get { return _sendvoltalert; }
            set
            {
                _sendvoltalert = value;
                this.ChangedProperties.Add("SendVoltAlert");
            }
        }

        private DateTime _insertTime;
        /// <summary>
        /// 输入时间
        /// </summary>
        public DateTime InsertTime
        {
            get
            {
                return this._insertTime;
            }
            set
            {
                this._insertTime = value;
            }
        }

        private DateTime _updateTime;
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime UpdateTime
        {
            get
            {
                return this._updateTime;
            }
            set
            {
                this._updateTime = value;
            }
        }

        private decimal _airpressure;
        public decimal AirPressure
        {
            get
            {
                return this._airpressure;
            }
            set
            {
                this._airpressure = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RTUSetting()
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templet">模板</param>
        public RTUSetting(RTUTemplet templet)
        {
            //this.Manu = templet.Manu;
            //this.Analog = templet.Analog;
            //this.Digit = templet.Digit;
            //this.VoltAlert = templet.VoltAlert;
            //this.VoltClose = templet.VoltClose;
            //this.CollCycle = templet.CollCycle;
            //this.SendCycle = templet.SendCycle;
            //this.SaveCycle = templet.SaveCycle;
            //this.ServiceCenter = templet.ServiceCenter;
            //this.CommunicationTypeId = templet.CommunicationTypeId;
            //this.Caliber = templet.Caliber;
            //this.CaliberType = templet.CaliberType;
            AddInfoByTemplet(templet);
        }

        /// <summary>
        /// 根据模板补充信息
        /// </summary>
        /// <param name="templet">模板</param>
        public void AddInfoByTemplet(RTUTemplet templet)
        {
            if (string.IsNullOrEmpty(Manu))
                this.Manu = templet.Manu;
            if (this.Analog == 0)
                this.Analog = templet.Analog;
            if (this.Digit == 0)
                this.Digit = templet.Digit;
            if (this.VoltAlert == Convert.ToDecimal(0))
                this.VoltAlert = templet.VoltAlert;
            if (this.VoltClose == Convert.ToDecimal(0))
                this.VoltClose = templet.VoltClose;
            if (this.CollCycle == 0)
                this.CollCycle = templet.CollCycle;
            if (this.SendCycle == 0)
                this.SendCycle = templet.SendCycle;
            if (this.SaveCycle == 0)
                this.SaveCycle = templet.SaveCycle;
            if (string.IsNullOrEmpty(ServiceCenter))
                this.ServiceCenter = templet.ServiceCenter;
            if (string.IsNullOrEmpty(CommunicationTypeId))
                this.CommunicationTypeId = templet.CommunicationTypeId;

            //// DLE设备默认为停机状态
            //DeviceStatus = this.CommunicationTypeId == "3" ? 0 : 1;

            if (string.IsNullOrEmpty(Caliber))
                this.Caliber = templet.Caliber;
            if (CaliberType == 0)
                this.CaliberType = templet.CaliberType;

            //[CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],
            //[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber]
            if (this.SaveCycle2 == 0)
            {
                this.SaveCycle2 = templet.SaveCycle2;
            }
            if (CollCycle2 == 0)
            {
                CollCycle2 = templet.CollCycle2;
            }
            if (SendCycle2 == 0)
            {
                SendCycle2 = templet.SendCycle2;
            }
            if (this.SaveCycle3 == 0)
            {
                this.SaveCycle3 = templet.SaveCycle3;
            }
            if (CollCycle3 == 0)
            {
                CollCycle3 = templet.CollCycle3;
            }
            if (SendCycle3 == 0)
            {
                SendCycle3 = templet.SendCycle3;
            }
            if (SafetyLevel == 0)
            {
                SafetyLevel = templet.SafetyLevel;
            }
            if (AlertLevel == 0)
            {
                AlertLevel = templet.AlertLevel;
            }
            if (BackValue == 0)
            {
                BackValue = templet.BackValue;
            }
            if (SystemVoltage == 0)
            {
                SystemVoltage = templet.SystemVoltage;
            }
            if (UltrasonicVoltage == 0)
            {
                UltrasonicVoltage = templet.UltrasonicVoltage;
            }
            if (LocationDevication == 0)
            {
                LocationDevication = templet.LocationDevication;
            }
            if (WellDeep == 0)
            {
                WellDeep = templet.WellDeep;
            }
            if (WellElevation == 0)
            {
                WellElevation = templet.WellElevation;
            }
            if (PipeElevation == 0)
            {
                PipeElevation = templet.PipeElevation;
            }
            if (PipeCaliber == 0)
            {
                PipeCaliber = templet.PipeCaliber;
            }

        }
    }
}
