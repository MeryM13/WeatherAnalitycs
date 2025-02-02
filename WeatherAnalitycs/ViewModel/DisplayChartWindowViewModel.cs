﻿using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherAnalitycs.ViewModel
{
    public class DisplayChartWindowViewModel
    {
        public DisplayChartWindowViewModel(string title, int entries, Dictionary<DateTime, decimal> data)
        {
            List<DateTime> list = new(data.Keys);
            var stringlabels = list.Select(x => x.ToString("d"));
            Series =
             [
                new LineSeries<decimal>
                {
                    Values = data.Values,
                    Fill = null

                }
             ];
            WindowTitle = title;
            EntriesUsed = $"В расчетах использовано {entries} записей";
            XAxes =
            [
              new() {
                // Use the labels property to define named labels.
                Labels = stringlabels.ToArray(),
                }
            ];
        }

        public string WindowTitle { get; set; }
        public string EntriesUsed { get; set; }
        public ISeries[] Series { get; set; }
        public List<Axis> XAxes { get; set; }
    }
}
