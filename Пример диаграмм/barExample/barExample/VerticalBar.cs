using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace barExample
{
    public class VertChart
    {
        // Максимальная высота графика (значение 100%)
        private readonly double factor = 0.95;
        // Расстояние между графиками
        private double gap = 5;
        // Отступ от "низа" диаграммы
        private double leftMargin = 5;

        public readonly double PaddingChart = 10;
        public double Width;
        public double Height;
        public Canvas ChartBg = new();
        public List<StoredValues> ValuesList;
        public double Count;

        public VertChart(List<StoredValues> values, double count)
        {
            ValuesList = values;
            Count = count;
            ChartBg.Background = Brushes.LightGray;
            ChartBg.Margin = new Thickness(0);
            ChartBg.SizeChanged += ChartBg_SizeChanged;
        }

        public void ChartBg_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = e.NewSize.Width * factor - leftMargin;
            Height = e.NewSize.Height - (PaddingChart * 2);
            createDiagram();
        }

        private void createDiagram()
        {
            // Вычисляем новую ширину бара, чтобы график поместился 
            double widthBar = (Height - ((ValuesList.Count - 1) * gap)) / ValuesList.Count;

            // Ограничение высоты графика
            double maxValue = Count;
            double denominator = maxValue / Width;

            ChartBg.Children.Clear();
            int count = 0;
            // Перерисовываем графики
            foreach (var item in ValuesList)
            {
                double heightPoint = item.SoglValue / denominator;

                double BarX = (count * (widthBar + gap)) + (ChartBg.ActualHeight - Height) / 2;

                // Создание max бара 
                Rectangle bar = CreateBar(BarX, Count / denominator, widthBar, Count, Colors.Gray);
                ChartBg.Children.Add(bar);

                // Создание бара
                bar = CreateBar(BarX, heightPoint, widthBar, item.SoglValue, Colors.DarkGreen);
                ChartBg.Children.Add(bar);

                // Надпись над баром
                Label title = CreateTitle(BarX, Count / denominator * 0.02, widthBar, item.SoglValue.ToString());
                ChartBg.Children.Add(title);
                count++;
            }
        }

        //Создание столбика в диаграмме
        private Rectangle CreateBar(double x, double height, double width, double value, Color brush)
        {
            Rectangle bar = new()
            {
                Stroke = Brushes.Black,
                Fill = new SolidColorBrush(brush),
                Height = width,
                Width = height,
                StrokeThickness = 0.5,
                Tag = value
            };
            Canvas.SetLeft(bar, leftMargin);
            Canvas.SetTop(bar, x);

            return bar;
        }

        //Создание подписи
        private Label CreateTitle(double x, double y, double height, string header)
        {
            Label title = new()
            {
                Content = header,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Padding = new Thickness(10, height / 2 - 10, 0, 0),
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(title, y);
            Canvas.SetTop(title, x);

            return title;
        }
    }
}
