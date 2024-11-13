using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using WeatherAnalitycs.Utility.ViewModelBases;
using WeatherAnalitycs.Utility;

namespace WeatherAnalitycs.ViewModel
{
    class DisplayRoseWindowViewModel(string title, int entries, SettingsClass settings) : ViewModelBase(settings)
    {
        public string WindowTitle { get; } = title;
        public string EntriesUsed { get; } = $"В расчетах использовано {entries} записей";
    }
}
