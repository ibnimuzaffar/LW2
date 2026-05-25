using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathMethods;

public struct Sphere
{
    public Point Center { get; }
    public double Radius { get; }

    public Sphere()
    {
        this.Center = new Point();
        this.Radius = 0;
    }

    public Sphere(Point center, double radius)
    {
        this.Center = center;
        this.Radius = radius;
    }

    public bool IsPointIn(Point point)
    {
        if ( Math.Pow((point.X - Center.X),2) + Math.Pow((point.Y - Center.Y), 2) + Math.Pow((point.Z - Center.Z), 2) < Radius * Radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Point> GetInnerPoints(List<Point> points)
    {
        List<Point> result = new List<Point>();

        foreach (Point point in points)
        {
            if (this.IsPointIn(point)) result.Add(point);
        }

        return result;
    }
}
