﻿using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.View;
using WeatherDataParser;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemWindroseViewModel : BaseTabItemViewModel
    {
        SearchParamsStore _store;
        bool _distributeCalm, _8Directions = true;
        int _numberOfDirections;

        public bool DistributeCalm
        {
            get => _distributeCalm;
            set
            {
                _distributeCalm = value;
                OnPropertyChanged(nameof(DistributeCalm));
            }
        }

        public bool Directions
        {
            get => _8Directions;
            set
            {
                _8Directions = value;
                if (_8Directions)
                    _numberOfDirections = 8;
                else
                    _numberOfDirections = 16;
                OnPropertyChanged(nameof(Directions));
            }
        }

        public TabItemWindroseViewModel(SearchParamsStore store) : base(store)
        {
            _store = store;
            ButtonPressCommand = new RelayCommand(OpenWindrose);
        }

        void OpenWindrose()
        {
            Statistics stat = new(_store.From,_store.To,_store.StationId);
            string title = $"{_numberOfDirections}-румбовая роза ветров {(_distributeCalm ? "с распределением штилей" : "без распределения штилей")} для станции {_store.StationId} за период с {_store.From:d} до {_store.To:d}";
            DisplayWindow window = new(title, stat.GetWindRose(_distributeCalm, _numberOfDirections));
            window.Show();
        }
    }
}