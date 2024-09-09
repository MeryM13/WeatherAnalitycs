using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.View;
using WeatherDataParser;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemTableViewModel:BaseTabItemViewModel
    {
        readonly List<Parameter> _parameters = new();
        bool _useStationName = false;
        bool _divideDateAndTime = false;
        bool _dateOnly = true;

        public bool UseStationName
        {
            get { return _useStationName; }
            set { 
                _useStationName = value;
                if (_useStationId == _useStationName)
                    _useStationId = !_useStationName;
                ChangeState(_useStationName, Parameter.Station);
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                    CombineCheckEnabled = false;
                ChangeState(_useDate, Parameter.Date);
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public TabItemTableViewModel(SearchParamsStore store): base(store) 
        {
            ButtonPressCommand = new RelayCommand(OpenTable);
        }

        void OpenTable()
        {
            if (_parameters.Count == 0)
            {
                MessageBox.Show("Выберите столбцы для построения таблицы");
                return;
            }

            string title = $"Таблица метеоданных для станции {SearchParamsStore.StationId} за период с {SearchParamsStore.From:d} до {SearchParamsStore.To:d}";
            Statistics _stat = new(SearchParamsStore.From, SearchParamsStore.To,SearchParamsStore.StationId);
            DisplayWindow window = new(title, _stat.GetRawData(_parameters, _useStationName, _divideDateAndTime, _dateOnly));
            window.Show();
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
                if (_parameters.Contains(param))
                    _parameters.Remove(param);
            }
        }
    }
}
