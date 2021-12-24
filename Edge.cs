using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Floating_Horizon
{
    public class Edge
    {
        public List<Point3D> points;
        public Color color = Color.Brown;
        public Edge()
        {
            this.points = new List<Point3D> { };
            this.color = Color.Brown;
        }
        public Edge(List<Point3D> p)
        {
            this.points = p;
        }
    }
}
