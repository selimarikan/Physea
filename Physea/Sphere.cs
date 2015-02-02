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
            this.Radius = r;
            this.Mass = mass;
            this.PositionX = xpos;
            this.PositionY = ypos;
            this.Volume = Math.Pow(this.Radius, 3) * Math.PI * 4 / 3;

            this.Forces = new List<Force>();
            this.TotalForce = new Vector2D(0, 0);
            this.Velocity = new Vector2D(0, 0);
        }

        public void CalculateForces(double t)
        {
            this.TotalForce.X = 0; this.TotalForce.Y = 0;

            foreach (Force f in this.Forces) // merge all applied forces into one vector, then use it as acceleration
            {
                if (f.GetType() == typeof(Gravity))
                {
                    this.TotalForce += ((f.Direction * f.Amplitude) * this.Mass); // G in Newtons
                }
                else if (f.GetType() == typeof(AirResistance))
                {
                    var ar = (f as AirResistance);
                    ar.DragCoefficient = 0.47;
                    ar.ObjectVelocity = this.Velocity.Length;
                    ar.Direction = -(this.TotalForce.Normalized());
                    ar.Area = Math.Pow(this.Radius, 2) * Math.PI;
                    this.TotalForce += (f.Direction * f.Amplitude); // other in Newtons
                }
                else
                {
                    this.TotalForce += (f.Direction * f.Amplitude); // other in Newtons
                }
            }

            // http://en.wikipedia.org/wiki/Equations_for_a_falling_body

            if (t != 0)
            {
                this.Velocity += (this.TotalForce / this.Mass) / GUI.FPS;
                // 2
                //this.PositionX += ((TotalForce.X / Mass) * t) - (Velocity.X / (2 * t));
                //this.PositionY += ((TotalForce.Y / Mass) * t) - (Velocity.Y / (2 * t));

                this.PositionX += (Velocity.X - (TotalForce.X / (Mass * 2))) / GUI.FPS;
                this.PositionY += (Velocity.Y - (TotalForce.Y / (Mass * 2))) / GUI.FPS;
            }
        }
    }
}
