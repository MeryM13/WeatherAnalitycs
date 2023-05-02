using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalitycs.ViewModel
{
    internal class DisplayTableWindowViewModel
    { 

        public DisplayTableWindowViewModel(DataTable table, string title)
        {
            Table = table;
            WindowTitle = title;
        }
        public DataTable Table { get; set; }
        public string WindowTitle { get; set; }
    }
}
