using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PanicHandler
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Directory.SetCurrentDirectory(executionPath);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PanicHandler());
        }
    }
}