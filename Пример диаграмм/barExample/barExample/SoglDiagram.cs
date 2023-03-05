using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace barExample
{
    public class SoglDiagram
    {
        public Canvas Canvas { get; set; }
        
        private List<StoredValues> storedValues;
        private VertChart chart;
        private double summ;

        public SoglDiagram(List<StoredValues> list) 
        { 
            storedValues = list;
            summ = (double)list.Sum(x => (decimal)x.SoglValue);
            chart = new(list, summ);
        }
    }
}
