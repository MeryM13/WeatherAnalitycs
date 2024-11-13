using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.Utility.ViewModelBases;
using WeatherAnalitycs.View;
using WeatherDataParser;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemWindroseViewModel : BaseTabItemViewModel
    {
        SearchParamsStore _store;
        bool _distributeCalm = false, _8Directions = true, _differentiate = false;
        int _numberOfDirections = 8;
        List<int> _diffSpeeds = new() { 20, 15, 10, 8, 6, 4, 2 };
        string _diffString;

        public bool DistributeCalm
        {
            get => _distributeCalm;
            set
            {
                _distributeCalm = value;
                OnPropertyChanged(nameof(DistributeCalm));
            }
        }

        public bool Directions
        {
            get => _8Directions;
            set
            {
                _8Directions = value;
                if (_8Directions)
                    _numberOfDirections = 8;
                else
                    _numberOfDirections = 16;
                OnPropertyChanged(nameof(Directions));
            }
        }

        public bool Differentiate
        {
            get => _differentiate;
            set
            {
                _differentiate = value;
                OnPropertyChanged(nameof(Differentiate));
            }
        }

        public string DiffString
        {
            get => _diffString;
            set
            {
                _diffString = value;
                OnPropertyChanged(nameof(DiffString));
            }
        }

        public TabItemWindroseViewModel(SearchParamsStore store, SettingsClass settings) : base(store, settings)
        {
            _store = store;
            _diffString = string.Join(", ", _diffSpeeds.Select(s => s.ToString()));
            ButtonPressCommand = new RelayCommand(OpenWindrose);
            ConvertToExcelCommand = new RelayCommand(CreateExcelSheet);
        }

        void OpenWindrose()
        {
            GetRose("OpenWindow");
        }

        void CreateExcelSheet()
        {
            GetRose("CreateExcel");
        }

        async void GetRose(string mode)
        {
            Statistics stat = new(_store.From, _store.To, _store.StationId);
            _diffSpeeds = _diffString.Split(", ").Select(int.Parse).OrderDescending().ToList();
            switch (mode)
            {
                case "OpenWindow":
                    {
                        int entries = stat.GetAll();
                        string title = $"{_numberOfDirections}-румбовая роза ветров {(_distributeCalm ? "с распределением штилей" : "без распределения штилей")} " +
                            $"для станции {_store.StationId} за период с {_store.From:d} до {_store.To:d}";

                        DisplayWindow window;
                        if (_differentiate)
                            window = new(title, entries, stat.GetDifferentiatedPercentageWindRose(_distributeCalm, _numberOfDirections, _diffSpeeds));
                        else
                            window = new(title, entries, stat.GetPercentageWindRose(_distributeCalm, _numberOfDirections));

                        window.Show();
                        break;
                    }
                case "CreateExcel":
                    {
                        var converter = new ExcelConverter();
                        string title = $"{SearchParamsStore.StationId}_Роза{_numberOfDirections}_{_store.From:d}-{_store.To:d}";

                        if (_differentiate)
                            await Task.Run(() => converter.Convert(title, stat.GetDifferentiatedPercentageWindRose(_distributeCalm, _numberOfDirections, _diffSpeeds)));
                        else
                            await Task.Run(() => converter.Convert(title, stat.GetPercentageWindRose(_distributeCalm, _numberOfDirections)));

                        MessageBox.Show("Таблица создана");
                        break;
                    }
            }
        }
    }
}
