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
        public static List<PhysObj> PhysObjects = new List<PhysObj>();
        public static List<Shape> ObjShapes = new List<Shape>();
        public static Task<bool> ProcessingTask;
        public static bool CancelTask = false;

        public static PhysObj c;

        public static Gravity g = new Gravity();
        public static AirResistance ar = new AirResistance(0, 0);
        


        public static Force2D defaultUp = new Force2D(20, new Vector2D(0, 1));
        public static Force2D defaultDown = new Force2D(20, new Vector2D(0, 1));
        public static Force2D defaultLeft = new Force2D(20, new Vector2D(0, 1));
        public static Force2D defaultRight = new Force2D(20, new Vector2D(0, 1));


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


        bool isClicked = false;
        Point clickCoord;


        public GUI()
        {
            InitializeComponent();
            this.DataContext = this;

            // Settings
            this.Mass = 100;
            this.CubeRB.IsChecked = true;
            this.UseGravityCB.IsChecked = true;
            this.UseAirResistanceCB.IsChecked = true;
        }

        public void DoPhysics(double massOfObj, bool isCube, bool useGravity, bool useAirResistance)
        {
            double tick = 0;

            if (isCube)
            {
                PhysObjects.Add(new Cube(3, massOfObj, 0, 0));
            }
            else
            {
                PhysObjects.Add(new Sphere(1.5, massOfObj, 0, 0));
            }

            if (useGravity)
            {
                foreach (PhysObj o in PhysObjects)
                {
                    o.Forces.Add(g); // gravity
                }     
            }

            if (useAirResistance)
            {
                foreach (PhysObj o in PhysObjects)
                {
                    o.Forces.Add(ar); // air resistance
                }
            }

            while (true)
            {
                Thread.Sleep(1000 / FPS);

                if (CancelTask)
                {
                    return;
                }

                this.Dispatcher.Invoke((Action)(() =>
                {
                    // controls needs to get set per object

                    //if (Keyboard.IsKeyDown(Key.Right))
                    //{
                    //    Force2D f = new Force2D(20, new Vector2D(1, 0));
                    //    c.Forces.Add(f);
                    //}
                    //else if (Keyboard.IsKeyDown(Key.Up))
                    //{
                    //    Force2D f = new Force2D(20, new Vector2D(0, 1));
                    //    c.Forces.Add(f);
                    //}
                }));

                CalculatePhysics(tick);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Draw();
                    if (PhysObjects.Count > 0)
                    {
                        Console.WriteLine("Sec: " + tick / FPS +
                                  " X pos = " + PhysObjects[0].PositionX + "m" +
                                  " Y pos = " + PhysObjects[0].PositionY + "m" +
                                  " Fg: " + g.Amplitude * massOfObj + "N" +
                                  " Fd: " + ar.Amplitude + "N" +
                                  " V: " + PhysObjects[0].Velocity.Length + "m/s" +
                                  " VecLen: " + PhysObjects[0].TotalForce.Length +
                                  "\n");
                    }
                    
                }));
               
                

                tick++;
            }

        }

        public void CalculatePhysics(double tick)
        {
            foreach (PhysObj o in PhysObjects)
            {
                o.CalculateForces(tick / FPS);
            }
        }

        public void Draw()
        {
            MainCanvas.Children.Clear();

            foreach (PhysObj o in PhysObjects)
            {
                Type oType = o.GetType();

                if (oType == typeof(Cube))
                {
                    Rectangle r = new Rectangle();
                    r.Width = (o as Cube).Width;
                    r.Height = (o as Cube).Height;
                    Canvas.SetLeft(r, o.PositionX);
                    Canvas.SetTop(r, -o.PositionY);
                    r.Fill = Brushes.Aqua;
                    MainCanvas.Children.Add(r);
                }

                else if (oType == typeof(Sphere))
                {
                    Ellipse e = new Ellipse();
                    e.Width = (o as Sphere).Radius * 2;
                    e.Height = e.Width;
                    Canvas.SetLeft(e, o.PositionX);
                    Canvas.SetTop(e, -o.PositionY);
                    e.Fill = Brushes.Aqua;
                    MainCanvas.Children.Add(e);
                }

                // UI Values, needs solution for mult objs
                Weight = Math.Truncate(o.Mass * g.Amplitude * 100) / 100;
                Drag = Math.Truncate(ar.Amplitude * 100) / 100;
                Velocity = Math.Truncate(o.Velocity.Length * 100) / 100;
                XPosition = Math.Truncate(o.PositionX * 100) / 100;
                YPosition = Math.Truncate(o.PositionY * 100) / 100;
            }

            
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            CancelTask = false;

            ProcessingTask = Task<bool>.Factory.StartNew(() =>
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

                if (CancelTask)
                {
                    return false;
                }

                DoPhysics(mass, (bool)isCube, (bool)useGravity, (bool)useAirResistance);
                return true;
            });
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //isClicked = true;
            //clickCoord = e.GetPosition(this);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if (isClicked)
            //{

            //}
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //isClicked = false;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e) // global key grab??
        {

        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            CancelTask = true;
            PhysObjects.Clear();
            MainCanvas.Children.Clear();
        }
    }
}
