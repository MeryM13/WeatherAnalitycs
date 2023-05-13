using System.Data;

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
