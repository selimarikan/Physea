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
            


            //Thread t = new Thread(Program.DoPhysics);
            //t.SetApartmentState(ApartmentState.MTA);
            //t.Priority = ThreadPriority.AboveNormal;
            //t.Start();

            
        
            //window.Show();

            //Force2D f1 = new Force2D(8, new Vector2D(1, 0));
            //c.Forces.Add(f1);



            app.Run(window);

        }
    }
}
