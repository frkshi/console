using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;

namespace MtuConsole.Common
{


    public interface ITcpListener
    {

        event CommnicationMessageHandler MessageArrived;
        event MessageSentHandler MessageSent;

        void MultiDataCheck();
        bool SetKey { set; }
        bool SetNeepReply { set; }
        int SetAddDay { set; }
        int SetAddSecond { set; }
        void StartListen();
        void CreateInstance(int portid, IResponseMessage objmsg);
        void ExitInstance();
        int Add2SendList(string rtuid, string content, string commandtype);
        void SetCheckTimeList(List<string> rtuList);


    }

    public class FactoryTcpListener
    {
        public ITcpListener CreateTcpListener(string sDllName, string sObjectName)
        {


            return (ITcpListener)CommonMethod.CreatePkgObject(sDllName, sObjectName);

        }




    }

}
