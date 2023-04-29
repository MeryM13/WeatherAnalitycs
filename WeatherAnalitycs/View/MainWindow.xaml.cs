using Microsoft.VisualBasic;
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
using WeatherDataParser.CLASSES;

namespace WeatherAnalytics.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Parser _parser;
        Statistics _stat;
        int _stationID;
        DateTime _from = new(2012,01,01);
        DateTime _to = DateTime.Now;
        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show("Вы уверены, что хотите обновить информацию о метеоданных следующих станций: все? \n Данное действие может занять много времени и не должно прерываться", "Обновление данных", MessageBoxButton.YesNoCancel);
            _parser = new Parser();
            cmbStations.Items.Clear();
            foreach (string station in _parser.GetStationNamesList())
            {
                cmbStations.Items.Add(station);
            }
            cmbStations.SelectedIndex = 0;
            dateFrom.Text = _from.ToString();
            dateTo.Text = _to.ToString();
            _stat = new(_from,_to,_stationID);

            cmbParameter.Items.Add(Parameter.Wind_Speed);
            cmbParameter.Items.Add(Parameter.Temperature);
            cmbParameter.Items.Add(Parameter.Humidity);
            cmbParameter.Items.Add(Parameter.Pressure);
            cmbParameter.Items.Add(Parameter.Snow_Height);

            cmbTimeInterval.Items.Add(DateInterval.Day);
            cmbTimeInterval.Items.Add(DateInterval.WeekOfYear);
            cmbTimeInterval.Items.Add(DateInterval.Month);
            cmbTimeInterval.Items.Add(DateInterval.Quarter);
            cmbTimeInterval.Items.Add(DateInterval.Year);
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            AddStationWindow addStationWindow = new AddStationWindow();
            addStationWindow.ShowDialog();
        }

        private void btnGetPeriodicityChart_Click(object sender, RoutedEventArgs e)
        {
            DisplayWindow displayWindow = new DisplayWindow();
            displayWindow.Show();
        }

        private void btnGetAveragesChart_Click(object sender, RoutedEventArgs e)
        {
            DisplayWindow displayWindow = new DisplayWindow();
            displayWindow.Show();
        }

        private void btnGetWindRose_Click(object sender, RoutedEventArgs e)
        {
            DisplayWindow displayWindow = new DisplayWindow();
            displayWindow.Show();
        }

        private void btnGetCalmStats_Click(object sender, RoutedEventArgs e)
        {
            if (chkGetCalmCount.IsChecked == true)
            {
                txtCalmCount.Text = _stat.GetCalmCount().ToString(); 
            }
            if (chkGetCalmPeriodicity.IsChecked == true)
            {
                txtCalmPeriodicity.Text = _stat.GetCalmPeriodicity().ToString();
            }
        }

        private void cmbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _stationID = int.Parse(cmbStations.SelectedItem.ToString().Split('(', ')')[1]);
            txtStationInfo.Text = _parser.GetStationInfo(_stationID);
            _stat = new(_from, _to, _stationID);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataUpdateWindow dataUpdateWindow = new DataUpdateWindow();
            dataUpdateWindow.ShowDialog();
        }
        private void dateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _from = (DateTime)dateFrom.SelectedDate;
            _stat = new(_from, _to, _stationID);
        }
        private void dateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _to = (DateTime)dateTo.SelectedDate;
            _stat = new(_from, _to, _stationID);
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
