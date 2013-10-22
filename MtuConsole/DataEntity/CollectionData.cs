using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    /// <summary>
    /// 采集数据
    /// </summary>
    [Serializable]
    public class CollectionData
    {
        /// <summary>
        /// 记录编号
        /// </summary>
        public int NoteId
        {
            get;
            set;
        }

        /// <summary>
        /// 发送端移动设备号
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 短消息中心号
        /// </summary>
        public string MessageCenterNo
        {
            get;
            set;
        }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime
        {
            get;
            set;
        }

        /// <summary>
        /// 短消息内容
        /// </summary>
        public string MessageContent
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// BCC校验
        /// </summary>
        public int BccResult
        {
            get;
            set;
        }

        /// <summary>
        /// 帧计数
        /// </summary>
        public decimal FrameMark
        {
            get;
            set;
        }

        /// <summary>
        /// 转换标记
        /// </summary>
        public int TransformMark
        {
            get;
            set;
        }
        /// <summary>
        /// 远端ip
        /// </summary>
        public string RemoteIP
        {
            get;
            set;
        }
        public string RtuID
        {
            get;
            set;
        }
        /// <summary>
        /// 重载ToString方法
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return Convert.ToString(NoteId) + "," + PhoneNumber + "," + MessageCenterNo + ","
                + Convert.ToString(SendTime) + "," + MessageContent + ","
                + Convert.ToString(Status) + "," + Convert.ToString(BccResult) + ","
                + Convert.ToString(FrameMark) + Convert.ToString(TransformMark) + "," + RtuID ;

        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    [Serializable]
    public class SendData
    {
        /// <summary>
        /// 记录编号
        /// </summary>
        public int NoteId
        {
            get;
            set;
        }

        
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime
        {
            get;
            set;
        }

        /// <summary>
        /// 短消息内容
        /// </summary>
        public string MessageContent
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// 转换标记
        /// </summary>
        public int TransformMark
        {
            get;
            set;
        }
        /// <summary>
        /// 远端ip
        /// </summary>
        public string RemoteIP
        {
            get;
            set;
        }

        /// <summary>
        /// 终端编号
        /// </summary>
        public string RtuID
        {
            get;
            set;
        }
        /// <summary>
        /// 重载ToString方法
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return Convert.ToString(NoteId) + ","
                + Convert.ToString(SendTime) + "," + MessageContent + ","
                + Convert.ToString(Status)  + ","
                + Convert.ToString(TransformMark) + ","
                + Convert.ToString(RtuID);

        }
    }

    [Serializable]
    public class SourceCodeData
    {
        /// <summary>
        /// 记录编号
        /// </summary>
        public int NoteId
        {
            get;
            set;
        }


        /// <summary>
        /// 时间
        /// </summary>
        public DateTime MessageTime
        {
            get;
            set;
        }
        /// <summary>
        /// 源包
        /// </summary>
        public string MessageContent
        {
            get;
            set;
        }

        /// <summary>
        /// 远端ip
        /// </summary>
        public string RemoteIP
        {
            get;
            set;
        }
        /// <summary>
        /// 源包方向
        /// </summary>
        public MessageDirection Direction
        {
            get;
            set;
        }
        /// <summary>
        /// 终端编号
        /// </summary>
        public string RtuID
        {
            get;
            set;
        }
    }

    public enum MessageDirection
    { 
        Receive,
        Send
    }
}
