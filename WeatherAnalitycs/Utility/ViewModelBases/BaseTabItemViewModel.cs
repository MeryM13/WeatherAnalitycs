using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalitycs.Utility.ViewModelBases
{
    internal class BaseTabItemViewModel : ViewModelBase
    {
        public SearchParamsStore SearchParamsStore { get; set; }
        public RelayCommand ButtonPressCommand { get; set; }
        public RelayCommand ConvertToExcelCommand { get; set; }

        public BaseTabItemViewModel(SearchParamsStore searchParamsStore, SettingsClass settings) : base(settings)
        {
            SearchParamsStore = searchParamsStore;
        }
    }
}
