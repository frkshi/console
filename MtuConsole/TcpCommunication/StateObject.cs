using System;
using System.Net.Sockets;
using System.Text;
namespace MtuConsole.TcpCommunication
{
    /// <summary>
    /// StateObject 的摘要说明。
    /// </summary>
    public class StateObject
    {
        public NetworkStream WorkStream = null;
        // Client socket.
        public Socket WorkSocket = null;
        // Size of receive buffer.
        public static int BufferSize = 5120;
        // Receive buffer.
        public byte[] Buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
        public string RtuId;
        //远端ip
        public string RemoteIp;

        public DateTime BeginTime;
        public StateObject()
        {
            WorkStream = null;
            WorkSocket = null;

            //sb = null;
        }
    }

    //  public class 

}