using System;
using System.Windows.Forms;
using SmartHomeSystemWinForms.Forms;
using SmartHomeSystemWinForms.Controllers;
using SmartHomeSystemWinForms.Services;

namespace SmartHomeSystemWinForms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Create the advanced database service
            IAdvancedDatabaseService dbService = new AdvancedDatabaseService();
            
            // Create the advanced controller with the database service
            ISmartHomeController controller = new AdvancedSmartHomeController(dbService);
            
            // Create and run the main form with the controller
            Application.Run(new MainForm(controller));
        }
    }
}
