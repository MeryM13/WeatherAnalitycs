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

namespace WeatherAnalitycs.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        readonly Parser _parser = new();
        public List<string> StationNames { get; set; }
        
        string _selectedStation;
        public string SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                SearchStore.StationId = int.Parse(_selectedStation.Split('(', ')')[1]);
                OnPropertyChanged(nameof(SelectedStation));
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

        public MainViewModel()
        {
            StationNames = _parser.GetStationNamesList();
            SelectedStation = StationNames[0];
            UpdateDataCommand = new RelayCommand(UpdateData);
            AddNewStationCommand = new RelayCommand(AddNewStation);
            ExitCommand = new RelayCommand(Exit);

            TableViewModel = new(SearchStore);
            RepeatViewModel = new(SearchStore);
            AverageViewModel = new(SearchStore);
            WindroseViewModel = new(SearchStore);
            CalmcountViewModel = new(SearchStore);
        }

        void UpdateData()
        {
            DataUpdateWindow DUW = new();
            DUW.ShowDialog();
        }
        void AddNewStation()
        {
            AddStationWindow ASW = new();
            ASW.ShowDialog();
        }
        void Exit()
        {
            Environment.Exit(0);
        }
    }
}
