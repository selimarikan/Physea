using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public class Sphere : PhysObj
    {
        public double Mass { get; set; }
        public double Volume { get; set; }
        public double Density
        {
            get
            {
                return Mass / Volume;
            }
        }
        public Vector2D Velocity { get; set; }
        public Vector2D TotalForce { get; set; }

        public double Radius { get; set; }

        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public List<Force> Forces { get; set; }


        public Sphere(double r, double mass, double xpos, double ypos)
        {
            Radius = r;
            Mass = mass;
            PositionX = xpos;
            PositionY = ypos;
            Volume = Math.Pow(this.Radius, 3) * Math.PI * 4 / 3;

            Forces = new List<Force>();
            TotalForce = new Vector2D(0, 0);
            Velocity = new Vector2D(0, 0);
        }

        public void CalculateForces(double t)
        {
            TotalForce.X = 0; TotalForce.Y = 0;

            foreach (Force f in Forces) // merge all applied forces into one vector, then use it as acceleration
            {
                if (f.GetType() == typeof(Gravity))
                {
                    TotalForce += ((f.Direction * f.Amplitude) * Mass); // G in Newtons
                }
                else if (f.GetType() == typeof(AirResistance))
                {
                    var ar = (f as AirResistance);
                    ar.DragCoefficient = 0.47;
                    ar.ObjectVelocity = Velocity.Length;
                    ar.Direction = -(TotalForce.Normalized());
                    ar.Area = Math.Pow(Radius, 2) * Math.PI;
                    TotalForce += (f.Direction * f.Amplitude); // other in Newtons
                }
                else
                {
                    TotalForce += (f.Direction * f.Amplitude); // other in Newtons
                }
            }

            // http://en.wikipedia.org/wiki/Equations_for_a_falling_body

            if (t != 0)
            {
                Velocity += (TotalForce / Mass) / GUI.FPS;
                // 2
                //this.PositionX += ((TotalForce.X / Mass) * t) - (Velocity.X / (2 * t));
                //this.PositionY += ((TotalForce.Y / Mass) * t) - (Velocity.Y / (2 * t));

                PositionX += (Velocity.X - (TotalForce.X / (Mass * 2))) / GUI.FPS;
                PositionY += (Velocity.Y - (TotalForce.Y / (Mass * 2))) / GUI.FPS;
            }
        }
    }
}
