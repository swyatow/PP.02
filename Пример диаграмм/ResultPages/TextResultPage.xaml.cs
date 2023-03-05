using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace TextAnalysis.ResultPages
{
    /// <summary>
    /// Логика взаимодействия для TextResult.xaml
    /// </summary>
    public partial class TextResult : Page
    {
        public List<SymbolClass> chars = new();

        public TextResult(Dictionary<char, int> symbols, int symbolsCount)
        {
            InitializeComponent();
            float percent = 0;
            foreach (var pair in symbols)
            {
                percent = (float)pair.Value / (float)symbolsCount * 100;
                SymbolClass sym = new(pair.Key, pair.Value, $"{Math.Round(percent,3)}%");
                chars.Add(sym);

            }
            
            this.TextResultListView.ItemsSource = chars;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TextResultListView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("symbolCount", ListSortDirection.Descending));
        }
    }

    public class SymbolClass
    {
        public char symbol { get; set; }
        public int symbolCount { get; set; }
        public string symbolPercent { get; set; }

        public SymbolClass(char symbol, int symbolCount, string symbolPercent)
        {
            this.symbol = symbol;
            this.symbolCount = symbolCount;
            this.symbolPercent = symbolPercent;
        }
    }
}
