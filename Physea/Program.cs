using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Physea
{
    public class Program
    {
        public const int FPS = 1;
        public const int objMass = 1000;

        static void Main(string[] args)
        {
            Gravity g = new Gravity();
            AirResistance ar = new AirResistance(0, 0, objMass);

            Cube c = new Cube(1, objMass, 0, 0); // 1m, 1000kg, 0x, 0y
            c.Forces.Add(g); // gravity
            c.Forces.Add(ar); // air resistance

            double tick = 0;


            //Force2D f1 = new Force2D(8, new Vector2D(1, 0));
            //c.Forces.Add(f1);

            while (true)
            {
                Thread.Sleep(1000 / FPS);

                ar.ObjectVelocity = c.Velocity.Length; // plus wind speed\ needs to calculate automatic direction based on vectors
                ar.Direction = -(c.TotalForce.Normalized()); // needs to get direction from all of the forces except itself!!!
                ar.Area = c.Volume / c.Depth;

                c.CalculateForces(tick / FPS);
                








                Console.WriteLine("Sec: " + tick / FPS +  
                                  " X pos = " + c.PositionX + 
                                  " Y pos = " + c.PositionY +
                                  " Fg: " + c.Forces[0].Amplitude * objMass + "N" +
                                  " Fd: " + c.Forces[1].Amplitude + "N" +
                                  " V: " + ar.ObjectVelocity +
                                  " VecLen: " + c.TotalForce.Length +
                                  "\n");
                tick++;

                //using (var file = new System.IO.StreamWriter(@"C:\physea.txt", true))  
                //{
                //    file.WriteLine(seconds + ", " + c.PositionY);
                //    file.Close();
                //}
            }

        }
    }
}
