using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalitycs.Utility.ViewModelBases
{
    internal class BaseTabItemViewModel(SearchParamsStore searchParamsStore, SettingsClass settings) : ViewModelBase(settings)
    {
        public SearchParamsStore Store { get; set; } = searchParamsStore;
        public RelayCommand ButtonPressCommand { get; set; }
        public RelayCommand ConvertToExcelCommand { get; set; }
    }
}
