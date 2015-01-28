using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public class Cube : PhysObj
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
        //public double TerminalVelocity
        //{
        //    get
        //    {
        //        //return Math.Sqrt((2 * this.Mass * 9.8)/);
        //    }
        //    set
        //    {

        //    }
        //}
        public Vector2D TotalForce { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }

        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public List<Force> Forces;

        public Cube (double len, double mass, double xpos, double ypos)
        {
            this.Width = len;
            this.Height = len;
            this.Depth = len;
            this.Mass = mass;
            this.Volume = this.Width * this.Height * this.Depth;

            this.PositionX = xpos;
            this.PositionY = ypos;

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
                else
                {
                    this.TotalForce += (f.Direction * f.Amplitude); // other in Newtons
                }
            }

            this.Velocity += this.TotalForce / this.Mass;

            this.PositionX += ((2 * t + 1) * (this.TotalForce.X / this.Mass)) / 2;
            this.PositionY += ((2 * t + 1) * (this.TotalForce.Y / this.Mass)) / 2;
            
        }
    }
}
