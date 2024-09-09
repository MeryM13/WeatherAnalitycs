using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WeatherAnalitycs.View;
using WeatherDataParser;

namespace WeatherAnalytics.View
{
    public partial class MainWindow : Window
    {
        readonly Parser _parser;
        Statistics _stat;
        int _stationID;
        string _stationName;
        DateTime _from = new(2012,01,01);
        DateTime _to = DateTime.Now;
        readonly Dictionary<string,Parameter> parameterDict = new()
        {
            {"Скорость ветра", Parameter.Wind_Speed },
            {"Температура воздуха", Parameter.Temperature },
            {"Относительная влажность воздуха", Parameter.Humidity },
            {"Атмосферное давление", Parameter.Pressure },
            {"Высота снежного покрова", Parameter.Snow_Height },
        };
        readonly Dictionary<string, DateInterval> intervalDict = new()
        {
            {"День", DateInterval.Day },
            {"Неделя", DateInterval.WeekOfYear },
            {"Месяц", DateInterval.Month },
            {"Квартал", DateInterval.Quarter },
            {"Год", DateInterval.Year },
        };

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                _parser = new Parser();
                UpdateStationCmb();
                dateFrom.Text = _from.ToString();
                dateTo.Text = _to.ToString();
                _stat = new(_from, _to, _stationID);

                foreach (string parameterName in parameterDict.Keys)
                    cmbParameter.Items.Add(parameterName);

                foreach (string intervalName in intervalDict.Keys)
                {
                    cmbTimeInterval.Items.Add(intervalName);
                    cmbPeriodicityTimeInterval.Items.Add(intervalName);
                }

