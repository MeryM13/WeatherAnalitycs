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
        Parser _parser;
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
            
            SearchStore.From = Settings.StartingDate;
            _parser = new Parser(Settings.StartingDate, Settings.DatabaseConnectionString, Settings.DatabaseServer);
            StationNames = _parser.GetStationNamesList();
            SelectedStation = StationNames[0];
            UpdateDataCommand = new RelayCommand(UpdateData);
            AddNewStationCommand = new RelayCommand(AddNewStation);
            ExitCommand = new RelayCommand(Exit);
            OpenSettingsCommand = new RelayCommand(OpenSettings);

            TableViewModel = new(SearchStore, Settings);
            RepeatViewModel = new(SearchStore, Settings);
            AverageViewModel = new(SearchStore, Settings);
            WindroseViewModel = new(SearchStore, Settings);
            CalmcountViewModel = new(SearchStore, Settings);
        }

        void UpdateData()
        {
            DataUpdateWindow DUW = new(Settings);
            DUW.ShowDialog();
        }
        void AddNewStation()
        {
            AddStationWindow ASW = new(Settings);
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
