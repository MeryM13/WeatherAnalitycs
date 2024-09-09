using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WeatherDataParser;

namespace WeatherAnalytics.View
{
    public partial class DataUpdateWindow : Window
    {
        readonly List <int> _updateStations = new();
        bool _doFullUpdate = true;
        readonly Parser _parser = new();
        public DataUpdateWindow()
        {
            InitializeComponent();
            int x = 0;
            int y = 0;
            foreach (string station in _parser.GetStationNamesList())
            {
                CheckBox check = new()
                {
                    Content = station
                };
                check.Checked += StationCheckbox_Checked;
                check.Unchecked += StationCheckbox_Unchecked;

                check.Margin = new Thickness(x * 300 + 10, y * 25 + 10, 0, 0);
                if (y < 6)
                {
                    y++;
                }
                else
                {
                    y = 0;
                    x++;
                }
                checkboxGrid.Children.Add(check);
            }
            rbUpdateAll.IsChecked = true;
        }

        private void StationCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            int stationID = int.Parse(check.Content.ToString().Split('(', ')')[1]);
            if (!_updateStations.Contains(stationID))
            {
                _updateStations.Add(stationID);
            }
        }

        private void StationCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            int stationID = int.Parse(check.Content.ToString().Split('(', ')')[1]);
            if (_updateStations.Contains(stationID))
            {
                _updateStations.Remove(stationID);
            }
        }

        private void RbUpdateAll_Checked(object sender, RoutedEventArgs e)
        {
            _doFullUpdate = true;
            checkboxGrid.IsEnabled = false;
        }

        private void RbChooseUpgrades_Checked(object sender, RoutedEventArgs e)
        {
            _doFullUpdate = false;
            checkboxGrid.IsEnabled = true;
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_doFullUpdate)
            {
                var result = MessageBox.Show("Вы уверены, что хотите обновить информацию о метеоданных для всех станций? \nДанное действие может занять много времени и не должно прерываться", "Обновление данных", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    ProgressBarWindow progress = new();
                    progress.Show();
                    await Task.Run(() => _parser.FullUpdate());
                    progress.Close();
                    this.Close();
                }
            }
            else
            {
                var result = MessageBox.Show($"Вы уверены, что хотите обновить информацию о метеоданных для следующих станций: {String.Join(',', _updateStations)} \nДанное действие может занять много времени и не должно прерываться", "Обновление данных", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    ProgressBarWindow progress = new();
                    progress.Show();
                    await Task.Run(() =>
                    {
                        foreach (int station in _updateStations)
                        {
                            _parser.UpdateStationData(station);
                        }
                    });
                    progress.Close();

                    this.Close();
                }
            }
        }
    }
}
