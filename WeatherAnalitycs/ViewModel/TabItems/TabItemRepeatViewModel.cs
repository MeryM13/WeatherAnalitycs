using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.Utility.ViewModelBases;
using WeatherAnalitycs.View;
using WeatherDataParser;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemRepeatViewModel: BaseTabItemViewModel
    {
        decimal _direction;
        string _intervalName, _directionName;
        DateInterval _interval = DateInterval.Month;
        bool _calm;

        readonly Dictionary<string, DateInterval> intervalDict = new()
        {
            {"День", DateInterval.Day },
            {"Неделя", DateInterval.WeekOfYear },
            {"Месяц", DateInterval.Month },
            {"Квартал", DateInterval.Quarter },
            {"Год", DateInterval.Year },
        };

        readonly Dictionary<decimal, string> directionDict = new()
        {
            { 0, "северного ветра" },
            { 45, "северо-восточного ветра" },
            { 90, "восточного ветра" },
            { 135, "юго-восточного ветра" },
            { 180, "южного ветра" },
            { 225, "юго-западного ветра" },
            { 270, "западного ветра" },
            { 315, "северо-западного ветра" }
        };
        
        public List<string> Intervals { get => intervalDict.Select(x => x.Key).ToList(); }
        public string IntervalName
        {
            get => intervalDict.FirstOrDefault(x => x.Value == _interval).Key;
            set
            {
                _intervalName = value;
                _interval = intervalDict[_intervalName];
                OnPropertyChanged(nameof(IntervalName));
            }
        }

        public decimal Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                _directionName = directionDict[_direction];
                _calm = false;
                OnPropertyChanged(nameof(Direction));
                OnPropertyChanged(nameof(Calm));
            }
        }

        public bool Calm
        {
            get { return _calm; } 
            set
            {
                _calm = value;
            }
        }

        public TabItemRepeatViewModel(SearchParamsStore store, SettingsClass settings): base(store, settings) 
        {
            _intervalName = "Месяц";
            _directionName = "северного ветра";
            ButtonPressCommand = new RelayCommand(OpenRepeatGraph);
            ConvertToExcelCommand = new RelayCommand(CreateExcelSheet);
        }

        void OpenRepeatGraph()
        {
            Statistics stat = new(Store.From, Store.To, Store.StationId, Settings.DatabaseConnectionString, Settings.DatabaseServer);
            decimal? direction = _direction;
            if (Calm)
            {
                direction = null;
                _directionName = "штилей";
            }
            string title = $"График повторяемости {_directionName} с интервалом {_intervalName} для станции {Store.StationId} за период с {Store.From:d} до {Store.To:d}";
            int entries = stat.GetAll();
            DisplayWindow window = new(title, entries, stat.GetWindPeriodicityChart(direction, _interval), Settings);
            window.Show();
        }

        async void CreateExcelSheet()
        {
            Statistics stat = new(Store.From, Store.To, Store.StationId, Settings.DatabaseConnectionString, Settings.DatabaseServer);
            decimal? direction = _direction;
            if (Calm)
            {
                direction = null;
                _directionName = "штилей";
            }
            var converter = new ExcelConverter(Settings.ExcelSavePath);
            await Task.Run(() => converter.Convert($"{Store.StationId}_Повторяемость {_directionName}_{Store.From:d}-{Store.To:d}",
                stat.GetWindPeriodicityChart(direction, _interval)));
            MessageBox.Show("Таблица создана");
        }
    }
}
