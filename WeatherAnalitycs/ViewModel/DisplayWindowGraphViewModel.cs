using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataParser;
using Microsoft.VisualBasic;

namespace WeatherAnalitycs.ViewModel
{
    public class DisplayWindowGraphViewModel
    {
        public DisplayWindowGraphViewModel(string title, Dictionary<DateTime, decimal> data)
        {
            Series = new ISeries[]
      {
                new LineSeries<decimal>
                {
                    Values = data.Values,
                    Fill = null
                }
      };
            Title = title;
        }

        public string Title { get; set; }
        public ISeries[] Series { get; set; }

    }
}
