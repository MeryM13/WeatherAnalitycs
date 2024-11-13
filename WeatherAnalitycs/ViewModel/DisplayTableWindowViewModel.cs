using System.Data;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.Utility.ViewModelBases;

namespace WeatherAnalitycs.ViewModel
{
    internal class DisplayTableWindowViewModel(DataTable table, int entries, string title, SettingsClass settings) : ViewModelBase(settings)
    {
        public DataTable Table { get; set; } = table;
        public string EntriesUsed { get; set; } = $"В расчетах использовано {entries} записей";
        public string WindowTitle { get; set; } = title;
    }
}
