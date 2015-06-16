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
            Amplitude = 9.8;
            Direction = new Vector2D(0, -1);
        }
    }
}
