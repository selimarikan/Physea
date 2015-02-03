using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public class AirResistance : Force
    {
        public double DragCoefficient { get; set; }
        public double AirDensity { get; set; }
        public double Amplitude {
            get
            {
                return ((this.AirDensity * this.DragCoefficient * Math.Pow(this.ObjectVelocity, 2) * this.Area) / 2);// -(this.ObjectMass * 9.8);
            }
            set
            {

            }
        }
        public Vector2D Direction { get; set; }

        public double Area { get; set; }
        public double ObjectVelocity { get; set; }

        public AirResistance(double area, double velocity)
        {
            this.Direction = new Vector2D(0, 0); // should be inverse of the current movement direction
            this.DragCoefficient = 1.05; // for cube
            this.AirDensity = 1.225; // kg/m^3
            this.Area = area; // m^2
            this.ObjectVelocity = velocity; // flow velocity, relative to the object
        }
    }
}
