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
using WeatherAnalitycs.View;
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
        string _stationName;
        DateTime _from = new(2012,01,01);
        DateTime _to = DateTime.Now;
        public MainWindow()
        {
            InitializeComponent();
            _parser = new Parser();
            UpdateStationCmb();
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

            cmbPeriodicityTimeInterval.Items.Add(DateInterval.Day);
            cmbPeriodicityTimeInterval.Items.Add(DateInterval.WeekOfYear);
            cmbPeriodicityTimeInterval.Items.Add(DateInterval.Month);
            cmbPeriodicityTimeInterval.Items.Add(DateInterval.Quarter);
            cmbPeriodicityTimeInterval.Items.Add(DateInterval.Year);
        }

        bool _updating = false;
        private void UpdateStationCmb()
        {
            _updating = true;
            cmbStations.Items.Clear();
            foreach (string station in _parser.GetStationNamesList())
            {
                cmbStations.Items.Add(station);
            }
            _updating = false;
            cmbStations.SelectedIndex = 0;
        }

        //Setting params
        private void cmbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_updating)
            {
                _stationName = cmbStations.SelectedItem.ToString();
                _stationID = int.Parse(_stationName.Split('(', ')')[1]);
                txtStationInfo.Text = _parser.GetStationInfo(_stationID);
                _stat = new(_from, _to, _stationID);
            }
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

        //Buttons generic
        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            AddStationWindow addStationWindow = new AddStationWindow();
            addStationWindow.ShowDialog();
            UpdateStationCmb();
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

        
        //Periodicity chart
        decimal? _direction = null;
        DateInterval _periodicityInterval = DateInterval.Month;
        private void btnGetPeriodicityChart_Click(object sender, RoutedEventArgs e)
        {
            string directionName = "";
            switch (_direction)
            {
                case 0:
                    {
                        directionName = "северного ветра";
                        break;
                    }
                case 45:
                    {
                        directionName = "северо-восточного ветра";
                        break;
                    }
                case 90:
                    {
                        directionName = "восточного ветра";
                        break;
                    }
                case 135:
                    {
                        directionName = "юго-восточного ветра";
                        break;
                    }
                case 180:
                    {
                        directionName = "южного ветра";
                        break;
                    }
                case 225:
                    {
                        directionName = "юго-западного ветра";
                        break;
                    }
                case 270:
                    {
                        directionName = "западного ветра";
                        break;
                    }
                case 315:
                    {
                        directionName = "северо-западного ветра";
                        break;
                    }
                case null:
                    {
                        directionName = "штилей";
                        break;
                    }
            }
            string title = $"График повторяемости {directionName} для станции {_stationName} за период с {_from.ToString("d")} до {_to.ToString("d")}";
            DisplayChartWindow window = new DisplayChartWindow(title, _stat.GetWindPeriodicityChart(_direction,_periodicityInterval));
            window.Show();
        }

        private void rbCalm_Checked(object sender, RoutedEventArgs e)
        {
            _direction = null;
        }

        private void rbNorth_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 0;
        }

        private void rbNorthEast_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 45;
        }

        private void rbEast_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 90;
        }

        private void rbSouthEast_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 135;
        }

        private void rbSouth_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 180;
        }

        private void rbSouthWest_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 225;
        }

        private void rbWest_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 270;
        }

        private void rbNorthWest_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 315;
        }

        private void cmbPeriodicityTimeInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _periodicityInterval = (DateInterval)cmbPeriodicityTimeInterval.SelectedValue;
        }

        //Averages chart
        Parameter _parameter = Parameter.Temperature;
        DateInterval _averagesInterval = DateInterval.Month;
        private void btnGetAveragesChart_Click(object sender, RoutedEventArgs e)
        {
            string average = "";
            switch (_averagesInterval)
            {
                case DateInterval.Day:
                    {
                        average = "суточных";
                        break;
                    }
                case DateInterval.WeekOfYear:
                    {
                        average = "недельных";
                        break;
                    }
                case DateInterval.Month:
                    {
                        average = "месячных";
                        break;
                    }
                case DateInterval.Quarter:
                    {
                        average = "квартальных";
                        break;
                    }
                case DateInterval.Year:
                    {
                        average = "годовых";
                        break;
                    }
            }
            string parameterName = "";
            switch (_parameter)
            {
                case Parameter.Wind_Speed:
                    {
                        parameterName = "скорости ветра";
                        break;
                    }
                case Parameter.Temperature:
                    {
                        parameterName = "температуры";
                        break;
                    }
                case Parameter.Humidity:
                    {
                        parameterName = "влажности воздуха";
                        break;
                    }
                case Parameter.Pressure:
                    {
                        parameterName = "атмосферного давления";
                        break;
                    }
                case Parameter.Snow_Height:
                    {
                        parameterName = "высоты снежного покрова";
                        break;
                    }
            }
            string title = $"График среднe{average} значений {parameterName} для станции {_stationName} за период с {_from.ToString("d")} до {_to.ToString("d")}";
            DisplayChartWindow window = new DisplayChartWindow(title, _stat.GetAveragesChart(_parameter, _averagesInterval));
            window.Show();
        }

        private void cmbParameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _parameter = (Parameter)cmbParameter.SelectedValue;
        }

        private void cmbTimeInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _averagesInterval = (DateInterval)cmbTimeInterval.SelectedValue;
        }

        //Wind rose
        int _numberOfDirections = 8;
        bool _distributeCalm = false;
        private void btnGetWindRose_Click(object sender, RoutedEventArgs e)
        {
            string title = $"{_numberOfDirections}-румбовая роза ветров {(_distributeCalm? "с распределением штилей" : "без распределения штилей")} для станции {_stationName} за период с {_from.ToString("d")} до {_to.ToString("d")}";
            DisplayRoseWindow window = new DisplayRoseWindow(title, _stat.GetWindRose(_distributeCalm, _numberOfDirections));
            window.Show();
        }

        private void rb8Directions_Checked(object sender, RoutedEventArgs e)
        {
            _numberOfDirections = 8;
        }

        private void rb16Directions_Checked(object sender, RoutedEventArgs e)
        {
            _numberOfDirections = 16;
        }

        private void chkDistributeCalm_Checked(object sender, RoutedEventArgs e)
        {
            _distributeCalm = true;
        }

        private void chkDistributeCalm_Unchecked(object sender, RoutedEventArgs e)
        {
            _distributeCalm = false;
        }

        //Calm stats
        bool _getCount = false;
        bool _getPeriodicity = false;
        int _calmCount = 0;
        decimal _calmPeriodicity = 0;

        private void btnGetCalmStats_Click(object sender, RoutedEventArgs e)
        {
            if (_getCount)
            {
                _calmCount = _stat.GetCalmCount();
                txtCalmCount.Text = _calmCount.ToString();
            }
            else
            {
                _calmCount = 0;
                txtCalmCount.Text = string.Empty;
            }

            if (_getPeriodicity)
            {
                _calmPeriodicity = _stat.GetCalmPeriodicity();
                txtCalmPeriodicity.Text = _calmPeriodicity.ToString();
            }
            else
            {
                _calmPeriodicity = 0;
                txtCalmPeriodicity.Text = string.Empty;
            }
        }

        private void chkGetCalmCount_Checked(object sender, RoutedEventArgs e)
        {
            _getCount = true;
        }

        private void chkGetCalmCount_Unchecked(object sender, RoutedEventArgs e)
        {
            _getCount = false;
        }

        private void chkGetCalmPeriodicity_Checked(object sender, RoutedEventArgs e)
        {
            _getPeriodicity = true;
        }

        private void chkGetCalmPeriodicity_Unchecked(object sender, RoutedEventArgs e)
        {
            _getPeriodicity = false;
        }

        //Table
        List<Parameter> _parameters = new List<Parameter>();
        bool _useStationName = false;
        bool _divideDateAndTime = true;
        bool _dateOnly = false;
        private void btnGetTable_Click(object sender, RoutedEventArgs e)
        {
            string title = $"Таблица метеоданных для станции {_stationName} за период с {_from.ToString("d")} до {_to.ToString("d")}";

            DisplayTableWindow window = new DisplayTableWindow(_stat.GetRawData(_parameters, _useStationName, _divideDateAndTime, _dateOnly), title);
            window.Show();
        }

        private void chkStationName_Checked(object sender, RoutedEventArgs e)
        {
            chkStationID.IsChecked = false;
            if (!_parameters.Contains(Parameter.Station))
            {
                _parameters.Add(Parameter.Station);
            }
            _useStationName = true;
        }

        private void chkStationName_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_parameters.Contains(Parameter.Station))
            {
                _parameters.Remove(Parameter.Station);
            }
            _useStationName = false;
        }

        private void chkStationID_Checked(object sender, RoutedEventArgs e)
        {
            chkStationName.IsChecked = false;
            if (!_parameters.Contains(Parameter.Station))
            {
                _parameters.Add(Parameter.Station);
            }
        }
        private void chkStationID_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_parameters.Contains(Parameter.Station))
            {
                _parameters.Remove(Parameter.Station);
            }
        }

        private void chkDate_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Date);
            chkTime.IsEnabled = true;
            _divideDateAndTime = true;
            _dateOnly = true;
        }

        private void chkDate_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Date);
            chkTime.IsChecked = false;
            chkTime.IsEnabled = false;
            chkDateAndTime.IsChecked = false;
            chkDateAndTime.IsEnabled = false;
        }

        private void chkTime_Checked(object sender, RoutedEventArgs e)
        {
            _dateOnly = false;
            _divideDateAndTime = true;
            chkDateAndTime.IsEnabled = true;
        }

        private void chkTime_Unchecked(object sender, RoutedEventArgs e)
        {
            _dateOnly = true;
            chkDateAndTime.IsChecked = false;
            chkDateAndTime.IsEnabled = false;
        }

        private void chkDateAndTime_Checked(object sender, RoutedEventArgs e)
        {
            _divideDateAndTime = false;
            _dateOnly = false;
        }

        private void chkDateAndTime_Unchecked(object sender, RoutedEventArgs e)
        {
            _divideDateAndTime = true;
        }

        private void chkWindDirection_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Wind_Direction);
        }

        private void chkWindDirection_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Wind_Direction);
        }

        private void chkWindSpeed_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Wind_Speed);
        }

        private void chkWindSpeed_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Wind_Speed);
        }

        private void chkTemperature_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Temperature);
        }

        private void chkTemperature_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Temperature);
        }

        private void chkHumidity_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Humidity);
        }

        private void chkHumidity_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Humidity);
        }

        private void chkPressure_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Pressure);
        }

        private void chkPressure_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Pressure);
        }

        private void chkSnowHeight_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Snow_Height);
        }

        private void chkSnowHeight_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Snow_Height);
        }
    }
}
