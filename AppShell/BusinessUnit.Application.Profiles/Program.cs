using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessUnit.Core.IoC;

namespace BusinessUnit.Application.Profiles
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            var container = IoC.Instance.Container;
            var mainForm = container.GetExport<MainForm>();
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}
