using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherAnalitycs.ViewModel
{
    public class DisplayChartWindowViewModel
    {
        public DisplayChartWindowViewModel(string title, Dictionary<DateTime, decimal> data)
        {
            List<DateTime> list = new(data.Keys);
            var stringlabels = list.Select(x => x.ToString("d"));
            Series = new ISeries[]
             {
                new LineSeries<decimal>
                {
                    Values = data.Values,
                    Fill = null

                }
             };
            WindowTitle = title;
            XAxes = new List<Axis>
            {
              new Axis
                {
                // Use the labels property to define named labels.
                Labels = stringlabels.ToArray(),
                }
            };
        }

        public string WindowTitle { get; set; }
        public ISeries[] Series { get; set; }
        public List<Axis> XAxes { get; set; }
    }
}
