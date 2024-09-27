using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using WeatherAnalytics.View;
using WeatherDataParser;
using WeatherDataParser.CLASSES;

namespace WeatherAnalitycs.ViewModel
{
    class DataUpdateViewModel:BaseViewModel
    {
        Parser _parser = new();
        Window _window;
        bool _updateAll = true;
        bool _listEnabled;
        public bool UpdateAll
        {
            get => _updateAll;
            set
            {
                _updateAll = value;
                ListEnabled = !_updateAll;
                OnPropertyChanged(nameof(UpdateAll));
                OnPropertyChanged(nameof(ListEnabled));
            }
        }
        public bool ListEnabled
        {
            get => _listEnabled;
            set
            {
                _listEnabled = value;
                OnPropertyChanged(nameof(ListEnabled));
            }
        }

        List<UpdateStationCheckboxViewModel> _stationList;
        public List<UpdateStationCheckboxViewModel> StationList
        {
            get => _stationList;
            set
            {
                _stationList = value;
                OnPropertyChanged(nameof(StationList));
            }
        }

        List<string> _selectedStations;
        public List<string> SelectedStations
        {
            get => _selectedStations;
            set
            {
                _selectedStations = value;
                OnPropertyChanged(nameof(SelectedStations));
            }
        }

        public RelayCommand UpdateCommand { get; set; }

        public DataUpdateViewModel(Window window)
        {
            _window = window;
            _selectedStations = new();
            _stationList = new();
            foreach (string station in _parser.GetStationNamesList())
            {
                _stationList.Add(new(station, ref _selectedStations));
            }      
            UpdateCommand = new RelayCommand(Update);
        }

        async void Update()
        {
            if (_updateAll)
            {
                var result = MessageBox.Show("Вы уверены, что хотите обновить информацию о метеоданных для всех станций? \nДанное действие может занять много времени и не должно прерываться", "Обновление данных", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    ProgressBarWindow progress = new();
                    progress.Show();
                    await Task.Run(() => _parser.FullUpdate());
                    progress.Close();
                    _window.Close();
                }
            }
            else
            {
                var result = MessageBox.Show($"Вы уверены, что хотите обновить информацию о метеоданных для следующих станций: {String.Join(',', _selectedStations)} \nДанное действие может занять много времени и не должно прерываться", "Обновление данных", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    ProgressBarWindow progress = new();
                    progress.Show();
                    await Task.Run(() =>
                    {
                        foreach (string station in _selectedStations)
                        {
                            _parser.UpdateStationData(int.Parse(station.Split('(', ')')[1]));
                        }
                    });
                    progress.Close();

                    _window.Close();
                }
            }
        }
    }

    class UpdateStationCheckboxViewModel : BaseViewModel
    {
        string _stationName;
        List<string> _selectedStations;
        public UpdateStationCheckboxViewModel(string stationName, ref List<string> selectedStations) 
        {
            _stationName = stationName;
            _selectedStations = selectedStations;
        }

        public string StationName
        {
            get { return _stationName; }
            set { _stationName = value; OnPropertyChanged(StationName); }
        }

        public bool IncludeStation
        {
            get { return _selectedStations.Contains(StationName); }
            set
            {
                if (value)
                {
                    if (!_selectedStations.Contains(StationName))
                    {
                        _selectedStations.Add(StationName);
                    }
                }
                else
                {
                    if (_selectedStations.Contains(StationName))
                    {
                        _selectedStations.Remove(StationName);
                    }
                }
            }
        }
    }
}