                cmbParameter.SelectedItem = "Температура воздуха";
                cmbTimeInterval.SelectedItem = "Квартал";
                cmbPeriodicityTimeInterval.SelectedItem = "Квартал";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка");
                Environment.Exit(0);
            }
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
        private void CmbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_updating)
            {
                _stationName = cmbStations.SelectedItem.ToString();
                _stationID = int.Parse(_stationName.Split('(', ')')[1]);
                txtStationInfo.Text = _parser.GetStationInfo(_stationID);
                _stat = new(_from, _to, _stationID);
            }
        }
        private void DateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _from = (DateTime)dateFrom.SelectedDate;
            _stat = new(_from, _to, _stationID);
        }
        private void DateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _to = (DateTime)dateTo.SelectedDate;
            _stat = new(_from, _to, _stationID);
        }

        //Buttons generic
        private void BtnAddStation_Click(object sender, RoutedEventArgs e)
        {
            AddStationWindow addStationWindow1 = new();
            AddStationWindow addStationWindow = addStationWindow1;
            addStationWindow.ShowDialog();
            UpdateStationCmb();
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataUpdateWindow dataUpdateWindow = new();
            dataUpdateWindow.ShowDialog();
        }
        
        //Periodicity chart
        decimal? _direction = null;
        DateInterval _periodicityInterval = DateInterval.Month;
        private void BtnGetPeriodicityChart_Click(object sender, RoutedEventArgs e)
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
            string periodName = "";
            switch (_periodicityInterval)
            {
                case DateInterval.Day:
                    {
                        periodName = "день";
                        break;
                    }
                case DateInterval.WeekOfYear:
                    {
                        periodName = "неделя";
                        break;
                    }
                case DateInterval.Month:
                    {
                        periodName = "месяц";
                        break;
                    }
                case DateInterval.Quarter:
                    {
                        periodName = "квартал";
                        break;
                    }
                case DateInterval.Year:
                    {
                        periodName = "год";
                        break;
                    }
            }
            string title = $"График повторяемости {directionName} с интервалом {periodName} для станции {_stationName} за период с {_from:d} до {_to:d}";
            DisplayWindow window = new(title, _stat.GetWindPeriodicityChart(_direction, _periodicityInterval));
            window.Show();
        }

        private void RbCalm_Checked(object sender, RoutedEventArgs e)
        {
            _direction = null;
        }

        private void RbNorth_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 0;
        }

        private void RbNorthEast_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 45;
        }

        private void RbEast_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 90;
        }

        private void RbSouthEast_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 135;
        }

        private void RbSouth_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 180;
        }

        private void RbSouthWest_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 225;
        }

        private void RbWest_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 270;
        }

        private void RbNorthWest_Checked(object sender, RoutedEventArgs e)
        {
            _direction = 315;
        }

        private void CmbPeriodicityTimeInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _periodicityInterval = intervalDict[cmbPeriodicityTimeInterval.SelectedValue.ToString()];
        }

        //Averages chart
        Parameter _parameter = Parameter.Temperature;
        DateInterval _averagesInterval = DateInterval.Month;
        private void BtnGetAveragesChart_Click(object sender, RoutedEventArgs e)
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
            string title = $"График среднe{average} значений {parameterName} для станции {_stationName} за период с {_from:d} до {_to:d}";
            DisplayWindow window = new(title, _stat.GetAveragesChart(_parameter, _averagesInterval));
            window.Show();
        }

        private void CmbParameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _parameter = parameterDict[cmbParameter.SelectedValue.ToString()];
        }

        private void CmbTimeInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _averagesInterval = intervalDict[cmbTimeInterval.SelectedValue.ToString()];
        }

        //Wind rose
        int _numberOfDirections = 8;
        bool _distributeCalm = false;
        private void BtnGetWindRose_Click(object sender, RoutedEventArgs e)
        {
            string title = $"{_numberOfDirections}-румбовая роза ветров {(_distributeCalm? "с распределением штилей" : "без распределения штилей")} для станции {_stationName} за период с {_from:d} до {_to:d}";
            DisplayWindow window = new(title, _stat.GetWindRose(_distributeCalm, _numberOfDirections));
            window.Show();
        }

        private void Rb8Directions_Checked(object sender, RoutedEventArgs e)
        {
            _numberOfDirections = 8;
        }

        private void Rb16Directions_Checked(object sender, RoutedEventArgs e)
        {
            _numberOfDirections = 16;
        }

        private void ChkDistributeCalm_Checked(object sender, RoutedEventArgs e)
        {
            _distributeCalm = true;
        }

        private void ChkDistributeCalm_Unchecked(object sender, RoutedEventArgs e)
        {
            _distributeCalm = false;
        }

        //Calm and low speed winds stats
        bool _getCalmCount = false;
        bool _getCalmPeriodicity = false;
        int _calmCount = 0;
        decimal _calmPeriodicity = 0;

        bool _getLowCount = false;
        bool _getLowPeriodicity = false;
        int _lowCount = 0;
        decimal _lowPeriodicity = 0;

        private void BtnGetCalmStats_Click(object sender, RoutedEventArgs e)
        {
            if (_getCalmCount)
            {
                _calmCount = _stat.GetCalmCount();
                txtCalmCount.Text = _calmCount.ToString();
            }
            else
            {
                _calmCount = 0;
                txtCalmCount.Text = string.Empty;
            }

            if (_getCalmPeriodicity)
            {
                _calmPeriodicity = _stat.GetCalmPeriodicity();
                txtCalmPeriodicity.Text = _calmPeriodicity.ToString();
            }
            else
            {
                _calmPeriodicity = 0;
                txtCalmPeriodicity.Text = string.Empty;
            }

            if (_getLowCount)
            {
                _lowCount = _stat.GetWeakCount();
                txtLowCount.Text = _lowCount.ToString();
            }
            else
            {
                _lowCount = 0;
                txtLowCount.Text = string.Empty;
            }

            if (_getLowPeriodicity)
            {
                _lowPeriodicity = _stat.GetWeakPeriodicity();
                txtLowPeriodicity.Text = _lowPeriodicity.ToString();
            }
            else
            {
                _lowPeriodicity = 0;
                txtLowPeriodicity.Text = string.Empty;
            }
        }

        private void chkGetLowCount_Checked(object sender, RoutedEventArgs e)
        {
            _getLowCount = true;
        }
        private void chkGetLowCount_Unchecked(object sender, RoutedEventArgs e)
        {
            _getLowCount = false;
        }
        private void chkGetLowPeriodicity_Checked(object sender, RoutedEventArgs e)
        {
            _getLowPeriodicity = true;
        }
        private void chkGetLowPeriodicity_Unchecked(object sender, RoutedEventArgs e)
        {
            _getLowPeriodicity = false;
        }
        private void ChkGetCalmCount_Checked(object sender, RoutedEventArgs e)
        {
            _getCalmCount = true;
        }

        private void ChkGetCalmCount_Unchecked(object sender, RoutedEventArgs e)
        {
            _getCalmCount = false;
        }

        private void ChkGetCalmPeriodicity_Checked(object sender, RoutedEventArgs e)
        {
            _getCalmPeriodicity = true;
        }

        private void ChkGetCalmPeriodicity_Unchecked(object sender, RoutedEventArgs e)
        {
            _getCalmPeriodicity = false;
        }

        //Table
        readonly List<Parameter> _parameters = new();
        bool _useStationName = false;
        bool _divideDateAndTime = true;
        bool _dateOnly = false;
        private void BtnGetTable_Click(object sender, RoutedEventArgs e)
        {
            if (_parameters.Count == 0)
            {
                MessageBox.Show("Выберите столбцы для построения таблицы");
                return;
            }

            string title = $"Таблица метеоданных для станции {_stationName} за период с {_from:d} до {_to:d}";

            DisplayWindow window = new(title, _stat.GetRawData(_parameters, _useStationName, _divideDateAndTime, _dateOnly));
            window.Show();
        }

        private void ChkStationName_Checked(object sender, RoutedEventArgs e)
        {
            chkStationID.IsChecked = false;
            if (!_parameters.Contains(Parameter.Station))
            {
                _parameters.Add(Parameter.Station);
            }
            _useStationName = true;
        }

        private void ChkStationName_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_parameters.Contains(Parameter.Station))
            {
                _parameters.Remove(Parameter.Station);
            }
            _useStationName = false;
        }

        private void ChkStationID_Checked(object sender, RoutedEventArgs e)
        {
            chkStationName.IsChecked = false;
            if (!_parameters.Contains(Parameter.Station))
            {
                _parameters.Add(Parameter.Station);
            }
        }
        private void ChkStationID_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_parameters.Contains(Parameter.Station))
            {
                _parameters.Remove(Parameter.Station);
            }
        }

        private void ChkDate_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Date);
            chkTime.IsEnabled = true;
            _divideDateAndTime = true;
            _dateOnly = true;
        }

        private void ChkDate_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Date);
            chkTime.IsChecked = false;
            chkTime.IsEnabled = false;
            chkDateAndTime.IsChecked = false;
            chkDateAndTime.IsEnabled = false;
        }

        private void ChkTime_Checked(object sender, RoutedEventArgs e)
        {
            _dateOnly = false;
            _divideDateAndTime = true;
            chkDateAndTime.IsEnabled = true;
        }

        private void ChkTime_Unchecked(object sender, RoutedEventArgs e)
        {
            _dateOnly = true;
            chkDateAndTime.IsChecked = false;
            chkDateAndTime.IsEnabled = false;
        }

        private void ChkDateAndTime_Checked(object sender, RoutedEventArgs e)
        {
            _divideDateAndTime = false;
            _dateOnly = false;
        }

        private void ChkDateAndTime_Unchecked(object sender, RoutedEventArgs e)
        {
            _divideDateAndTime = true;
        }

        private void ChkWindDirection_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Wind_Direction);
        }

        private void ChkWindDirection_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Wind_Direction);
        }

        private void ChkWindSpeed_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Wind_Speed);
        }

        private void ChkWindSpeed_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Wind_Speed);
        }

        private void ChkTemperature_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Temperature);
        }

        private void ChkTemperature_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Temperature);
        }

        private void ChkHumidity_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Humidity);
        }

        private void ChkHumidity_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Humidity);
        }

        private void ChkPressure_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Pressure);
        }

        private void ChkPressure_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Pressure);
        }

        private void ChkSnowHeight_Checked(object sender, RoutedEventArgs e)
        {
            _parameters.Add(Parameter.Snow_Height);
        }

        private void ChkSnowHeight_Unchecked(object sender, RoutedEventArgs e)
        {
            _parameters.Remove(Parameter.Snow_Height);
        }
    }
}
