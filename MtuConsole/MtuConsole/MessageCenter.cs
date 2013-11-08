using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MtuConsole.Common;
namespace MtuConsole
{
    /// <summary>
    /// console中往各窗体发送通道消息事件
    /// </summary>
    /// <param name="objMessage"></param>
    public delegate void ConsoleMessageHandler(Message objMessage);



   public class MessageCenter
    {
      // public event MtuMessageHandler SendMsg;
        ServerHost _host;

        public event ConsoleMessageHandler Onreceivemsg;
        public ServerHost MessageHost
        { get{return _host;} set{_host=value;} }
        public MessageCenter()
        {
            _host = new ServerHost();
            RegistHost();
           
        }

        void _host_SendMsg(Message objMessage)
        {
            fireMessage(objMessage);
        }
        public void RegistHost()
        {
            _host.SendMsg += new MtuMessageHandler(_host_SendMsg);
        }


        private void fireMessage(Message objmsg)
        {
            Onreceivemsg(objmsg);
        }


    }
}
