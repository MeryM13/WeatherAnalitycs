using System.Data;

namespace WeatherAnalitycs.ViewModel
{
    internal class DisplayTableWindowViewModel
    { 
        public DisplayTableWindowViewModel(DataTable table, int entries, string title)
        {
            Table = table;
            WindowTitle = title;
            EntriesUsed = $"В расчетах использовано {entries} записей";
        }
        public DataTable Table { get; set; }
        public string EntriesUsed { get; set; }
        public string WindowTitle { get; set; }
    }
}
