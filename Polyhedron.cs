using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floating_Horizon
{
    public class Polyhedron
    {
        public List<Edge> edges;
        public double[] materialK = new double[5];
        public Point3D material_color = new Point3D();
        public Polyhedron()
        {
            this.edges = new List<Edge> { };
        }
        public Polyhedron(List<Edge> e)
        {
            this.edges = e;
        }
        public bool RayIntersectsTriangle(Ray ray, Point3D p0, Point3D p1, Point3D p2, out double intersect)
        {
            var eps = 0.0001;
            Point3D edge1 = p1 - p0;
            Point3D edge2 = p2 - p0;
            Point3D h = ray.direction * edge2;
            double a = Point3D.scalar(edge1, h);
            intersect = -1;
            if (a > -eps && a < eps)
                return false;

            Point3D s = ray.start - p0;
            double u = Point3D.scalar(s, h) / a;
            if (u < 0 || u > 1)
                return false;

            Point3D q = s * edge1;
            double v = Point3D.scalar(ray.direction, q) / a;
            if (v < 0 || u + v > 1)
                return false;

            double t = Point3D.scalar(edge2, q) / a;
            if (t <= eps)
                return false;

            intersect = t;
            return true;
        }
        public virtual bool FigureIntersection(Ray r, out double intersect, out Point3D normal)
        {
            intersect = 0;
            normal = null;
            Edge side = null;
            foreach (var figure_side in edges)
            {
                //треугольная сторона
                if (figure_side.points.Count == 3)
                {
                    if (RayIntersectsTriangle(r, figure_side.points[0], figure_side.points[1], figure_side.points[2], out double t) && (intersect == 0 || t < intersect))
                    {
                        intersect = t;
                        side = figure_side;
                    }
                }

                //четырехугольная сторона
                else if (figure_side.points.Count == 4)
                {
                    if (RayIntersectsTriangle(r, figure_side.points[0], figure_side.points[1], figure_side.points[3], out double t) && (intersect == 0 || t < intersect))
                    {
                        intersect = t;
                        side = figure_side;
                    }
                    else if (RayIntersectsTriangle(r, figure_side.points[1], figure_side.points[2],  figure_side.points[3], out t) && (intersect == 0 || t < intersect))
                    {
                        intersect = t;
                        side = figure_side;
                    }
                }
            }
            if (intersect != 0)
            {
                normal = Form1.norm(side);
                material_color = new Point3D(side.color.R / 255f, side.color.G / 255f, side.color.B / 255f);
                return true;
            }
            return false;
        }

        public static Polyhedron Hex(int size)
        {
            var hc = size / 2;
            Polyhedron p = new Polyhedron();
            Edge e = new Edge();
            // 1-2-3-4
            e.points = new List<Point3D> {
                new Point3D(-hc, hc, -hc), // 1
                new Point3D(hc, hc, -hc), // 2
                new Point3D(hc, -hc, -hc), // 3
                new Point3D(-hc, -hc, -hc) // 4
            };
            p.edges.Add(e);
            e = new Edge();

            // 1-2-6-5
            e.points = new List<Point3D> {
                //new Point3(-hc, hc, -hc), // 1
                //new Point3(hc, hc, -hc), // 2
                //new Point3(hc, hc, hc), // 6 
                //new Point3(-hc, hc, hc) // 5
                new Point3D(-hc, hc, -hc), // 1
                new Point3D(-hc, hc, hc), // 5
                new Point3D(hc, hc, hc), // 6 
                new Point3D(hc, hc, -hc) // 2
            };
            p.edges.Add(e);
            e = new Edge();

            // 5-6-7-8
            e.points = new List<Point3D> {
                //new Point3(-hc, hc, hc), // 5
                //new Point3(hc, hc, hc), // 6 
                //new Point3(hc, -hc, hc), // 7
                //new Point3(-hc, -hc, hc) // 8
                new Point3D(-hc, hc, hc), // 5
                new Point3D(-hc, -hc, hc), // 8
                new Point3D(hc, -hc, hc), // 7
                new Point3D(hc, hc, hc) // 6 
            };
            p.edges.Add(e);
            e = new Edge();

            // 6-2-3-7
            e.points = new List<Point3D> {
                new Point3D(hc, hc, hc), // 6 
                new Point3D(hc, -hc, hc), // 7
                new Point3D(hc, -hc, -hc), // 3
                new Point3D(hc, hc, -hc) // 2
            };
            p.edges.Add(e);
            e = new Edge();

            // 5-1-4-8
            e.points = new List<Point3D> {
                new Point3D(-hc, hc, hc), // 5
                new Point3D(-hc, hc, -hc), // 1
                new Point3D(-hc, -hc, -hc), // 4
                new Point3D(-hc, -hc, hc) // 8
            };
            p.edges.Add(e);
            e = new Edge();

            // 4-3-7-8
            e.points = new List<Point3D> {
                new Point3D(-hc, -hc, -hc), // 4
                new Point3D(hc, -hc, -hc), // 3
                new Point3D(hc, -hc, hc), // 7
                new Point3D(-hc, -hc, hc) // 8
            };
            p.edges.Add(e);
            e = new Edge();

            return p;

        }
    }
}
