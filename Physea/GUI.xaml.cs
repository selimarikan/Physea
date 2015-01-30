using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Physea
{
    public partial class GUI : UserControl
    {


        public double Weight
        {
            get { return (double)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }
        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(double), typeof(GUI), null);

        public double Drag
        {
            get { return (double)GetValue(DragProperty); }
            set { SetValue(DragProperty, value); }
        }
        public static readonly DependencyProperty DragProperty =
            DependencyProperty.Register("Drag", typeof(double), typeof(GUI), null);

        public double Velocity
        {
            get { return (double)GetValue(VelocityProperty); }
            set { SetValue(VelocityProperty, value); }
        }
        public static readonly DependencyProperty VelocityProperty =
            DependencyProperty.Register("Velocity", typeof(double), typeof(GUI), null);



        public double XPosition
        {
            get { return (double)GetValue(XPositionProperty); }
            set { SetValue(XPositionProperty, value); }
        }
        public static readonly DependencyProperty XPositionProperty =
            DependencyProperty.Register("XPosition", typeof(double), typeof(GUI), null);



        public double YPosition
        {
            get { return (double)GetValue(YPositionProperty); }
            set { SetValue(YPositionProperty, value); }
        }
        public static readonly DependencyProperty YPositionProperty =
            DependencyProperty.Register("YPosition", typeof(double), typeof(GUI), null);

        


        public GUI()
        {
            InitializeComponent();
            this.DataContext = this;

            
        }

        public void DoPhysics()
        {
            int FPS = 1;
            int objMass = 1000;

            Gravity g = new Gravity();
            AirResistance ar = new AirResistance(0, 0, objMass);

            Cube c = new Cube(1, objMass, 0, 0); // 1m, 1000kg, 0x, 0y
            c.Forces.Add(g); // gravity
            //c.Forces.Add(ar); // air resistance

            double tick = 1;

            

            while (true)
            {
                Thread.Sleep(1000 / FPS);

                c.CalculateForces(tick / FPS);



                

                Console.WriteLine("Sec: " + tick / FPS +
                                  " X pos = " + c.PositionX +
                                  " Y pos = " + c.PositionY +
                                  " Fg: " + c.Forces[0].Amplitude * objMass + "N" +
                          //        " Fd: " + c.Forces[1].Amplitude + "N" +
                                  " V: " + c.Velocity.Length +
                                  " VecLen: " + c.TotalForce.Length +
                                  "\n");

                this.Dispatcher.Invoke((Action)(() =>
                {
                    Weight = Math.Truncate(c.Mass * g.Amplitude * 100) / 100;
                    //Drag = Math.Truncate(c.Forces[1].Amplitude * 100) / 100;
                    Velocity = Math.Truncate(c.Velocity.Length * 100) / 100;
                    XPosition = Math.Truncate(c.PositionX * 100) / 100;
                    YPosition = Math.Truncate(c.PositionY * 100) / 100;

                    double x = c.PositionX;
                    double y = -c.PositionY;

                    double w = c.Width;
                    double h = c.Height;

                    Canvas.SetLeft(Cube, x);
                    Canvas.SetTop(Cube, y);
                    Cube.Width = w;
                    Cube.Height = h;
                }));
                
                tick++;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task<bool>.Factory.StartNew(() =>
            {
                DoPhysics();
                return true;
            });
        }
    }
}
