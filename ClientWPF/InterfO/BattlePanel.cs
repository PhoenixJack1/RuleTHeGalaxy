using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    class BattlePanel:SelectedGrid
    {
        public BattlePanel(string Name)
            : base(Name)
        {
            put();
        }
        ScrollViewer viewer;
        public void put()
        {
            Children.Clear();
            ShowGridLines = false;
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions[1].Width = new GridLength(150, GridUnitType.Pixel);
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            viewer = new ScrollViewer();
            Children.Add(viewer);
            viewer.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            Grid.SetRowSpan(viewer, 2);

            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            Button btn = new Button();


            Grid grid = new Grid();
            grid.Width = 1400;
            grid.Height = 600;
            viewer.Content = grid;
            grid.ShowGridLines = true;
            for (int i = 0; i < 7; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.Children.Add(btn);
            Grid.SetRow(btn, 1);

        }

        public void Select()
        {
            base.Select();
            
        }
    }
}
/*
 * using System;
{
    class GalaxyPanel:SelectedGrid
    {
        public GalaxyPanel(string Name):base(Name)
        {
            put();
            

        }
        Canvas galaxyCanvas;
        public void put()
        {
            Children.RemoveAt(0);
            Viewbox vbx = new Viewbox();
            Children.Add(vbx);
            galaxyCanvas = new Canvas();
            vbx.Child = galaxyCanvas;
            galaxyCanvas.Background = Brushes.Black;
            galaxyCanvas.Width = Links.GalaxyWidth;
            galaxyCanvas.Height = Links.GalaxyHeight;
            foreach (GSStar star in Links.Stars.Values)
                new Star(star, galaxyCanvas);
            for (int i = Links.SectorWidth; i < galaxyCanvas.Width; i += Links.SectorWidth)
            {
                Line line = new Line();
                line.Stroke = Brushes.White;
                line.StrokeThickness = 1;
                line.X1 = i; line.Y1 = 0;
                line.X2 = i; line.Y2 = galaxyCanvas.Height;
                line.StrokeDashArray = new DoubleCollection { 1.0, 2.0 };
                galaxyCanvas.Children.Add(line);
            }
            for (int i = Links.SectorHeight; i < galaxyCanvas.Height; i += Links.SectorHeight)
            {
                Line line = new Line();
                line.Stroke = Brushes.White;
                line.StrokeThickness = 1;
                line.X1 = 0; line.Y1 = i;
                line.X2 = galaxyCanvas.Width; line.Y2 = i;
                line.StrokeDashArray = new DoubleCollection { 1.0, 2.0 };
                galaxyCanvas.Children.Add(line);
            }
            galaxyCanvas.MouseDown += new MouseButtonEventHandler(galaxyCanvas_MouseDown);
        }

        void galaxyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point MouseClick = Mouse.GetPosition(galaxyCanvas);
                int xSector = (int)(MouseClick.X / Links.SectorWidth);
                int ySector = (int)(MouseClick.Y / Links.SectorHeight);
                SectorPanel.SectorX = xSector;
                SectorPanel.SectorY = ySector;
                Links.Controller.sectorpanel.Select();
            }
        }
        class Star
        {
            GSStar star;
            Shape Figure;
            Size FigureSize;

            public Star(GSStar star, Canvas parent)
            {
                this.star = star;
                
                switch (star.Class)
                {
                    case EStarClass.BH: CreateBlackHole(); break;
                    default: CreateStar(); break;
                }
                //if (ID > 3) return;
                parent.Children.Add(Figure);
                double left = star.Coordinate.Galaxy.X - FigureSize.Width/ 2;
                double top = star.Coordinate.Galaxy.Y - FigureSize.Height / 2;
                Canvas.SetLeft(Figure, left);
                Canvas.SetTop(Figure, top);

            }
            void CreateBlackHole()
            {
                Ellipse bh = new Ellipse();
                double size = 15 - (int)star.Class;
                bh.Width = size; bh.Height = size;
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.Black, 0.2));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#e6e6fa"), 0.35));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#ea8df7"), 0.6));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#4b0082"), 0.8));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#660099"), 0.9));
                brush.GradientStops.Add(new GradientStop(Color.FromArgb(0,0,0,0), 1));
                bh.Fill = brush;
                Figure = bh;
                FigureSize.Width = size; FigureSize.Height = size;
            }
            void CreateStar()
            {
                double size = 15 - (int)star.Class*2;
                double MaxLength = 5 * size/10;
                double SmallLength = size/10;
                Polygon pg1 = new Polygon();
                pg1.Points.Add(new Point(0, MaxLength));
                pg1.Points.Add(new Point(MaxLength - SmallLength, MaxLength - SmallLength));
                pg1.Points.Add(new Point(MaxLength, 0));
                pg1.Points.Add(new Point(MaxLength + SmallLength, MaxLength - SmallLength));
                pg1.Points.Add(new Point(2 * MaxLength, MaxLength));
                pg1.Points.Add(new Point(MaxLength + SmallLength, MaxLength + SmallLength));
                pg1.Points.Add(new Point(MaxLength, 2 * MaxLength));
                pg1.Points.Add(new Point(MaxLength - SmallLength, MaxLength + SmallLength));

                pg1.Fill = Links.starBrushes[(int)star.Class];
                Figure = pg1;
                FigureSize.Width = pg1.Points[4].X;
                FigureSize.Height = pg1.Points[6].Y;
            }
        }
    }
    

}

*/