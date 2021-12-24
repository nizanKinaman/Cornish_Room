using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floating_Horizon
{
    public class Ball : Polyhedron
    {
        double radius;
        static double eps = 0.0001;
        public Ball(Point3D p, double r)
        {
            this.edges.Add(new Edge(new List<Point3D>()));
            this.edges[0].points.Add(p);
            radius = r;
        }

        public static bool RaySphereIntersection(Ray r, Point3D sphere_pos, double sphere_rad, out double t)
        {
            Point3D k = r.start - sphere_pos;
            double b = Point3D.scalar(k, r.direction);
            double c = Point3D.scalar(k, k) - sphere_rad * sphere_rad;
            double d = b * b - c;
            t = 0;
            if (d >= 0)
            {
                double sqrtd = Math.Sqrt(d);
                double t1 = -b + sqrtd;
                double t2 = -b - sqrtd;

                double min_t = Math.Min(t1, t2);
                double max_t = Math.Max(t1, t2);

                t = (min_t > eps) ? min_t : max_t;
                return t > eps;
            }
            return false;
        }

        public override bool FigureIntersection(Ray r, out double t, out Point3D normal)
        {
            t = 0;
            normal = null;
            if (RaySphereIntersection(r, edges[0].points[0], radius, out t) && (t > eps))
            {
                normal = (r.start + r.direction * t) - edges[0].points[0];
                normal = Point3D.norm(normal);
                return true;
            }
            return false;
        }
    }
}
