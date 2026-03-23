using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetrahedron_Sierpinski
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model3DGroup group = new Model3DGroup();
        DispatcherTimer timer;
        bool isAuto = false;
        int level;
        int currentLevel = 0;
        double scale = 0.1;
        double angleX = 0;
        double angleY = 0;
        double distance = 10;

        Point lastMouse;
        bool isDragging = false;

        public MainWindow()
        {
            InitializeComponent();

            ModelVisual3D model = new ModelVisual3D();
            model.Content = group;
            viewport.Children.Add(model);

            AddLight();
            StartAnimation();
        }

        void AddLight()
        {
            group.Children.Add(new AmbientLight(Colors.LightGray));
        }

        void StartAnimation()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            group.Children.Clear();
            AddLight();

            //int level = (int)levelSlider.Value;


            if (isAuto)
            {
                level = currentLevel;

                if (scale >= 1)
                {
                    currentLevel++;
                    scale = 0.1;

                    if (currentLevel > (int)levelSlider.Maximum)
                        currentLevel = 0;
                }

                levelSlider.Value = level;
            }
            else
            {
                level = (int)levelSlider.Value;
            }

            scale += 0.02;
            if (scale > 1) scale = 1;

            double size = 4 * scale;
            DrawSierpinski(level, new Point3D(-size / 2, -size / 2, -size / 2), size, level);

            UpdateCamera();
        }

        void UpdateCamera()
        {
            double x = distance * Math.Cos(angleY) * Math.Cos(angleX);
            double y = distance * Math.Sin(angleX);
            double z = distance * Math.Sin(angleY) * Math.Cos(angleX);

            camera.Position = new Point3D(x, y, z);
            camera.LookDirection = new Vector3D(-x, -y, -z);
        }

        void DrawSierpinski(int lvl, Point3D pos, double size, int originalLevel)
        {
            if (lvl == 0)
            {
                AddTetrahedron(pos, size, originalLevel);
                return;
            }

            size /= 2;

            DrawSierpinski(lvl - 1, pos, size, originalLevel);
            DrawSierpinski(lvl - 1, new Point3D(pos.X + size, pos.Y, pos.Z), size, originalLevel);
            DrawSierpinski(lvl - 1, new Point3D(pos.X + size / 2, pos.Y + size, pos.Z), size, originalLevel);
            DrawSierpinski(lvl - 1, new Point3D(pos.X + size / 2, pos.Y + size / 2, pos.Z + size), size, originalLevel);
        }

        void AddTetrahedron(Point3D p, double s, int depth)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            //punkty tetrahedronu
            Point3D p0 = p;
            Point3D p1 = new Point3D(p.X + s, p.Y, p.Z);
            Point3D p2 = new Point3D(p.X + s / 2, p.Y + s, p.Z);
            Point3D p3 = new Point3D(p.X + s / 2, p.Y + s / 2, p.Z + s);

            // dodajemy punkty do siatki
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.Positions.Add(p3);

            // definiujemy trójkąty (4 ściany tetrahedronu)
            mesh.TriangleIndices = new Int32Collection {
                0,1,2,
                0,1,3,
                1,2,3,
                0,2,3
            };
            // kolor zależny od poziomu dzielenia
            Color color = Color.FromRgb(
                (byte)(50 + depth * 30),
                (byte)(100),
                (byte)(200 - depth * 30)
            );
            // materiał i model 3D
            var material = new DiffuseMaterial(new SolidColorBrush(color));
            var model = new GeometryModel3D(mesh, material);
            model.BackMaterial = material;

            group.Children.Add(model);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                lastMouse = e.GetPosition(this);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || e.LeftButton != MouseButtonState.Pressed)
                return;

            Point current = e.GetPosition(this);
            double dx = current.X - lastMouse.X;
            double dy = current.Y - lastMouse.Y;

            angleY += dx * 0.01;
            angleX += dy * 0.01;

            lastMouse = current;
            UpdateCamera();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            distance -= e.Delta * 0.01;
            if (distance < 3) distance = 3;
            if (distance > 50) distance = 50;
            UpdateCamera();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            isAuto = true;
            //currentLevel = 0;
            scale = 0.1;
            timer.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            isAuto = false;
            //timer.Stop();
        }
    }
}