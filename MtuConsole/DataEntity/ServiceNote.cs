using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    public class ServiceNote : EntityBase
    {
       
        private int _id;
        /// <summary>
        /// 记录编号
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _notetime;
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime NoteTime
        {
            get { return _notetime; }
            set { _notetime = value; }
        }

        private string _note;
        /// <summary>
        /// 记录内容
        /// </summary>
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        private int _datatype;
        /// <summary>
        /// 数据类型 0:Info  1:Alert  2:Error
        /// </summary>
        public int DataType
        {
            get { return _datatype; }
            set { _datatype = value; }
        }


        private int _state;
        /// <summary>
        /// 记录状态
        /// </summary>
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
        
    }
}
