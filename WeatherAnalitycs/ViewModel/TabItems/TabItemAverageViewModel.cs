using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.View;
using WeatherDataParser;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemAverageViewModel: BaseTabItemViewModel
    {
        SearchParamsStore _store;
        string _intervalName, _parameterName;
        Parameter _parameter = Parameter.Temperature;
        DateInterval _interval = DateInterval.Month;

        readonly Dictionary<string, Parameter> parameterDict = new()
        {
            {"Скорость ветра", Parameter.Wind_Speed },
            {"Температура воздуха", Parameter.Temperature },
            {"Относительная влажность воздуха", Parameter.Humidity },
            {"Атмосферное давление", Parameter.Pressure },
            {"Высота снежного покрова", Parameter.Snow_Height },
        };
        readonly Dictionary<string, DateInterval> intervalDict = new()
        {
            {"День", DateInterval.Day },
            {"Неделя", DateInterval.WeekOfYear },
            {"Месяц", DateInterval.Month },
            {"Квартал", DateInterval.Quarter },
            {"Год", DateInterval.Year },
        };

        public List<string> Intervals { get => intervalDict.Select(x => x.Key).ToList(); }
        public List<string> Parameters { get => parameterDict.Select(x => x.Key).ToList(); }

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

        public string ParameterName
        {
            get => parameterDict.FirstOrDefault(x => x.Value == _parameter).Key;
            set
            {
                _parameterName = value;
                _parameter = parameterDict[_parameterName];
                OnPropertyChanged(nameof(ParameterName));
            }
        }

        public TabItemAverageViewModel(SearchParamsStore store) : base(store)
        {
            _store = store;
            ButtonPressCommand = new RelayCommand(OpenAverageGraph);
        }

        void OpenAverageGraph()
        {
            Statistics stat = new(_store.From, _store.To, _store.StationId);
            int entries = stat.GetAll();
            string title = $"График среднe{_intervalName} значений {_parameterName} для станции {_store.StationId} за период с {_store.From:d} до {_store.To:d}";
            DisplayWindow window = new(title, entries, stat.GetAveragesChart(_parameter, _interval));
            window.Show();
        }
    }
}
