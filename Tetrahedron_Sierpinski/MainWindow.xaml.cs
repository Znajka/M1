using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Tetrahedron_Sierpinski
{
    public partial class MainWindow : Window
    {
        // deklaracja zmiennych
        Model3DGroup group = new Model3DGroup();
        DispatcherTimer timer;

        AxisAngleRotation3D axisRotationX = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
        AxisAngleRotation3D axisRotationY = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);

        bool isAuto = false;
        bool autoRotate = true;
        double distance = 20;
        int autoBuildCounter = 0;
        Point lastMouse;
        bool isDragging = false;

        public MainWindow()
        {
            InitializeComponent();
            // ustawienia kamery i światła
            Transform3DGroup transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(new RotateTransform3D(axisRotationX));
            transformGroup.Children.Add(new RotateTransform3D(axisRotationY));
            // dodanie modelu do viewportu
            group.Transform = transformGroup;
            ModelVisual3D modelVisual = new ModelVisual3D { Content = group };
            viewport.Children.Add(modelVisual);
            // inicjalizacja modelu i animacji
            RefreshModel(0);
            StartAnimation();
        }

        // metoda uruchamiająca animację
        void StartAnimation()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        // metoda obsługująca zdarzenie tick timera
        void Timer_Tick(object sender, EventArgs e)
        {
            // automatyczna rotacja
            if (autoRotate)
            {
                axisRotationY.Angle = (axisRotationY.Angle + 1.2) % 360;
                axisRotationX.Angle = (axisRotationX.Angle + 0.6) % 360;
            }
            // automatyczne budowanie poziomów
            if (isAuto)
            {
                autoBuildCounter++;
                if (autoBuildCounter > 40) 
                {
                    if (levelSlider.Value < levelSlider.Maximum)
                    {
                        levelSlider.Value += 1;
                    }
                    else
                    {
                        isAuto = false;
                    }
                    autoBuildCounter = 0;
                }
            }
        }
        // metoda obsługująca zmianę wartości slider'a poziomu
        void LevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                RefreshModel((int)e.NewValue);
        }
        // metoda odświeżająca model na podstawie wybranego poziomu
        void RefreshModel(int lvl)
        {
            group.Children.Clear();
            group.Children.Add(new AmbientLight(Colors.Gray));
            group.Children.Add(new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1)));

            double size = 5;
            DrawSierpinski(lvl, new Point3D(-size / 2, -size / 2, -size / 2), size, lvl);
        }
        // metoda rekurencyjna rysująca fraktal Sierpińskiego
        void DrawSierpinski(int lvl, Point3D pos, double size, int totalDepth)
        {
            // jeśli poziom głębokości jest równy 0, dodaj pojedynczy tetrahedron
            if (lvl == 0)
            {
                AddTetrahedron(pos, size, totalDepth);
                return;
            }
            // obliczanie połowy rozmiaru i wywoływanie rekurencyjnie dla 4 mniejszych tetrahedronów
            double half = size / 2;
            DrawSierpinski(lvl - 1, pos, half, totalDepth);
            DrawSierpinski(lvl - 1, new Point3D(pos.X + half, pos.Y, pos.Z), half, totalDepth);
            DrawSierpinski(lvl - 1, new Point3D(pos.X + half / 2, pos.Y + half, pos.Z), half, totalDepth);
            DrawSierpinski(lvl - 1, new Point3D(pos.X + half / 2, pos.Y + half / 2, pos.Z + half), half, totalDepth);
        }
        // metoda dodająca pojedynczy tetrahedron do modelu
        void AddTetrahedron(Point3D p, double s, int depth)
        {
            // tworzenie siatki tetrahedronu
            MeshGeometry3D mesh = new MeshGeometry3D();
            // wierzchołki tetrahedronu
            Point3D p0 = p;
            Point3D p1 = new Point3D(p.X + s, p.Y, p.Z);
            Point3D p2 = new Point3D(p.X + s / 2, p.Y + s, p.Z);
            Point3D p3 = new Point3D(p.X + s / 2, p.Y + s / 2, p.Z + s);
            // dodawanie wierzchołków do siatki
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.Positions.Add(p3);
            // indeksy trójkątów tworzących tetrahedron
            int[] indices = {
                0, 1, 2,
                0, 1, 3,
                1, 2, 3,
                0, 2, 3 
            };
            // kolorowanie tetrahedronu na podstawie poziomu głębokości
            Color color = Color.FromRgb(
                (byte)(50 + depth * 30),
                (byte)(100),
                (byte)(200 - depth * 30)
            );
            // dodawanie indeksów do siatki
            foreach (int i in indices) mesh.TriangleIndices.Add(i);
            // tworzenie modelu 3D z siatki i materiału
            var model = new GeometryModel3D(mesh, new DiffuseMaterial(new SolidColorBrush(color)));
            // ustawienie materiału tylnej ściany na ten sam kolor
            model.BackMaterial = model.Material;
            // dodawanie modelu do grupy
            group.Children.Add(model);
        }
        // metoda obsługująca kliknięcie przycisku ustawienia maksymalnego poziomu
        void SetMax_Click(object sender, RoutedEventArgs e)
        {
            // próba parsowania wartości z TextBox'a i ustawienie maksymalnego poziomu slider'a
            if (int.TryParse(maxLevelTextBox.Text, out int newMax))
            {
                // ograniczenie maksymalnego poziomu do 7 ze względu na wydajność
                if (newMax > 7) newMax = 7; 
                levelSlider.Maximum = newMax;
                maxLevelTextBox.Text = newMax.ToString();
                //MessageBox.Show("Maksymalny poziom ustawiony na: " + newMax);
            }
        }
        // metoda obsługująca kliknięcie przycisku startującego automatyczne budowanie poziomów
        void Start_Click(object sender, RoutedEventArgs e)
        {
            levelSlider.Value = 0;
            autoBuildCounter = 0;
            isAuto = true;
            autoRotate = true;
        }
        // metoda obsługująca kliknięcie przycisku zatrzymującego automatyczną rotację i budowanie poziomów
        void Stop_Click(object sender, RoutedEventArgs e)
        {
            isAuto = false;
            autoRotate = false;
        }
        // metody obsługujące zdarzenia myszy do ręcznej rotacji modelu
        void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                isDragging = true; lastMouse = e.GetPosition(this); 
            }
        }
        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            Point current = e.GetPosition(this);
            axisRotationY.Angle += (current.X - lastMouse.X) * 0.5;
            axisRotationX.Angle += (current.Y - lastMouse.Y) * 0.5;
            lastMouse = current;
        }
        void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }
        // metoda obsługująca zdarzenie mousescroll do przybliżania i oddalania kamery
        void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            distance -= e.Delta * 0.01;
            distance = Math.Max(2, Math.Min(distance, 50));
            camera.Position = new Point3D(0, 0, distance);
            camera.LookDirection = new Vector3D(0, 0, -distance);
        }
    }
}