using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floating_Horizon
{
    public class Point3D
    {
        public double x;
        public double y;
        public double z;

        public Point3D() { x = 0; y = 0; z = 0; }

        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3D(Point3D p)
        {
            if (p == null)
                return;
            x = p.x;
            y = p.y;
            z = p.z;
        }

        public double length()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static Point3D operator -(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);

        }

        public static double scalar(Point3D p1, Point3D p2)
        {
            return p1.x * p2.x + p1.y * p2.y + p1.z * p2.z;
        }

        public static Point3D norm(Point3D p)
        {
            double z = Math.Sqrt(p.x * p.x + p.y * p.y + p.z * p.z);
            return z == 0 ? new Point3D(p) : new Point3D(p.x / z, p.y / z, p.z / z);
        }

        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);

        }

        public static Point3D operator *(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.y * p2.z - p1.z * p2.y, p1.z * p2.x - p1.x * p2.z, p1.x * p2.y - p1.y * p2.x);
        }

        public static Point3D operator *(double t, Point3D p1)
        {
            return new Point3D(p1.x * t, p1.y * t, p1.z * t);
        }


        public static Point3D operator *(Point3D p1, double t)
        {
            return new Point3D(p1.x * t, p1.y * t, p1.z * t);
        }

        public static Point3D operator -(Point3D p1, double t)
        {
            return new Point3D(p1.x - t, p1.y - t, p1.z - t);
        }

        public static Point3D operator -(double t, Point3D p1)
        {
            return new Point3D(t - p1.x, t - p1.y, t - p1.z);
        }

        public static Point3D operator +(Point3D p1, double t)
        {
            return new Point3D(p1.x + t, p1.y + t, p1.z + t);
        }

        public static Point3D operator +(double t, Point3D p1)
        {
            return new Point3D(p1.x + t, p1.y + t, p1.z + t);
        }

        public static Point3D operator /(Point3D p1, double t)
        {
            return new Point3D(p1.x / t, p1.y / t, p1.z / t);
        }

        public static Point3D operator /(double t, Point3D p1)
        {
            return new Point3D(t / p1.x, t / p1.y, t / p1.z);
        }
    }
}
