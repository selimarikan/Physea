using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public class Force2D : Force
    {
        public double Amplitude { get; set; }
        public Vector2D Direction { get; set; }

        public Force2D(double Amp, Vector2D Dir) // dir must be normalized
        {
            this.Amplitude = Amp;
            this.Direction = Dir;
        }
    }
}
