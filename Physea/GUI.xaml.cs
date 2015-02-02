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
        public static int FPS = 60;

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

        public double Mass
        {
            get { return (double)GetValue(MassProperty); }
            set { SetValue(MassProperty, value); }
        }
        public static readonly DependencyProperty MassProperty =
            DependencyProperty.Register("Mass", typeof(double), typeof(GUI), null);

        


        public GUI()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Mass = 100;
            this.CubeRB.IsChecked = true;
            this.UseGravityCB.IsChecked = true;
            this.UseAirResistanceCB.IsChecked = true;
        }

        public void DoPhysics(double massOfObj, bool isCube, bool useGravity, bool useAirResistance)
        {

            //int objMass = 1000;

            Gravity g = new Gravity();
            AirResistance ar = new AirResistance(0, 0, massOfObj);
            PhysObj c;
            double tick = 0;


            if (isCube)
            {
                c = new Cube(3, massOfObj, 0, 0); // 
            }
            else
            {
                c = new Sphere(1.5, massOfObj, 0, 0);
            }

            if (useGravity)
            {
                c.Forces.Add(g); // gravity
            }
            if (useAirResistance)
            {
                c.Forces.Add(ar); // air resistance
            }

            while (true)
            {
                Thread.Sleep(1000 / FPS);

                c.CalculateForces(tick / FPS);



                

                Console.WriteLine("Sec: " + tick / FPS +
                                  " X pos = " + c.PositionX + "m" +
                                  " Y pos = " + c.PositionY + "m" +
                                  " Fg: " + g.Amplitude * massOfObj + "N" +
                                  " Fd: " + ar.Amplitude + "N" +
                                  " V: " + c.Velocity.Length + "m/s" +
                                  " VecLen: " + c.TotalForce.Length +
                                  "\n");

                this.Dispatcher.Invoke((Action)(() =>
                {
                    Weight = Math.Truncate(c.Mass * g.Amplitude * 100) / 100;
                    Drag = Math.Truncate(ar.Amplitude * 100) / 100;
                    Velocity = Math.Truncate(c.Velocity.Length * 100) / 100;
                    XPosition = Math.Truncate(c.PositionX * 100) / 100;
                    YPosition = Math.Truncate(c.PositionY * 100) / 100;

                    if (isCube)
                    {
                        double x = c.PositionX;
                        double y = -c.PositionY;

                        double w = (c as Cube).Width;
                        double h = (c as Cube).Height;

                        Canvas.SetLeft(Cube, x);
                        Canvas.SetTop(Cube, y);
                        Cube.Width = w;
                        Cube.Height = h;
                    }
                    else
                    {
                        double x = c.PositionX;
                        double y = -c.PositionY;
                        double w = (c as Sphere).Radius;
                        double h = (c as Sphere).Radius;

                        Canvas.SetLeft(Sphere, x);
                        Canvas.SetTop(Sphere, y);
                        Sphere.Width = w * 2;
                        Sphere.Height = h * 2;
                    }
                   
                }));

                tick++;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task<bool>.Factory.StartNew(() =>
            {
                double mass = 0;
                bool? isCube = false;
                bool? useGravity = true;
                bool? useAirResistance = true;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    mass = Mass;
                    isCube = CubeRB.IsChecked;
                    useGravity = UseGravityCB.IsChecked;
                    useAirResistance = UseAirResistanceCB.IsChecked;
                }));

                DoPhysics(mass, (bool)isCube, (bool)useGravity, (bool)useAirResistance);
                return true;
            });
        }
    }
}
