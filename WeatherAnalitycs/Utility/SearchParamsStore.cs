using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataParser.CLASSES;

namespace WeatherAnalitycs.Utility
{
    internal class SearchParamsStore:BaseViewModel
    {
        public int StationId { get; set; } = 0;
        public DateTime From { get; set; } = new(2012, 01, 01);
        public DateTime To { get; set; } = DateTime.Now;
    }
}
