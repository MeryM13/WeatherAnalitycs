using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherDataParser;

namespace WeatherAnalytics.View
{
    /// <summary>
    /// Логика взаимодействия для DataUpdateWindow.xaml
    /// </summary>
    public partial class DataUpdateWindow : Window
    {
        List <int> _updateStations = new List <int> ();
        bool _doFullUpdate = true;
        Parser _parser = new Parser ();
        public DataUpdateWindow()
        {
            InitializeComponent();
            int x = 0;
            int y = 0;
            foreach (string station in _parser.GetStationNamesList())
            {
                CheckBox check = new();
                check.Content = station;
                check.Checked += StationCheckbox_Checked;
                check.Unchecked += StationCheckbox_Unchecked;
                check.FontSize = 22;
                check.FontFamily = new FontFamily("Bahnschrift SemiLight");

                check.Margin = new Thickness(x * 250 + 10, y * 25 + 10, 0, 0);
                if (y < 5)
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

        private void rbUpdateAll_Checked(object sender, RoutedEventArgs e)
        {
            _doFullUpdate = true;
        }

        private void rbChooseUpgrades_Checked(object sender, RoutedEventArgs e)
        {
            _doFullUpdate = false;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_doFullUpdate)
            {
                _parser.FullUpdate();
                this.Close();
            }
            foreach (int station in _updateStations) 
            {
                _parser.UpdateStationData(station);
            }
            this.Close();
        }
    }
}
