using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RadioManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "/version":
                        System.Console.WriteLine("v0.03");
                        break;

                    default:
                        System.Console.WriteLine("RadioManager - v0.03");
                        break;
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new RadioManager());
            }
        }
    }
}
