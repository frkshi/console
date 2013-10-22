
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Xml;
using System.Reflection;
using System.Threading;


using MtuConsole.Common;
using DataAccess;
using DataEntity;



namespace MtuConsole.TcpProcess
{
    /// <summary>
    /// processcontrol属性集
    /// </summary>
    public class ProcessControlPropties : CommunicationPropties
    {
        /// <summary>
        /// 指令回复集合
        /// </summary>
        private Hashtable _commandListToReply;
        public Hashtable CommandListToReply
        {
            get
            {
                return _commandListToReply;
            }
            set
            {
                _commandListToReply = value;
            }
        }
        /// <summary>
        /// 线程数
        /// </summary>
        private int _processThreadCount;
        public int ProcessThreadCount
        {
            get
            {
                return _processThreadCount;
            }
            set
            {
                _processThreadCount = value;
            }

        }
        /// <summary>
        /// 下发列表
        /// </summary>
        private Hashtable _commandList;
        public Hashtable CommandList
        {
            get
            {
                return _commandList;
            }
            set
            {
                _commandList = value;
            }
        }

        /// <summary>
        /// state 指示processcontrol状态，0：未initial 1:开始initial 2:正常工作 3：load失败 4：正在退出
        /// </summary>
        private int _state;
        public int State
        {
            get
            { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 通道启动时间 
        /// </summary>
        private DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
            }
        }
        /// <summary>
        /// 通道结束时间
        /// </summary>
        private DateTime _endTime;
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
            }
        }

        /// <summary>
        /// 解码组件名
        /// </summary>
        private String _decodeDllName;
        public string DecodeDllName
        {
            get { return _decodeDllName; }
            set { _decodeDllName = value; }
        }

        /// <summary>
        /// 通道号
        /// </summary>
        private int _processgroupID;
        public int ProcessGroupID
        {
            get { return _processgroupID; }
            set { _processgroupID = value; }

        }

        /// <summary>
        /// 使用气压计
        /// </summary>
        private bool _enableAirPressure;
        public bool EnableAirPressure
        {
            get { return _enableAirPressure; }
            set { _enableAirPressure = value; }
        }

        private RTUDataCacheDictionary _rtuDataCache;
        public RTUDataCacheDictionary RTUDataCache
        {
            get { return _rtuDataCache; }
            set { _rtuDataCache = value; }
        }

    }
    /// <summary>
    /// 通道属性类
    /// </summary>
    public class CommunicationPropties
    {   /// <summary>
        /// 监听端口
        /// </summary>
        private int _tcpPort;
        public int TcpPort
        {
            get { return _tcpPort; }
            set { _tcpPort = value; }
        }

        /// <summary>
        /// 是否回复
        /// </summary>
        private bool _needReply;

        public bool NeedReply
        {
            get { return _needReply; }
            set { _needReply = value; }
        }

        /// <summary>
        /// 午夜对时
        /// </summary>
        private bool _checkTimeMidNight;
        public bool CheckTimeMidNight
        {
            get { return _checkTimeMidNight; }
            set { _checkTimeMidNight = value; }
        }
        /// <summary>
        /// 偏移量天
        /// </summary>
        private int _addDay;
        public int AddDay
        {
            get { return _addDay; }
            set { _addDay = value; }
        }
        /// <summary>
        /// 偏移量秒
        /// </summary>
        private int _addSecond;
        public int AddSecond
        {
            get { return _addSecond; }
            set { _addSecond = value; }
        }




    }
}
