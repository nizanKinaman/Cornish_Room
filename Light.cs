using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floating_Horizon
{
    public class Light
    {
        public Point3D start_point;
        public Point3D color_light;

        public Light(Point3D p, Point3D c)
        {
            start_point = new Point3D(p);
            color_light = new Point3D(c);
        }

        public Point3D Shade(Point3D hit_point, Point3D normal, Point3D material_color, double diffuse_coef)
        {
            Point3D dir = start_point - hit_point;
            var p = 0;
            dir = Point3D.norm(dir);
            Point3D diff;
            if (Point3D.scalar(normal, dir) > 0)
            diff = diffuse_coef * color_light * Point3D.scalar(normal, dir);
            else
                diff = diffuse_coef * color_light * 0.5;

            //diff = diffuse_coef * color_light * Math.Max(Point3D.scalar(normal, dir), 0.5);
            //if (diff.x == 0)
            //    p = 0;
            return new Point3D(diff.x * material_color.x, diff.y * material_color.y, diff.z * material_color.z);
        }
    }
}
