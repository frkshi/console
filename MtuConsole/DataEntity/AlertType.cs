using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    public class AlertType : EntityBase
    {
        private int _alerttypeid;
        /// <summary>
        /// 报警类型
        /// </summary>
        public int AlertTypeId
        {
            get { return _alerttypeid; }
            set { _alerttypeid = value; }
        }

        private string _alertname;
        /// <summary>
        /// 报警名称
        /// </summary>
        public string AlertName
        {
            get { return _alertname; }
            set
            {
                _alertname = value;
                this.ChangedProperties.Add("AlertName");
            }
        }

        private int _validtime;
        /// <summary>
        /// 报警时效
        /// </summary>
        public int ValidTime
        {
            get { return _validtime; }
            set
            {
                _validtime = value;
                this.ChangedProperties.Add("ValidTime");
            }
        }

        private string _remark;
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set
            {
                _remark = value;
                this.ChangedProperties.Add("Remark");
            }
        }

        private string _staticimageurl;
        /// <summary>
        /// 静态图标
        /// </summary>
        public string StaticImageUrl
        {
            get { return _staticimageurl; }
            set
            {
                _staticimageurl = value;
                this.ChangedProperties.Add("StaticImageUrl");
            }
        }

        private string _dynamicimageurl;
        /// <summary>
        /// 动态图标
        /// </summary>
        public string DynamicImageUrl
        {
            get { return _dynamicimageurl; }
            set
            {
                _dynamicimageurl = value;
                this.ChangedProperties.Add("DynamicImageUrl");
            }
        }

        private int _grouptype;
        /// <summary>
        /// 报警组
        /// </summary>
        public int GroupType
        {
            get { return _grouptype; }
            set
            {
                _grouptype = value;
                this.ChangedProperties.Add("GroupType");
            }
        }
    }
}
