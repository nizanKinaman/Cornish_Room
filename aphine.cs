using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floating_Horizon
{
    class aphine
    {
        public static double[,] MultiplyMatrix(double[,] m1, double[,] m2)
        {
            double[,] m = new double[1, 4];

            for (int i = 0; i < 4; i++)
            {
                var temp = 0.0;
                for (int j = 0; j < 4; j++)
                {
                    temp += m1[0, j] * m2[j, i];
                }
                m[0, i] = temp;
            }
            return m;
        }

        public static Polyhedron move(Polyhedron poly,double posx,double posy,double posz)
        {
            Polyhedron newEdges = new Polyhedron();
            foreach (var edge in poly.edges)
            {
                Edge newPoints = new Edge();
                foreach (var point in edge.points)
                {
                    double[,] m = new double[1, 4];
                    m[0, 0] = point.x;
                    m[0, 1] = point.y;
                    m[0, 2] = point.z;
                    m[0, 3] = 1;

                    double[,] matr = new double[4, 4]
                {   { 1, 0, 0, 0},
                    { 0, 1, 0, 0 },
                    {0, 0, 1, 0 },
                    { posx, -posy, posz, 1 } };

                    var final_matrix = MultiplyMatrix(m, matr);

                    newPoints.points.Add(new Point3D(final_matrix[0, 0], final_matrix[0, 1], final_matrix[0, 2]));
                }
                newEdges.edges.Add(newPoints);

            }
            return newEdges;
        }
        public static Polyhedron rotate(Polyhedron poly,double x_angle,double y_angle,double z_angle)
        {
            Polyhedron newEdges = new Polyhedron();
            foreach (var edge in poly.edges)
            {
                Edge newPoints = new Edge();
                foreach (var point in edge.points)
                {
                    double[,] m = new double[1, 4];
                    m[0, 0] = point.x;
                    m[0, 1] = point.y;
                    m[0, 2] = point.z;
                    m[0, 3] = 1;

                    var angle = x_angle * Math.PI / 180;
                    double[,] matrx = new double[4, 4]
                {   { Math.Cos(angle), 0, Math.Sin(angle), 0},
                    { 0, 1, 0, 0 },
                    {-Math.Sin(angle), 0, Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 } };

                    angle = y_angle * Math.PI / 180;
                    double[,] matry = new double[4, 4]
                    {  { 1, 0, 0, 0 },
                    { 0, Math.Cos(angle), -Math.Sin(angle), 0},
                    {0, Math.Sin(angle), Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 } };

                    angle = z_angle * Math.PI / 180;
                    double[,] matrz = new double[4, 4]
                    {  { Math.Cos(angle), -Math.Sin(angle), 0, 0},
                    { Math.Sin(angle), Math.Cos(angle), 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 } };

                    var final_matrix = MultiplyMatrix(m, matrx);
                    final_matrix = MultiplyMatrix(final_matrix, matry);
                    final_matrix = MultiplyMatrix(final_matrix, matrz);

                    newPoints.points.Add(new Point3D(final_matrix[0, 0], final_matrix[0, 1], final_matrix[0, 2]));
                }
                newEdges.edges.Add(newPoints);
            }
            return newEdges;
        }
    }
}
