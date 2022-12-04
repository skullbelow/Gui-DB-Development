using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestApp.Control;

namespace TestApp
{
    static class Controller
    {

        /*Startup: This encompasses the initial set of tasks that the system needs to perform once it
        is started. For instance, loading up and displaying an initial set of data, or performing initial
        application configurations */

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DBConnector.InitializeDB();
            Application.Run(new Login());
        }
    }
}