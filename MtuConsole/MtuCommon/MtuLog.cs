using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;

namespace MtuConsole.Common
{

    public enum ActionSource
    {

        RTU,
        /// <summary>
        /// Channel
        /// </summary>       
        Channel,
        /// <summary>
        /// Service
        /// </summary>
        Service,
        /// <summary>
        /// 其他
        /// </summary>
        Other,
        /// <summary>
        /// 错误
        /// </summary>
        Error

    }
    public enum EventLevel
    {
        Low,
        Medium,
        High
    }

    public class LogMessage
    {

        public LogMessage()
        {


        }

        public string Operator
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string IP
        {
            get;
            set;

        }
        public EventLevel Level
        {
            get;
            set;
        }
        public ActionSource Action
        {
            get;
            set;
        }
        public string Detail
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }


    }
    //public class Log4netMessage
    //{
    //    public Log4netMessage(LogMessage msg)
    //    {

    //        Operator = msg.Operator;
    //        Message = msg.Message;
    //        IP = msg.IP;
    //        Level = msg.Level.ToString();
    //        Action = msg.Action.ToString();
    //        Detail = msg.Detail;
    //    }



    //    public string Operator
    //    {
    //        get;
    //        set;
    //    }
    //    public string Message
    //    {
    //        get;
    //        set;
    //    }
    //    public string IP
    //    {
    //        get;
    //        set;

    //    }
    //    public string Level
    //    {
    //        get;
    //        set;
    //    }
    //    public string Action
    //    {
    //        get;
    //        set;
    //    }
    //    public string Detail
    //    {
    //        get;
    //        set;
    //    }
    //    public string Location
    //    {
    //        get;
    //        set;

    //    }
    //}
    public class LogProperty
    {
        /// <summary>
        /// 发起者
        /// </summary>
        public string Launcher
        {
            get;
            set;
        }
        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime ExcuteTime
        {
            get;
            set;
        }
        /// <summary>
        /// 操作对象
        /// </summary>
        public string Target
        {
            get;
            set;
        }
        /// <summary>
        /// 原值
        /// </summary>
        public string Originalvalue
        {
            get;
            set;
        }
        public string FinalValue
        { get; set; }

        public override string ToString()
        {
            string result;
            result = "{0} 发起人[{1}]:{2}-{3} --〉{4}; ";
            result = string.Format(result, ExcuteTime.ToString(), Launcher, Target, Originalvalue, FinalValue);

            return result;
        }
    }


    /// <summary>
    /// 定义service 报警
    /// </summary>
    public class ServiceAlert : LogProperty
    {
        public override string ToString()
        {
            return base.ToString();
        }
    }

    /// <summary>
    /// 服务日志，日志分为报错，服务事件和服务警报
    /// 日志使用log4net向 log.db3 和 txt输出，并同时向API发送
    /// </summary>
    public class MtuLog
    {
        ILog _logger4netsqlite;
        ILog _filelog;



        public MtuLog()
        {

            _logger4netsqlite = LogManager.GetLogger("InfoError");
            _filelog = LogManager.GetLogger("Debug");
        }
        public void Info(LogMessage msg)
        {

            try
            {
                StackTrace st = new StackTrace(true);
                string parentmethod = st.GetFrame(1).GetMethod().DeclaringType.ToString() + ":" + st.GetFrame(1).GetMethod().Name.ToString();

                msg.Location = parentmethod;
                _logger4netsqlite.Info(msg);
                _filelog.Info(msg.Location + ":" + msg.Message);
                // DataChangeNotify.SendServiceNote(msg);
            }
            catch
            { }
        }

        public void Error(string message, Exception e)
        {
            try
            {
                LogMessage lm = new LogMessage();
                lm.Action = ActionSource.Error;
                lm.Message = message;
                lm.Operator = "";
                lm.Level = EventLevel.High;
                lm.IP = "";
                lm.Detail = e.ToString();
                StackTrace st = new StackTrace(true);
                string parentmethod = st.GetFrame(1).GetMethod().DeclaringType.ToString() + ":" + st.GetFrame(1).GetMethod().Name.ToString();
                lm.Location = parentmethod;
                _logger4netsqlite.Error(lm);
                _filelog.Error(message, e);
            }
            catch
            {

            }

        }
        public void Error(string message)
        {
            try
            {
                LogMessage lm = new LogMessage();
                lm.Action = ActionSource.Error;
                lm.Message = message;
                lm.Operator = "";
                lm.Level = EventLevel.High;
                lm.IP = "";
                StackTrace st = new StackTrace(true);
                string parentmethod = st.GetFrame(1).GetMethod().DeclaringType.ToString() + ":" + st.GetFrame(1).GetMethod().Name.ToString();
                lm.Location = parentmethod;
                _logger4netsqlite.Error(lm);
                _filelog.Error(parentmethod + ":" + message);
            }
            catch
            {

            }

        }
        public void Debug(string msg)
        {
            StackTrace st = new StackTrace(true);
            string parentmethod = st.GetFrame(1).GetMethod().DeclaringType.ToString() + ":" + st.GetFrame(1).GetMethod().Name.ToString();

            _filelog.Debug(parentmethod + ":" + msg);
        }



    }
}
