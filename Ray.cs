using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Floating_Horizon
{
    public class Ray
    {
        public Point3D start, direction;

        public Ray(Point3D st, Point3D end)
        {
            start = new Point3D(st);
            direction = Point3D.norm(end - st);
        }

        public Ray() { }

        public Ray(Ray r)
        {
            start = r.start;
            direction = r.direction;
        }

        //считаем отражение
        public Ray Reflect(Point3D hit_point, Point3D normal)
        {
            Point3D reflect_dir = direction - 2 * normal * Point3D.scalar(direction, normal);
            return new Ray(hit_point, hit_point + reflect_dir);
        }
        //считаем преломление
        public Ray Refract(Point3D hit_point, Point3D normal, double refraction, double k_refract)
        {
            Ray new_ray = new Ray();
            new_ray.start = new Point3D(hit_point);

            double scalar = normal.x * direction.x + normal.y * direction.y + normal.z * direction.z;
            double refract_result = refraction / k_refract;
            double theta_formula = 1 - refract_result * refract_result * (1 - scalar * scalar);
            if (theta_formula >= 0)
            {
                double cos_theta = Math.Sqrt(theta_formula);
                new_ray.direction = Point3D.norm(direction * refract_result - (cos_theta + refract_result * scalar) * normal);
                return new_ray;
            }
            else
                return null;
        }
    }

}
