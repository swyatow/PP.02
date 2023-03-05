using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Charts
{
    internal class BarChart : Chart
    {
        // Расстояние между графиками
        private double gap = 5;

        public override void AddValue(double value, string header)
        {
            BarsDict.Add(header, value);

            // Вычисляем новую ширину бара, чтобы график поместился 
            double widthBar = (Width - ((BarsDict.Count - 1) * gap)) / BarsDict.Count;

            // Ограничение высоты графика
            double maxValue = BarsDict.Values.Max();
            double denominator = maxValue / Height;

            ChartBg.Children.Clear();

            // Перерисовываем графики
            foreach (var pair in BarsDict)
            {
                int count = ChartBg.Children.OfType<Rectangle>().Count();

                double heightPoint = pair.Value / denominator;

                double BarX = (count * (widthBar + gap)) + (ChartBg.ActualWidth - Width) / 2;

                // Создание бара
                Rectangle bar = CreateBar(BarX, heightPoint, widthBar, pair.Value);
                ChartBg.Children.Add(bar);

                // Надпись над баром
                Label title = CreateTitle(BarX, bar.Height, widthBar, pair.Key);
                ChartBg.Children.Add(title);
            }
        }

        //Создание столбика в диаграмме
        private Rectangle CreateBar(double x, double height, double width, double value)
        {
            Random random = new();

            Rectangle bar = new()
            {
                Stroke = Brushes.Black,
                Fill = new SolidColorBrush(Colors.Gray),
                Height = height,
                Width = width,
                StrokeThickness = 0.5,
                Tag = value
            };

            Canvas.SetLeft(bar, x);
            Canvas.SetBottom(bar, 0);

            return bar;
        }

        //Создание подписи
        private Label CreateTitle(double x, double y, double width, string header)
        {
            Label title = new()
            {
                Content = header,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Width = width,
                Padding = new Thickness(0, 0, 0, 10),
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(title, x);
            Canvas.SetBottom(title, y);

            return title;
        }
    }
}
