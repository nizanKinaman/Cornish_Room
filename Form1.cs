using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Floating_Horizon
{
    public partial class Form1 : Form
    {
        static Bitmap bmp;
        List<Polyhedron> All_figures = new List<Polyhedron>();
        public int height, width;
        public Color[,] colors;
        public Point3D cameraPoint = new Point3D();
        public List<Light> lights = new List<Light>();
        public Point3D[,] pixels;
        int edge_num = 5;
        public Form1()
        {
            InitializeComponent();
            height = pictureBox1.Height;
            width = pictureBox1.Width;
            bmp = new Bitmap(width, height);
            colors = new Color[width, height];
            pixels = new Point3D[width, height];
            Make_Ray_Tracing();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(width, height);
            colors = new Color[width, height];
            pixels = new Point3D[width, height];
            cameraPoint = new Point3D();
            lights = new List<Light>();
            All_figures = new List<Polyhedron>();
            Make_Ray_Tracing();
        }

        public void Make_Ray_Tracing()
        {
            Set_figures();

            Create_pixel_map();
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {
                    Ray r = new Ray(cameraPoint, pixels[i, j]);
                    r.start = new Point3D(pixels[i, j]);
                    Point3D color = RayTrace(r, 10, 1);//луч,кол-во итераций,коэфф
                    if (color.x > 1.0f || color.y > 1.0f || color.z > 1.0f)
                        color = Point3D.norm(color);
                    colors[i, j] = Color.FromArgb((int)(255 * color.x), (int)(255 * color.y), (int)(255 * color.z));
                }

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    bmp.SetPixel(i, j, colors[i, j]);
            pictureBox1.Image = bmp;
        }
        public void Create_pixel_map()
        {
            var points = All_figures[edge_num].edges[0].points;
            Point3D step_up = (points[1] - points[0]) / (width - 1);
            Point3D step_down = (points[2] - points[3]) / (width - 1);
            Point3D up = new Point3D(points[0]);
            Point3D down = new Point3D(points[3]);

            for (int i = 0; i < width; ++i)
            {
                Point3D step_y = (up - down) / (height - 1);
                Point3D d = new Point3D(down);
                for (int j = 0; j < height; ++j)
                {
                    pixels[i, j] = d;
                    d += step_y;
                }
                up += step_up;
                down += step_down;
            }
        }

        public void Add_wall(int wall_number, Color color, bool is_reflection)
        {
            Polyhedron wall1 = new Polyhedron(new List<Edge>() { Polyhedron.Hex(10).edges[wall_number] });
            wall1.edges[0].color = color;
            if (is_reflection)
                wall1.materialK = new double[] { 1, 0, 0.05, 0.7, 1 };
            else
                wall1.materialK = new double[] { 0, 0, 0.05, 0.7, 1 };

            All_figures.Add(wall1);
        }
        public void Set_figures()
        {
            Add_wall(0, Color.Green, false);//пол
            Add_wall(1, Color.White, checkBox7.Checked);//передняя
            Add_wall(2, Color.White, false);//потолок
            Add_wall(3, Color.Blue, checkBox9.Checked);//правая
            Add_wall(4, Color.Red, checkBox8.Checked);//левая
            Add_wall(5, Color.White, checkBox10.Checked);//задняя
            cameraPoint = new Point3D(0, -25, 0);

            Light light1 = new Light(new Point3D(0f, -3f, 1.9f), new Point3D(1f, 1f, 1f));
            Light light2 = new Light(new Point3D(double.Parse(textBox1.Text), double.Parse(textBox3.Text) - 3f, double.Parse(textBox2.Text) + 3.9f), new Point3D(1f, 1f, 1f));
            if (checkBox5.Checked) lights.Add(light1);
            if (checkBox6.Checked) lights.Add(light2);

            //первый куб
            Polyhedron cube = Polyhedron.Hex(3);
            cube.edges = aphine.rotate(cube, 0, 0, 20).edges;
            cube.edges = aphine.move(cube, 3, 0, -4).edges;
            cube.materialK = new double[] { Convert.ToInt32(checkBox1.Checked), 0, 0.1, 0.7, 1.5 };

            //cube.material_color = new Point3D( Color.Red.R,Color.Red.G,Color.Red.B);
            foreach (var x in cube.edges)
                x.color = Color.White;
            All_figures.Add(cube);

            //второй куб
            Polyhedron cube2 = Polyhedron.Hex(5);
            cube2.edges = aphine.rotate(cube2, 0, 0, 20).edges;
            cube2.edges = aphine.move(cube2, -3, 0, -4).edges;
            if (checkBox3.Checked)
                cube2.materialK = new double[] { 0, 0.7f, 0.1f, 0.5f, 1.05f };
            else
                cube2.materialK = new double[] { 0, 0, 0.1, 0.7, 1.5 };

            //cube2.material_color = new Point3D( Color.Red.R,Color.Red.G,Color.Red.B);
            foreach (var x in cube2.edges)
                x.color = Color.Red;
            All_figures.Add(cube2);

            //первый шар
            Ball ball_mirror = new Ball(new Point3D(2.5f, 2, 0f), 2f);
            ball_mirror.materialK = new double[] { Convert.ToInt32(checkBox2.Checked), 0f, 0.1f, 0f, 1f };
            foreach (var x in ball_mirror.edges)
                x.color = Color.Green;
            ball_mirror.material_color = new Point3D(Color.Green.R, Color.Green.G, Color.Green.B);
            All_figures.Add(ball_mirror);

            //второй шар
            Ball ball_mirror2 = new Ball(new Point3D(2.5f, 2, 0f), 1.5f);
            ball_mirror2.edges = aphine.move(ball_mirror, 0, 4, -2).edges;
            if (checkBox4.Checked)
                ball_mirror2.materialK = new double[] { 0f, 0.9f, 0.1f, 0.5f, 1.05f };
            else
                ball_mirror2.materialK = new double[] { 0f, 0f, 0.1f, 0f, 1f };
            foreach (var x in ball_mirror2.edges)
                x.color = Color.Violet;
            ball_mirror2.material_color = new Point3D(Color.Violet.R, Color.Violet.G, Color.Violet.B);
            All_figures.Add(ball_mirror2);
        }
        public static Point3D norm(Edge S)
        {
            if (S.points.Count() < 3)
                return new Point3D(0, 0, 0);
            Point3D U = S.points[1] - S.points[0];
            Point3D V = S.points[S.points.Count - 1] - S.points[0];
            Point3D normal = U * V;
            return Point3D.norm(normal);
        }

        public Point3D RayTrace(Ray r, int iter, double env)
        {
            if (iter <= 0)
                return new Point3D(0, 0, 0);
            double shortest_intersect = 0;//точка пересечения,ближайшая
            Point3D normal = null;
            double[] materialK = new double[5];
            //отражение,  преломление, фоновое освещение, диффузное освещение, преломление среды
            //reflection, refraction,  ambient,           diffuse,             envoironment
            Point3D material_color = new Point3D();
            Point3D res_color = new Point3D(0, 0, 0);
            bool is_sharp = false;

            foreach (var fig in All_figures)
            {
                if (fig.FigureIntersection(r, out double intersect, out Point3D norm))
                    if (intersect < shortest_intersect || shortest_intersect == 0)
                    {
                        shortest_intersect = intersect;
                        normal = norm;
                        materialK = fig.materialK;
                        material_color = fig.material_color;
                    }
            }

            if (shortest_intersect == 0)//если не пересекается с фигурой
                return new Point3D(0, 0, 0);

            if (Point3D.scalar(r.direction, normal) > 0)
            {
                normal *= -1;
                is_sharp = true;
            }

            //Точка пересечения луча с фигурой
            Point3D hit_point = r.start + r.direction * shortest_intersect;

            foreach (Light light in lights)
            {
                Point3D ambient_coef = light.color_light * materialK[2];
                ambient_coef.x = (ambient_coef.x * material_color.x);
                ambient_coef.y = (ambient_coef.y * material_color.y);
                ambient_coef.z = (ambient_coef.z * material_color.z);
                res_color += ambient_coef;
                // диффузное освещение
                if (IsVisible(light.start_point, hit_point))
                    res_color += light.Shade(hit_point, normal, material_color, materialK[3]);
                else
                    res_color += light.Shade(hit_point, normal, material_color, materialK[3]) / 5 * 3;
            }

            if (materialK[0] > 0)
            {
                Ray reflected_ray = r.Reflect(hit_point, normal);
                res_color = materialK[0] * RayTrace(reflected_ray, iter - 1, env);
            }

            if (materialK[1] > 0)
            {
                double refract_coef;
                if (is_sharp)
                    refract_coef = materialK[4];
                else
                    refract_coef = 1 / materialK[4];

                Ray refracted_ray = r.Refract(hit_point, normal, materialK[1], refract_coef);//преломленный луч

                if (refracted_ray != null)
                    res_color = materialK[1] * RayTrace(refracted_ray, iter - 1, materialK[4]);
            }
            return res_color;
        }
        public bool IsVisible(Point3D light_point, Point3D hit_point)
        {
            double max_t = (light_point - hit_point).length(); //позиция источника света на луче
            Ray r = new Ray(hit_point, light_point);
            foreach (var fig in All_figures)
                if (fig.FigureIntersection(r, out double t, out Point3D n))
                    if (t < max_t && t > 0.0001)
                        return false;
            return true;
        }
    }
}
