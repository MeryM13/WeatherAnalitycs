using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;

namespace WeatherAnalitycs.ViewModel
{
    class DisplayRoseWindowViewModel
    {
        public DisplayRoseWindowViewModel(string title, int entries, Dictionary<decimal, decimal> data)
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
            EntriesUsed = $"В расчетах использовано {entries} записей";
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

        public DisplayRoseWindowViewModel(string title, int entries, Dictionary<decimal, int> data)
        {
            Series = new ISeries[]
             {
                new PolarLineSeries<int>
                {
                    Values = data.Values,
                    Fill = null

                }
             };
            WindowTitle = title;
            EntriesUsed = $"В расчетах использовано {entries} записей";
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
        public string EntriesUsed { get; set; }
        public ISeries[] Series { get; set; }
        public decimal ChartRotation {get; set;}
    }
}
