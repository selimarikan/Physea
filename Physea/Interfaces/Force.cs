using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public interface Force
    {
        double Amplitude { get; set; }
        Vector2D Direction { get; set; }
    }
}
