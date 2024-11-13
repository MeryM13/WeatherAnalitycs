using CommunityToolkit.Mvvm.Input;
using System;
using MvvmHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalytics.View;
using WeatherDataParser.CLASSES;
using WeatherAnalitycs.Utility;
using WeatherDataParser;
using WeatherAnalitycs.ViewModel.TabItems;
using WeatherAnalitycs.View;
using WeatherAnalitycs.Utility.ViewModelBases;

namespace WeatherAnalitycs.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        readonly Parser _parser = new();
        List<string> _stationNames;
        public List<string> StationNames 
        { 
            get => _stationNames; 
            set
            {
                _stationNames = value;
                OnPropertyChanged(nameof(StationNames));
            } 
        }
        
        string _selectedStation;
        public string SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                SearchStore.StationId = int.Parse(_selectedStation.Split('(', ')')[1]);
                StationInfo = _parser.GetStationInfo(SearchStore.StationId);
                OnPropertyChanged(nameof(SelectedStation));
                OnPropertyChanged(nameof(StationInfo));
            }
        }

        string _stationInfo;
        public string StationInfo
        {
            get => _stationInfo;
            set
            {
                _stationInfo = value;
                OnPropertyChanged(nameof(StationInfo));
            }
        }

        public SearchParamsStore SearchStore { get; set; } = new();

        public TabItemTableViewModel TableViewModel { get; set; }
        public TabItemRepeatViewModel RepeatViewModel { get; set; }
        public TabItemAverageViewModel AverageViewModel { get; set; }
        public TabItemWindroseViewModel WindroseViewModel { get; set; }
        public TabItemCalmcountViewModel CalmcountViewModel { get; set; }

        public RelayCommand UpdateDataCommand { get; set; }
        public RelayCommand AddNewStationCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand OpenSettingsCommand { get; set; }

        public MainViewModel(SettingsClass settings): base(settings)
        {
            StationNames = _parser.GetStationNamesList();
            SelectedStation = StationNames[0];
            UpdateDataCommand = new RelayCommand(UpdateData);
            AddNewStationCommand = new RelayCommand(AddNewStation);
            ExitCommand = new RelayCommand(Exit);
            OpenSettingsCommand = new RelayCommand(OpenSettings);

            TableViewModel = new(SearchStore, settings);
            RepeatViewModel = new(SearchStore, settings);
            AverageViewModel = new(SearchStore, settings);
            WindroseViewModel = new(SearchStore, settings);
            CalmcountViewModel = new(SearchStore, settings);
        }

        void UpdateData()
        {
            DataUpdateWindow DUW = new();
            DUW.ShowDialog();
        }
        async void AddNewStation()
        {
            AddStationWindow ASW = new();
            ASW.ShowDialog();
            StationNames = _parser.GetStationNamesList();
        }
        void Exit()
        {
            Environment.Exit(0);
        }

        void OpenSettings()
        {
            SettingsWindow SW = new(Settings);
            SW.ShowDialog();
        }
    }
}
