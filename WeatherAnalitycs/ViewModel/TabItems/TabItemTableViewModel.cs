using CommunityToolkit.Mvvm.Input;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.Utility.ViewModelBases;
using WeatherAnalitycs.View;
using WeatherDataParser;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemTableViewModel:BaseTabItemViewModel
    {
        readonly List<Parameter> _parameters = [];
        bool _useStationName = true;
        bool _divideDateAndTime = true;
        bool _dateOnly = true;

        public bool UseStationName
        {
            get { return _useStationName; }
            set { 
                _useStationName = value;
                if (_useStationId == _useStationName)
                    _useStationId = !_useStationName;
                ChangeState(_useStationName, Parameter.Station);
                OnPropertyChanged(nameof(UseStationName));
                OnPropertyChanged(nameof(UseStationId));
            }
        }

        bool _useStationId;
        public bool UseStationId
        {
            get { return _useStationId; }
            set
            {
                _useStationId = value;
                if (_useStationName == _useStationId)
                    _useStationName = !_useStationId;
                ChangeState(_useStationId, Parameter.Station);
                OnPropertyChanged(nameof(UseStationName));
                OnPropertyChanged(nameof(UseStationId));
            }
        }

        public bool TimeCheckEnabled { get; set; } = false;
        public bool CombineCheckEnabled { get; set; } = false;
        bool _useDate;
        public bool UseDate
        {
            get { return _useDate; }
            set
            {
                _useDate = value;
                TimeCheckEnabled = value;
                if (value == false)
                {
                    CombineCheckEnabled = false;
                    _useTime = false;
                    _combine = false;
                    OnPropertyChanged(nameof(UseTime));
                    OnPropertyChanged(nameof(Combine));
                    OnPropertyChanged(nameof(CombineCheckEnabled));
                }
                ChangeState(_useDate, Parameter.Date);
                OnPropertyChanged(nameof(UseDate));
                OnPropertyChanged(nameof(TimeCheckEnabled));
            }
        }

        bool _useTime;
        public bool UseTime
        {
            get { return _useTime; }
            set
            {
                _useTime = value;
                CombineCheckEnabled = value;
                _dateOnly = !value;
                OnPropertyChanged(nameof(UseTime));
                OnPropertyChanged(nameof(CombineCheckEnabled));
            }
        }

        bool _combine;
        public bool Combine
        {
            get { return _combine; }
            set
            {
                _combine = value;
                _divideDateAndTime = !value;
                OnPropertyChanged(nameof(Combine));
            }
        }

        bool _useWindDirection;
        public bool UseWindDirection
        {
            get { return _useWindDirection; }
            set
            {
                _useWindDirection = value;
                ChangeState(_useWindDirection, Parameter.Wind_Direction);
                OnPropertyChanged(nameof(UseWindDirection));
            }
        }

        bool _useWindSpeed;
        public bool UseWindSpeed
        {
            get { return _useWindSpeed; }
            set
            {
                _useWindSpeed = value;
                ChangeState(_useWindSpeed, Parameter.Wind_Speed);
                OnPropertyChanged(nameof(UseWindSpeed));
            }
        }

        bool _useTemperature;
        public bool UseTemperature
        {
            get { return _useTemperature; }
            set
            {
                _useTemperature = value;
                ChangeState(_useTemperature, Parameter.Temperature);
                OnPropertyChanged(nameof(UseTemperature));
            }
        }

        bool _useHumidity;
        public bool UseHumidity
        {
            get { return _useHumidity; }
            set
            {
                _useHumidity = value;
                ChangeState(_useHumidity, Parameter.Humidity);
                OnPropertyChanged(nameof(UseHumidity));
            }
        }

        bool _usePressure;
        public bool UsePressure
        {
            get { return _usePressure; }
            set
            {
                _usePressure = value;
                ChangeState(_usePressure, Parameter.Pressure);
                OnPropertyChanged(nameof(UsePressure));
            }
        }

        bool _useSnowHeight;
        public bool UseSnowHeight
        {
            get { return _useSnowHeight; }
            set
            {
                _useSnowHeight = value;
                ChangeState(_useSnowHeight, Parameter.Snow_Height);
                OnPropertyChanged(nameof(UseSnowHeight));
            }
        }

        public TabItemTableViewModel(SearchParamsStore store, SettingsClass settings): base(store, settings) 
        {
            ButtonPressCommand = new RelayCommand(OpenTable);
            ConvertToExcelCommand = new RelayCommand(CreateExcelSheet);
        }

        void OpenTable()
        {
            if (_parameters.Count == 0)
            {
                MessageBox.Show("Выберите столбцы для построения таблицы");
                return;
            }

            string title = $"Таблица метеоданных для станции {Store.StationId} за период с {Store.From:d} до {Store.To:d}";
            Statistics _stat = new(Store.From, Store.To,Store.StationId, Settings.DatabaseConnectionString, Settings.DatabaseServer);
            int entries = _stat.GetAll();
            DisplayWindow window = new(title, entries, _stat.GetRawData(_parameters, _useStationName, _divideDateAndTime, _dateOnly), Settings);
            window.Show();
        }

       async void CreateExcelSheet()
        {
            if (_parameters.Count == 0)
            {
                MessageBox.Show("Выберите столбцы для построения таблицы");
                return;
            }
            Statistics _stat = new(Store.From, Store.To, Store.StationId, Settings.DatabaseConnectionString, Settings.DatabaseServer);
            var converter = new ExcelConverter(Settings.ExcelSavePath);
            await Task.Run(() =>
            {
                if (Settings.DivideExcelTable)
                    converter.Convert($"{Store.StationId}_Данные_{Store.From:d}-{Store.To:d}",
                        _stat.GetRawData(_parameters, _useStationName, _divideDateAndTime, _dateOnly), Settings.ExcelTableDivider);
                else
                    converter.Convert($"{Store.StationId}_Данные_{Store.From:d}-{Store.To:d}",
                    _stat.GetRawData(_parameters, _useStationName, _divideDateAndTime, _dateOnly));
            });
            MessageBox.Show("Таблица создана");
        }

        void ChangeState(bool state, Parameter param)
        {
            if (state)
            {
                if (!_parameters.Contains(param))
                    _parameters.Add(param);
            }
            else
            {
                _parameters.Remove(param);
            }
        }
    }
}
