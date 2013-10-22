using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    [Serializable]
    public class MeasureSetting : EntityBase
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

        private int _measureid;
        /// <summary>
        /// 检测量编号
        /// </summary>
        public int MeasureId
        {
            get { return _measureid; }
            set { _measureid = value; }
        }

        private string _measurename;
        /// <summary>
        /// 检测量名称
        /// </summary>
        public string MeasureName
        {
            get { return _measurename; }
            set {
                _measurename = value;
                this.ChangedProperties.Add("MeasureName");
            }
        }

        private string _rtuid;
        /// <summary>
        /// 终端编号
        /// </summary>
        public string RTUId
        {
            get { return _rtuid; }
            set
            {
                _rtuid = value;
                this.ChangedProperties.Add("RTUId");
            }
        }
        

        private string _signaltype;
        /// <summary>
        /// 信号类型
        /// </summary>
        public string SignalType
        {
            get { return _signaltype; }
            set
            {
                _signaltype = value;
                this.ChangedProperties.Add("SignalType");
            }
        }    

        private string _datatype;
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType
        {
            get { return _datatype; }
            set
            {
                _datatype = value;
                this.ChangedProperties.Add("DataType");
            }
        }  
        
        private string _portid;
        /// <summary>
        /// 数据标号
        /// </summary>
        public string PortId
        {
            get { return _portid; }
            set
            {
                _portid = value;
                this.ChangedProperties.Add("PortId");
            }
        }

        private string _unit;
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                this.ChangedProperties.Add("Unit");
            }
        }

        private decimal _range;
        /// <summary>
        /// 量程
        /// </summary>
        public decimal Range
        {
            get { return _range; }
            set
            {
                _range = value;
                this.ChangedProperties.Add("Range");
            }
        }

        private decimal _elevation;
        /// <summary>
        /// 标高
        /// </summary>
        public decimal Elevation
        {
            get { return _elevation; }
            set
            {
                _elevation = value;
                this.ChangedProperties.Add("Elevation");
            }
        }

        private decimal _precision;
        /// <summary>
        /// 精度
        /// </summary>
        public decimal Precision
        {
            get { return _precision; }
            set
            {
                _precision = value;
                this.ChangedProperties.Add("Precision");
            }
        }

        private decimal _decimaldigits;
        /// <summary>
        /// 小数位数
        /// </summary>
        public decimal DecimalDigits
        {
            get { return _decimaldigits; }
            set
            {
                _decimaldigits = value;
                this.ChangedProperties.Add("DecimalDigits");
            }
        }

        private decimal _scale;
        /// <summary>
        /// 比例系数
        /// </summary>
        public decimal Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                this.ChangedProperties.Add("Scale");
            }
        }

        private decimal _offset;
        /// <summary>
        /// 偏移量
        /// </summary>
        public decimal Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
                this.ChangedProperties.Add("Offset");
            }
        }

        private decimal _revise;
        /// <summary>
        /// 校正值
        /// </summary>
        public decimal Revise
        {
            get { return _revise; }
            set
            {
                _revise = value;
                this.ChangedProperties.Add("Revise");
            }
        }

        private decimal _upperlimit;
        /// <summary>
        /// 上限值
        /// </summary>
        public decimal UpperLimit
        {
            get { return _upperlimit; }
            set
            {
                _upperlimit = value;
                this.ChangedProperties.Add("UpperLimit");
            }
        }

        private decimal _lowerlimit;
        /// <summary>
        /// 下限值
        /// </summary>
        public decimal LowerLimit
        {
            get { return _lowerlimit; }
            set
            {
                _lowerlimit = value;
                this.ChangedProperties.Add("LowerLimit");
            }
        }

        private decimal _ratio;
        /// <summary>
        /// 突变范围
        /// </summary>
        public decimal Ratio
        {
            get { return _ratio; }
            set
            {
                _ratio = value;
                this.ChangedProperties.Add("Ratio");
            }
        }

        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                this.ChangedProperties.Add("Note");
            }
        }

        private int _type;
        /// <summary>
        /// 分类
        /// </summary>
        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                this.ChangedProperties.Add("Type");
            }
        }

        private bool _isopen;
        /// <summary>
        /// 是否生效
        /// </summary>
        public bool IsOpen
        {
            get { return _isopen; }
            set
            {
                _isopen = value;
                this.ChangedProperties.Add("IsOpen");
            }
        }

        private bool _isset;
        /// <summary>
        /// 是否允许设置
        /// </summary>
        public bool IsSet
        {
            get { return _isset; }
            set
            {
                _isset = value;
                this.ChangedProperties.Add("IsSet");
            }
        }

        private bool _sendoutdata;
        /// <summary>
        /// 是否上传越界数据
        /// </summary>
        public bool SendOutData
        {
            get { return _sendoutdata; }
            set
            {
                _sendoutdata = value;
                this.ChangedProperties.Add("SendOutData");
            }
        }

        private bool _sendchangedata;
        /// <summary>
        /// 是否上传突变数据
        /// </summary>
        public bool SendChangeData
        {
            get { return _sendchangedata; }
            set
            {
                _sendchangedata = value;
                this.ChangedProperties.Add("SendChangeData");
            }
        }

        private bool _datastatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        public bool DataStatus
        {
            get { return _datastatus; }
            set
            {
                _datastatus = value;
                this.ChangedProperties.Add("DataStatus");
            }
        }

        private bool _switchtype;
        /// <summary>
        /// 开关量类型
        /// </summary>
        public bool SwitchType
        {
            get { return _switchtype; }
            set
            {
                _switchtype = value; 
                this.ChangedProperties.Add("SwitchType");
            }
        }

        private bool _sendredirectionalert;
        /// <summary>
        /// 是否发送流量转向报警
        /// </summary>
        public bool SendRedirectionAlert
        {

            get { return _sendredirectionalert; }
            set
            {
                _sendredirectionalert = value;
                this.ChangedProperties.Add("SendRedirectionAlert");
            }
        }

        private int _increaseinterval;

        public int IncreaseInterval
        {
            get
            {
                return _increaseinterval;
            }
            set
            {
                _increaseinterval = value;
                this.ChangedProperties.Add("IncreaseInterval");
            }
        }

        private DateTime _inserttime;
        /// <summary>
        /// 输入时间
        /// </summary>
        public DateTime InsertTime
        {
            get { return _inserttime; }
            set { _inserttime = value;}
        }

        private DateTime _updatetime;
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return _updatetime; }
            set { _updatetime = value;}
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MeasureSetting()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templet">模板</param>
        public MeasureSetting(MeasureTemplet templet)
        {
            //this.MeasureName = templet.MTempletName;
            //this.SignalType = templet.SignalType;
            //this.Unit = templet.Unit;
            //this.Range = templet.Range;
            //this.Precision = templet.Precision;
            //this.DecimalDigits = templet.DecimalDigits;
            //this.Scale = templet.Scale;
            //this.Offset = templet.Offset;
            //this.UpperLimit = templet.UpperLimit;
            //this.LowerLimit = templet.LowerLimit;
            //this.Ratio = templet.Ratio;
            //this.DataType = templet.DataType;
            //this.PortId = templet.PortId;
            AddInfoByTemplet(templet);
        }

        /// <summary>
        /// 根据模板填充信息
        /// </summary>
        /// <param name="templet">模板</param>
        public void AddInfoByTemplet(MeasureTemplet templet)
        {
            if (string.IsNullOrEmpty(this.MeasureName))
                this.MeasureName = templet.MTempletName;
            if(string.IsNullOrEmpty(this.SignalType))
                this.SignalType = templet.SignalType;
            if (string.IsNullOrEmpty(this.Unit))
                this.Unit = templet.Unit;
            if (this.Range == Convert.ToDecimal(0))
                this.Range = templet.Range;
            if (this.Precision == Convert.ToDecimal(0))
                this.Precision = templet.Precision;
            if (this.DecimalDigits == Convert.ToDecimal(0))
                this.DecimalDigits = templet.DecimalDigits;
            if (this.Scale == Convert.ToDecimal(0))
                this.Scale = templet.Scale;
            if (this.Offset == Convert.ToDecimal(0))
                this.Offset = templet.Offset;
            if (this.UpperLimit == Convert.ToDecimal(0))
                this.UpperLimit = templet.UpperLimit;
            if (this.LowerLimit == Convert.ToDecimal(0))
                this.LowerLimit = templet.LowerLimit;
            if (this.Ratio == Convert.ToDecimal(0))
                this.Ratio = templet.Ratio;
            if (string.IsNullOrEmpty(this.DataType))
                this.DataType = templet.DataType;
            if (string.IsNullOrEmpty(this.PortId))
                this.PortId = templet.PortId;
            this.IsOpen = true;
        }

        public MeasureSetting CopyMeasureSetting(MeasureSetting entity)
        {
            MeasureSetting ms = new MeasureSetting();
            ms.MeasureId = entity.MeasureId;
            ms.MeasureName = entity.MeasureName;
            ms.RTUId = entity.RTUId;
            ms.SignalType = entity.SignalType;
            ms.DataType = entity.DataType;
            ms.PortId = entity.PortId;
            ms.Unit = entity.Unit;
            ms.Range = entity.Range;
            ms.Elevation = entity.Elevation;
            ms.Precision = entity.Precision;
            ms.DecimalDigits = entity.DecimalDigits;
            ms.Scale = entity.Scale;
            ms.Offset = entity.Offset;
            ms.Revise = entity.Revise;
            ms.UpperLimit = entity.UpperLimit;
            ms.LowerLimit = entity.LowerLimit;
            ms.Ratio = entity.Ratio;
            ms.Note = entity.Note;
            ms.Type = entity.Type;
            ms.IsOpen = entity.IsOpen;
            ms.IsSet = entity.IsSet;
            ms.SendOutData = entity.SendOutData;
            ms.SendChangeData = entity.SendChangeData;
            ms.DataStatus = entity.DataStatus;
            ms.SwitchType = entity.SwitchType;
            ms.SendRedirectionAlert = entity.SendRedirectionAlert;
            ms.InsertTime = entity.InsertTime;
            ms.UpdateTime = entity.UpdateTime;
            return ms;
        }
    }
}
