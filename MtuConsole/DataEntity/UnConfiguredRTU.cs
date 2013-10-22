using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataEntity
{
    [Serializable]
    public class UnConfiguredRTU : EntityBase
    {
        private string _rtuid;
        public string RtuID
        {
            get { return _rtuid; }
            set { _rtuid = value; }
        }

        private string _producttypeid;
        public string ProductTypeID
        {
            get { return _producttypeid; }
            set { _producttypeid = value; }
        }
        private DateTime _inserttime;
        public DateTime InsertTime
        {
            get { return _inserttime; }
            set { _inserttime = value; }
        }

        private int _tag;
        public int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
    }
}
