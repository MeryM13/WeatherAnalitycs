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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherAnalytics.View;
using WeatherDataParser;

namespace WeatherAnalitycs.View
{
    /// <summary>
    /// Логика взаимодействия для GetTableUC.xaml
    /// </summary>
    public partial class GetTableUC : UserControl
    {
        Statistics _stat;
        int _stationID;
        DateTime _from = new(2012, 01, 01);
        DateTime _to = DateTime.Now;
        public GetTableUC(Statistics stat, int stationID, DateTime from, DateTime to)
        {
            InitializeComponent();
            _stat = stat;
            _stationID = stationID;
            _from = from;
            _to = to;
        }

        private void btnGetTable_Click(object sender, RoutedEventArgs e)
        {
            List<Parameter> parameters = new List<Parameter>();
            bool useStationName = false;
            bool divideDateAndTime = true;
            bool dateOnly = false;

            if (chkStationID.IsChecked == true)
            {
                parameters.Add(Parameter.Station);
            }

            if (chkStationName.IsChecked == true)
            {
                useStationName = true;
            }

            if (chkDate.IsChecked == true)
            {
                parameters.Add(Parameter.Date);
                if (chkTime.IsChecked == false)
                {
                    dateOnly = true;
                }
            }

            if (chkDateAndTime.IsChecked == true)
            {
                divideDateAndTime = false;
            }

            if (chkWindDirection.IsChecked == true)
            {
                parameters.Add(Parameter.Wind_Direction);
            }

            if (chkWindSpeed.IsChecked == true)
            {
                parameters.Add(Parameter.Wind_Speed);
            }

            if (chkTemperature.IsChecked == true)
            {
                parameters.Add(Parameter.Temperature);
            }

            if (chkHumidity.IsChecked == true)
            {
                parameters.Add(Parameter.Humidity);
            }

            if (chkPressure.IsChecked == true)
            {
                parameters.Add(Parameter.Pressure);
            }

            if (chkSnowHeight.IsChecked == true)
            {
                parameters.Add(Parameter.Snow_Height);
            }

            string title = $"Таблица метеоданных для станции {_stationID} за период с {_from.ToString("d")} до {_to.ToString("d")}";

            DisplayTableWindow displayWindow = new DisplayTableWindow(_stat.GetRawData(parameters, useStationName, divideDateAndTime, dateOnly), title);
            displayWindow.Show();
        }

        private void chkStationName_Checked(object sender, RoutedEventArgs e)
        {
            chkStationID.IsChecked = false;
        }

        private void chkStationID_Checked(object sender, RoutedEventArgs e)
        {
            chkStationName.IsChecked = false;
        }

        private void chkDate_Checked(object sender, RoutedEventArgs e)
        {
            if (chkDate.IsChecked == true && chkTime.IsChecked == true)
            {
                chkDateAndTime.IsEnabled = true;
            }
            else { chkDateAndTime.IsEnabled = false; }
        }

        private void chkTime_Checked(object sender, RoutedEventArgs e)
        {
            if (chkDate.IsChecked == true && chkTime.IsChecked == true)
            {
                chkDateAndTime.IsEnabled = true;
            }
            else { chkDateAndTime.IsEnabled = false; }
        }
    }
}
