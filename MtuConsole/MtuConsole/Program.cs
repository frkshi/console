using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;//.Tasks;
using System.Windows.Forms;

namespace MtuConsole
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new MainParent());

            using (IApplication app = new App())
            {
                //app.GetService<IViewService>().Navigate(Views.Main);

                Application.Run(new MainParent());
                System.Environment.Exit(0);
            }
        }
    }
}
