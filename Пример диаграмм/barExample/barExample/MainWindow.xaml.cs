using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace barExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VertChart chart;
        List<StoredValues> storedValues = new()
        {
            new StoredValues() {Header = "Первое", SoglValue = 100},
            new StoredValues() {Header = "Второе", SoglValue = 50},
            new StoredValues() {Header = "Третье", SoglValue = 77},
        };

        public MainWindow()
        {
            InitializeComponent();
            MinHeight = 800;
            MinWidth = 1000;
            chart = new VertChart(storedValues, 1000);

            //Добавляем диаграмму на грид
            PaintingSurface.Children.Add(chart.ChartBg);
        }

        private void PaintingSurface_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            CreateBarGraphics();
        }

        private void CreateBarGraphics()
        {
            // Принудительно обновляем размеры контейнера для графика
            PaintingSurface.UpdateLayout();
        }

        private void PaintingSurface_Loaded(object sender, RoutedEventArgs e)
        {
            CreateBarGraphics();
            PaintingSurface.SizeChanged += PaintingSurface_SizeChanged;
        }
    }
}
