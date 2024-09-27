using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalytics.View;
using WeatherDataParser;
using WeatherDataParser.CLASSES;

namespace WeatherAnalitycs.ViewModel
{
    class AddStationViewModel : BaseViewModel
    {
        Parser _parser = new();
        Station _station;
        Window _window;
        string _stationInfo = "Название станции: Расположение: Географические координаты: широта: долгота: Высота над уровнем моря:";
        string _stationId;

        public string StationId
        {
            get => _stationId;
            set
            {
                _stationId = value;
                OnPropertyChanged(nameof(StationId));
            }
        }

        public string StationInfo
        {
            get => _stationInfo;
            set
            {
                _stationInfo = value;
                OnPropertyChanged(nameof(StationInfo));
            }
        }

        public Station Station
        {
            get { return _station; }
            set
            {
                _station = value;
                if (String.IsNullOrEmpty(_station.Name))
                {
                    AddButtonEnabled = false;
                    StationInfo = "Название станции:  Расположение:  Географические координаты: широта:  долгота:  Высота над уровнем моря:";
                }
                else
                {
                    AddButtonEnabled = true;
                    StationInfo = $"Название станции: {_station.Name}; Расположение: {_station.Location}; Географические координаты: широта: {_station.Latitude}, долгота: {_station.Longitude}; Высота над уровнем моря: {_station.Height} м.";
                }
                OnPropertyChanged(nameof(Station));
                OnPropertyChanged(nameof(StationInfo));
                OnPropertyChanged(nameof(AddButtonEnabled));
            }
        }

        bool _addButtonEnabled = false;
        public bool AddButtonEnabled
        {
            get => _addButtonEnabled;
            set
            {
                _addButtonEnabled = value;
                OnPropertyChanged(nameof(AddButtonEnabled));
            }
        }

        public RelayCommand ExitCommand { get; set; }
        public RelayCommand SearchStationCommand { get; set; }
        public RelayCommand AddStationCommand { get; set; }
        

        public AddStationViewModel(Window window)
        {
            _window = window;
            ExitCommand = new RelayCommand(Exit);
            SearchStationCommand = new RelayCommand(SearchStation);
            AddStationCommand = new RelayCommand(AddStation);
        }

        void Exit()
        {
_window.Close();
        }

        void SearchStation()
        {
            Station = new();
            try
            {
                Station = _parser.FindStation(int.Parse(StationId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void AddStation()
        {
            try
            {
                var result = MessageBox.Show($"Вы уверены что хотите добавить станцию {_station.Name}? Добавление запустит процесс парсинга всех имеющихся метеоданных этой станции, что может быть долгим процессом. Процесс парсинга не должен прерываться.", $"Добавить станцию {_station.Name}", MessageBoxButton.YesNoCancel);
                ProgressBarWindow progress = new();
                progress.Show();
                await Task.Run(() => _parser.AddStation(_station));
                progress.Close();
                Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
