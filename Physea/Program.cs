using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Physea
{

    public class Program
    {
        public static volatile Window window;
        static readonly object locker = new object();

        [STAThread]
        static void Main(string[] args)
        {
            Application app = new Application();

            window = new Window();
            window.Title = "Physea";
            window.Content = new GUI();

            app.Run(window);
        }
    }
}
