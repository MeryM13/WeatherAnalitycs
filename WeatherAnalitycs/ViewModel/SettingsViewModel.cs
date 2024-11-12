using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using WeatherAnalitycs.Utility;

namespace WeatherAnalitycs.ViewModel
{
    internal class SettingsViewModel : BaseViewModel
    {
        Window _window;
        SettingsClass _settingsClass = new();

        readonly Dictionary<string, DateInterval> intervalDict = new()
        {
            {"День", DateInterval.Day },
            {"Неделя", DateInterval.WeekOfYear },
            {"Месяц", DateInterval.Month },
            {"Квартал", DateInterval.Quarter },
            {"Год", DateInterval.Year },
        };

        public SettingsClass Settings
        {
            get => _settingsClass;
            set
            {
                _settingsClass = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public List<string> DatabasesNames { get; set; }
        public List<string> WindRoseDataTypes { get; set; }
        public List<string> Intervals { get => intervalDict.Select(x => x.Key).ToList(); }
        public string IntervalName
        {
            get => intervalDict.FirstOrDefault(x => x.Value == Settings.ExcelTableDivider).Key;
            set
            {
                Settings.ExcelTableDivider = intervalDict[value];
                OnPropertyChanged(nameof(IntervalName));
            }
        }

        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ResetDatabaseCommand {  get; set; }
        public RelayCommand RecreateDatabaseCommand { get; set; }
        public RelayCommand OpenFileExplorerForExcelPathCommand {  get; set; }

        public SettingsViewModel(Window window)
        {
            _window = window;
            Load();
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExitCommand = new RelayCommand(Exit);
            ResetDatabaseCommand = new RelayCommand(ResetDatabase);
            RecreateDatabaseCommand = new RelayCommand(RecreateDatabase);
            OpenFileExplorerForExcelPathCommand = new RelayCommand(OpenFileExplorerForExcelPath);
        }

        async void Load()
        {
            DatabasesNames = ["SQLite", "MSSQL"];
            WindRoseDataTypes = ["Проценты", "Числа"];
            await _settingsClass.LoadFromFile();
        }

        async void SaveSettings()
        {
            await _settingsClass.SaveToFile();
            _window.Close();
        }

        void Exit()
        {
            _window.Close();
        }

        void ResetDatabase()
        {

        }

        void RecreateDatabase()
        {

        }

        void OpenFileExplorerForExcelPath()
        {
            var dlg = new CommonOpenFileDialog()
            {
                IsFolderPicker = true
            };
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Settings.ExcelSavePath = dlg.FileName;
            }
        }
    }
}
