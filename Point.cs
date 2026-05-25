using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathMethods;

public struct Point
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Point()
    {
        this.X = 0;
        this.Y = 0;
        this.Z = 0;
    }
    public Point(double x, double y, double z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

}
