using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MtuConsole.Common;
namespace mtuapptest
{
   public class MessageCenter
    {

        ServerHost _host;

        public ServerHost MessageHost
        { get{return _host;} set{_host=value;} }
        public MessageCenter()
        {
            _host = new ServerHost();
            RegistHost();
           
        }

        void _host_SendMsg(Message objMessage)
        {
            Console.WriteLine("[MessageCenter]:" + objMessage.ToString());
        }
        public void RegistHost()
        {
            _host.SendMsg += new MtuMessageHandler(_host_SendMsg);
        }


    }
}
