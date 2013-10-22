using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Collections;
using DataEntity;
namespace MtuConsole.Common
{
    public delegate void CommnicationMessageHandler(SendMsg objMessage);
    public delegate void MessageSentHandler(string msg, string remoteip);
    public enum MsgType
    {
        DATA_MSG = 1,
        COMMAND_SEND_COMPLETE = 2,
        ERR_MSG = 3,
        WHOLE_SOURCE_MSG = 4,   //++ DLE移植
        SYSTEMTIME_SET = 5
    }
    public struct SendMsg
    {
        public MsgType Type;
        public string Msg;
        public DataTable Measuresetting;    //++ DLE移植
        //public DataTable AirPressure;
        public DateTime SendTime;
        public int TableID;
        public string RtuId;

        public string LocalFileName;  //本地文件名
        public int NoteId;  //记录id

        public string RemoteIP;     //++ DLE移植

    }
    public class FixedSizeStack<T> : IEnumerable<T>, IEnumerable
    {
        private int _size = 3;
        private List<T> _items = new List<T>();

        public FixedSizeStack(int size)
        {
            _size = size;
        }

        public void Add(T item)
        {
            if (_items.Count == _size)
                _items.RemoveAt(0);

            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public int Count
        {
            get { return _size; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
    public class RTULogStack : FixedSizeStack<RTULog>
    {
        public RTULogStack() : base(1) { }
    }
    public class RTUDataCache
    {
        public RTUDataCache()
        {
            RTULog = new RTULogStack();
        }

        public RTULogStack RTULog { get; set; }

        private Hashtable _properties = new Hashtable();
        public object this[string property]
        {
            get
            {
                if (!_properties.ContainsKey(property))
                    return null;
                return _properties[property];
            }
            set
            {
                if (!_properties.ContainsKey(property))
                    _properties.Add(property, value);
                else
                    _properties[property] = value;
            }
        }
    }
    public class RTUDataCacheDictionary
    {
        private Dictionary<string, RTUDataCache> _data;

        public RTUDataCacheDictionary()
        {
            _data = new Dictionary<string, RTUDataCache>();
        }

        public bool Contains(string rtuid)
        {
            return _data.ContainsKey(rtuid);
        }

        public void Add(string rtuid, RTUDataCache cacheData)
        {
            if (!_data.ContainsKey(rtuid))
                _data.Add(rtuid, cacheData);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public void Remove(string rtuid)
        {
            if (!_data.ContainsKey(rtuid))
                _data.Remove(rtuid);
        }

        public RTUDataCache this[string rtuid]
        {
            get
            {
                if (!Contains(rtuid))
                    _data.Add(rtuid, new RTUDataCache());

                return _data[rtuid];
            }
            //set { _data[rtuid] = value; }
        }

        public IList<RTUDataCache> Values { get { return new List<RTUDataCache>(_data.Values); } }
    }

}
