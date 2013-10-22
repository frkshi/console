using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using MtuConsole.ProcessManager;
using log4net;

namespace mtuapptest
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ServiceControl testSC = new ServiceControl();
            testSC.CreateInstance();
            Console.ReadLine();
            testSC.ExitInstance();

            GC.SuppressFinalize(testSC);
        }
    }
}
