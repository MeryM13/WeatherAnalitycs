using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;

namespace WeatherAnalitycs.ViewModel
{
    class DisplayRoseWindowViewModel
    {
        public DisplayRoseWindowViewModel(string title, Dictionary<decimal, decimal> data)
        {
            Series = new ISeries[]
             {
                new PolarLineSeries<decimal>
                {
                    Values = data.Values,
                    Fill = null,
                }
             };
            WindowTitle = title;
            switch (data.Count)
            {
                case 8:
                    {
                        ChartRotation = 270;
                        AngleAxes = new PolarAxis[]
                        {
                            new PolarAxis
                            {
                                Labels = new[] { "С", "СВ", "В", "ЮВ", "Ю", "ЮЗ", "З", "СЗ" }
                            }
                        };
                        break;
                    }
                case 16:
                    {
                        ChartRotation = 270;
                        AngleAxes = new PolarAxis[]
                        {
                            new PolarAxis
                            {
                                Labels = new[] { "С", "ССВ", "СВ", "СВВ", "В", "ЮВВ", "ЮВ", "ЮЮВ", "Ю","ЮЮЗ", "ЮЗ","ЮЗЗ", "З", "СЗЗ", "СЗ", "ССЗ" }
                            }
                        };
                        break;
                    }
                default:
                    {
                        throw new Exception();
                    }
            }
        }

        public DisplayRoseWindowViewModel(string title, Dictionary<decimal, int> data)
        {
            Series = new ISeries[]
             {
                new LineSeries<int>
                {
                    Values = data.Values,
                    Fill = null

                }
             };
            WindowTitle = title;
            switch (data.Count)
            {
                case 8:
                    {
                        ChartRotation = 225;
                        AngleAxes = new PolarAxis[]
                        {
                            new PolarAxis
                            {
                                Labels = new[] { "С", "СВ", "В", "ЮВ", "Ю", "ЮЗ", "З", "СЗ" }
                            }
                        };
                        break;
                    }
                case 16:
                    {
                        ChartRotation = 270;
                        AngleAxes = new PolarAxis[]
                        {
                            new PolarAxis
                            {
                                Labels = new[] { "С", "ССВ", "СВ", "СВВ", "В", "ЮВВ", "ЮВ", "ЮЮВ", "Ю","ЮЮЗ", "ЮЗ","ЮЗЗ", "З", "СЗЗ", "СЗ", "ССЗ" }
                            }
                        };
                        break;
                    }
                default:
                    {
                        throw new Exception();
                    }
            }
        }
        public PolarAxis[] AngleAxes { get; set; }
        public string WindowTitle { get; set; }
        public ISeries[] Series { get; set; }
        public decimal ChartRotation {get; set;}
    }
}
