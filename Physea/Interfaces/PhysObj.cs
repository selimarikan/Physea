using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public interface PhysObj // SI
    {
        double Mass { get; set; }       // kg
        double Volume { get; set; }     // m
        double Density { get; }         // kg/m^3
        double PositionX { get; set; }
        double PositionY { get; set; }
        Vector2D TotalForce { get; set; }
        Vector2D Velocity { get; set; }
    }
}
