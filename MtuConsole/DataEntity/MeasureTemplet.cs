using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    [Serializable]
    public class MeasureTemplet : EntityBase
    {
        private int _mtempletid;
        /// <summary>
        /// 模板编号
        /// </summary>
        public int MTempletId
        {
            get { return _mtempletid; }
            set { _mtempletid = value; }
        }

        private string _mtempletname;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string MTempletName
        {
            get { return _mtempletname; }
            set
            {
                _mtempletname = value;
                this.ChangedProperties.Add("MTempletName");
            }
        }

        private string _producttypeid;
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductTypeId
        {
            get { return _producttypeid; }
            set
            {
                _producttypeid = value;
                this.ChangedProperties.Add("ProductTypeId");
            }
        }
        

        private string _manu;
        /// <summary>
        /// 厂商
        /// </summary>
        public string Manu
        {
            get { return _manu; }
            set
            {
                _manu = value;
                this.ChangedProperties.Add("Manu");
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
        /// 变化率报警限
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

    }
}
