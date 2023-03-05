using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Charts;

namespace TextAnalysis.ResultPages
{
    /// <summary>
    /// Логика взаимодействия для BarDiagramPage.xaml
    /// </summary>
    public partial class BarDiagramPage : Page
    {
        Chart chart = new BarChart();
        Dictionary<char, int> symbols = new Dictionary<char, int>();

        public BarDiagramPage(Dictionary<char, int> symbols, int symbolsCount)
        {
            InitializeComponent();
            this.symbols = symbols;

            //Добавляем диаграмму на грид
            PaintingSurface.Children.Add(chart.ChartBg);
        }

        private void PaintingSurface_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CreateBarGraphics();
            PaintingSurface.SizeChanged += PaintingSurface_SizeChanged;
        }

        private void PaintingSurface_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            CreateBarGraphics();
        }

        private void CreateBarGraphics()
        {
            // Принудительно обновляем размеры контейнера для графика
            PaintingSurface.UpdateLayout();
            PaintingSurface.ToolTip = null;

            chart.ChartBg.Children.Clear();
            chart.BarsDict.Clear();

            int counter = 0;
            foreach (var symbol in symbols)
            {
                chart.AddValue(symbol.Value, $"{symbol.Key} - {symbol.Value}");
                counter++;
                if (counter == 10) break;
            }
        }
    }
}
