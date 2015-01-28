using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public class Gravity : Force
    {
        public double Amplitude { get; set; }
        public Vector2D Direction { get; set; }

        public Gravity()
        {
            this.Amplitude = 9.8;
            this.Direction = new Vector2D(0, -1);
        }
    }
}
