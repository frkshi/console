using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;

namespace MtuConsole
{
   public class ListMessage
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 收发
        /// </summary>
        public string Direct { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 解包
        /// </summary>
        public string Encode { get; set; }
    }
}
