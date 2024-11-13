//using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.Utility.ViewModelBases;

namespace WeatherAnalitycs.ViewModel
{
    internal class SettingsViewModel : ViewModelBase
    {
        readonly Window _window;

        readonly Dictionary<string, DateInterval> intervalDict = new()
        {
            {"День", DateInterval.Day },
            {"Неделя", DateInterval.WeekOfYear },
            {"Месяц", DateInterval.Month },
            {"Квартал", DateInterval.Quarter },
            {"Год", DateInterval.Year },
        };

        readonly Dictionary<string, string> windRoseDataTypes = new()
        {
            {"Проценты", "Percent" },
            {"Абсолютное", "Count" }
        };

        readonly Dictionary<string, int> databases = new()
        {
            {"SQLite", 1 },
            {"MSSQL", 2 }
        };

        string _diffString;
        public string DiffString
        {
            get => _diffString;
            set
            {
                _diffString = value;
                OnPropertyChanged(nameof(DiffString));
            }
        }

        public ObservableRangeCollection<string> DatabasesNames { get; set; }
        public List<string> WindRoseDataTypes { get => windRoseDataTypes.Select(x => x.Key).ToList(); }
        public string TypeName
        {
            get => windRoseDataTypes.FirstOrDefault(x => x.Value == Settings.WindRoseDataType).Key;
            set
            {
                Settings.WindRoseDataType = windRoseDataTypes[value];
                OnPropertyChanged(nameof(TypeName));
                OnPropertyChanged(nameof(Settings));
            }
        }

        public List<string> Intervals { get => intervalDict.Select(x => x.Key).ToList(); }
        public string IntervalName
        {
            get => intervalDict.FirstOrDefault(x => x.Value == Settings.ExcelTableDivider).Key;
            set
            {
                Settings.ExcelTableDivider = intervalDict[value];
                OnPropertyChanged(nameof(IntervalName));
                OnPropertyChanged(nameof(Settings));
            }
        }

        public List<string> Databases { get => databases.Select(x => x.Key).ToList(); }
        public string DatabaseName
        {
            get => databases.FirstOrDefault(x => x.Value == Settings.DatabaseServer).Key;
            set
            {
                Settings.DatabaseServer = databases[value];
                OnPropertyChanged(nameof(IntervalName));
                OnPropertyChanged(nameof(Settings));
            }
        }

        public ICommand SaveSettingsCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ResetDatabaseCommand {  get; set; }
        public ICommand RecreateDatabaseCommand { get; set; }
        public ICommand OpenFileExplorerForExcelPathCommand {  get; set; }

        public SettingsViewModel(Window window, SettingsClass settings): base(settings)
        {
            _window = window;
            Load();
            SaveSettingsCommand = new Command(SaveSettings);
            ExitCommand = new Command(Exit);
            ResetDatabaseCommand = new Command(ResetDatabase);
            RecreateDatabaseCommand = new Command(RecreateDatabase);
            OpenFileExplorerForExcelPathCommand = new Command(OpenFileExplorerForExcelPath);
        }

        void Load()
        {
            DiffString = string.Join(", ", Settings.DifferentiateRoseSpeeds.Select(s => s.ToString()));
            OnPropertyChanged(nameof(Settings));
            OnPropertyChanged(nameof(DiffString));
        }

        void SaveSettings()
        {
            try
            {
                Settings.DifferentiateRoseSpeeds = [.. DiffString.Split(", ").Select(int.Parse).OrderDescending()];
            }
            catch 
            {
                MessageBox.Show("Проблема со значениями диференциации розы. Перепроверьте поле");
            }
            Settings.SaveToFile();
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
                OnPropertyChanged(nameof(Settings));
            }
        }
    }
}
