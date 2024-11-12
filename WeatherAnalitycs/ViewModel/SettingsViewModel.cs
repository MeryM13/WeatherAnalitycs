using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalitycs.Utility;

namespace WeatherAnalitycs.ViewModel
{
    internal class SettingsViewModel : BaseViewModel
    {
        Window _window;
        SettingsClass _settingsClass = new();

        public SettingsClass Settings
        {
            get => _settingsClass;
            set
            {
                _settingsClass = value;
                OnPropertyChanged(nameof(Settings));
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
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExitCommand = new RelayCommand(Exit);
            ResetDatabaseCommand = new RelayCommand(ResetDatabase);
            RecreateDatabaseCommand = new RelayCommand(RecreateDatabase);
            OpenFileExplorerForExcelPathCommand = new RelayCommand(OpenFileExplorerForExcelPath);
        }

        void SaveSettings()
        {
            _settingsClass.SaveToFile();
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
