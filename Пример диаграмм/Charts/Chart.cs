using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Charts
{
    internal abstract class Chart
    {
        // Максимальная высота графика (значение 100%)
        private readonly double factor = 0.66666667;
        public readonly double PaddingChart = 10;
        public double Width;
        public double Height;
        public Canvas ChartBg = new();
        public Dictionary<string, double> BarsDict = new Dictionary<string, double>();
        public List<StoredValues> PathList = new List<StoredValues>();

        public Chart()
        {
            ChartBg.Margin = new Thickness(0);
            ChartBg.SizeChanged += ChartBg_SizeChanged;
        }

        public void ChartBg_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = e.NewSize.Width - (PaddingChart * 2);
            Height = e.NewSize.Height * factor;

            ChartBg.Background = DrawLines(e.NewSize.Width, Width, PaddingChart);
        }

        public abstract void AddValue(double value, string header);


        // Кисть для заднего фона
        private Brush DrawLines(double actualwidth, double widthchart, double padding)
        {
            double totalWidth = widthchart / actualwidth;

            int numLines = 10;

            DrawingBrush brush = new()
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(1, 0, totalWidth / numLines, factor / numLines),
                // Рисуем прямоугольник, формирующий фоновую сетку.
                Drawing = new GeometryDrawing()
                {
                    Pen = new(Brushes.Black, 0.05),
                    Brush = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    Geometry = new RectangleGeometry(new Rect(0, 0, 45, 20))
                }
            };

            return brush;
        }
        public class StoredValues
        {
            // Угол сектора диска
            public double Degree;

            // Угловое смещение
            public double Offset;

            public double Value;
            public string Header;
        }
    }
}
