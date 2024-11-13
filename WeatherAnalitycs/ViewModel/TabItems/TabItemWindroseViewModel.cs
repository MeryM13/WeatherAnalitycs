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
        bool _distributeCalm = false, _8Directions = true, _differentiate;
        int _numberOfDirections = 8;
        List<int> _diffSpeeds;
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
            Differentiate = settings.DifferentiateRoseByDefault;
            _diffSpeeds = settings.DifferentiateRoseSpeeds;
            DiffString = string.Join(", ", _diffSpeeds.Select(s => s.ToString()));
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
            Statistics stat = new(Store.From, Store.To, Store.StationId, Settings.DatabaseConnectionString, Settings.DatabaseServer);
            _diffSpeeds = [.. _diffString.Split(", ").Select(int.Parse).OrderDescending()];
            switch (mode)
            {
                case "OpenWindow":
                    {
                        int entries = stat.GetAll();
                        string title = $"{_numberOfDirections}-румбовая роза ветров {(_distributeCalm ? "с распределением штилей" : "без распределения штилей")} " +
                            $"для станции {Store.StationId} за период с {Store.From:d} до {Store.To:d}";

                        DisplayWindow window;
                        if (_differentiate)
                            window = new(title, entries, stat.GetDifferentiatedPercentageWindRose(_distributeCalm, _numberOfDirections, _diffSpeeds), Settings);
                        else
                            window = new(title, entries, stat.GetPercentageWindRose(_distributeCalm, _numberOfDirections), Settings);

                        window.Show();
                        break;
                    }
                case "CreateExcel":
                    {
                        var converter = new ExcelConverter(Settings.ExcelSavePath);
                        string title = $"{Store.StationId}_Роза{_numberOfDirections}_{Store.From:d}-{Store.To:d}";

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
