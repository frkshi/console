using System;
using System.Collections.Generic;
using System.Text;
using MtuConsole.Common;
namespace MtuConsole.TcpProcess
{
    public class CommandMsg
    {
        public RTUCommandType Type;

        public string DestinationRtuID;  //目标rtu
        //  public string RtuID;
        public DateTime StartDate;
        public DateTime EndDate;
        public int LineNumber;
        public string PortID;

        /// <summary>
        /// 命令号
        /// </summary>
        public string CommandId;
        /// <summary>
        /// 发起人
        /// </summary>
        public string Initiator;
        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime InitiateTime;
        /// <summary>
        /// 命令包内容, 格式化后的数据
        /// </summary>
        public string Data;
        /// <summary>
        /// 命令状态
        /// </summary>
        public bool CommandSendState;
    }
}
