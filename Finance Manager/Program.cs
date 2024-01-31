using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Finance_Manager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            FMLogin login = new FMLogin();
          
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            if( login.checkLogin() != "false" ){
                Application.Run(new Dashboard());
            } else{
                Application.Run(new UserInterface());
            }
        }
    }
}
