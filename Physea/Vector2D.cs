using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physea
{
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2D v)
        {
            X = v.X;
            Y = v.Y;
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector2D Direction()
        {
            return new Vector2D(X / Math.Abs(X + Y), Y / Math.Abs(X + Y));
        }

        public Vector2D Normalized()
        {
            if (Length == 0)
            {
                return this;
            }
            else
            {
                return new Vector2D(X / Length, Y / Length);
            }
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.X, -v.Y);
        }
        public static Vector2D operator *(Vector2D v, double scalar)
        {
            return new Vector2D(v.X * scalar, v.Y * scalar);
        }
        public static Vector2D operator /(Vector2D v, double scalar)
        {
            return new Vector2D(v.X / scalar, v.Y / scalar);
        }
    }
}
