using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Charts
{
    internal class PieChart : Chart
    {
        public override void AddValue(double value, string header)
        {
            PathList = СalculateSectorAngle(value, header);

            ChartBg.Children.Clear();

            // Размещение секторов на канвасе
            for (int i = 0; i < PathList.Count; i++)
            {
                Path p = CreateSector(PathList[i]);
                ChartBg.Children.Add(p);

                // Числовые значения секторов диска.
                Label label = new()
                {
                    Content = PathList[i].Header,
                    FontWeight = FontWeights.Bold,
                };

                // Цветовые метки перед числовыми
                Rectangle r = new()
                {
                    Width = 16,
                    Height = 12,
                    Fill = p.Fill,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                StackPanel sp = new()
                {
                    Orientation = Orientation.Horizontal
                };
                sp.Children.Add(r);
                sp.Children.Add(label);
                Canvas.SetLeft(sp, 10);
                Canvas.SetTop(sp, 20 * i);
                ChartBg.Children.Add(sp);
            }
        }

        // Создание готового для вывода сектора
        private Path CreateSector(StoredValues storedValues)
        {
            Random random = new();

            Path path = new()
            {
                StrokeThickness = 3,
                Stroke = Brushes.Black,
                Fill = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(50, 256), (byte)random.Next(50, 256), (byte)random.Next(50, 256))),

                Data = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                    {
                        SectorGeometry(storedValues.Degree, storedValues.Offset)
                    }
                },

                Tag = new StoredValues()
                {
                    Degree = storedValues.Degree,
                    Offset = storedValues.Offset,
                    Value = storedValues.Value
                }
            };



            return path;
        }

        // Перерасчет переменных для создания сектора
        private PathFigure SectorGeometry(double degree, double offset)
        {
            double ChartRadius = ChartBg.ActualHeight / 2 - PaddingChart;

            bool isLarge = degree > 180 ? true : false;

            // Если начальная точка совпадет с конечной, сектор не отразится
            if (degree >= 360) degree = 359.999;

            Point centerPoint = new(ChartBg.ActualWidth / 2, ChartBg.ActualHeight / 2);

            Point startPoint = new(centerPoint.X, centerPoint.Y - ChartRadius);

            Point endPoint = startPoint;

            // Поворачиваем на угол смещения стартовую точку.
            RotateTransform rotateStartPoint = new(offset)
            {
                CenterX = centerPoint.X,
                CenterY = centerPoint.Y
            };
            startPoint = rotateStartPoint.Transform(startPoint);

            // Поворачиваем на заданный угол конечную точку
            RotateTransform rotateEndPoint = new(offset + degree)
            {
                CenterX = centerPoint.X,
                CenterY = centerPoint.Y
            };
            endPoint = rotateEndPoint.Transform(endPoint);

            PathFigure sector = new()
            {
                StartPoint = startPoint,
                Segments = new PathSegmentCollection()
                {
                    new ArcSegment()
                    {
                        Point = endPoint,
                        Size = new(ChartRadius, ChartRadius),
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = isLarge,
                        IsStroked = true
                    },

                    new PolyLineSegment()
                    {
                        Points = new PointCollection() { startPoint, centerPoint, endPoint },
                        IsStroked = false,
                    }
                }
            };

            return sector;
        }

        // Пересчитываем уже добавленные значения и записываем новые
        private List<StoredValues> СalculateSectorAngle(double value, string header)
        {
            StoredValues d = new()
            {
                Value = value,
                Header = header
            };
            
            PathList.Add(d);

            PathList = PathList.OrderByDescending(p => p.Value).ToList();

            double sum = PathList.Select(p => p.Value).Sum();

            // Общий знаменатель (значение, которое приходится на 1 градус)
            double denominator = sum / 360;

            for (int i = 0; i < PathList.Count; i++)
            {
                // Для исключения артефактов при рисовании снижаем точность
                PathList[i].Degree = Math.Round(PathList[i].Value / denominator, 2);

                double offset = 0;
                if (i > 0)
                {
                    offset = PathList[i - 1].Degree + PathList[i - 1].Offset;
                }

                PathList[i].Offset = offset;
            }

            return PathList;
        }
    }

}
