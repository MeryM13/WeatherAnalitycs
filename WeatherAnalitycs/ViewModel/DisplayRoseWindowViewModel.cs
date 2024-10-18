using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;

namespace WeatherAnalitycs.ViewModel
{
    class DisplayRoseWindowViewModel
    {
        public DisplayRoseWindowViewModel(string title, int entries)
        {
            WindowTitle = title;
            EntriesUsed = $"В расчетах использовано {entries} записей";
        }
        public string WindowTitle { get; }
        public string EntriesUsed { get; }
    }
}
