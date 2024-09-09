using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalitycs.Utility
{
    internal class BaseTabItemViewModel:BaseViewModel
    {
        public SearchParamsStore SearchParamsStore { get; set; }
        public RelayCommand ButtonPressCommand { get; set; }

        public BaseTabItemViewModel(SearchParamsStore searchParamsStore) 
        { 
            this.SearchParamsStore = searchParamsStore;
        }
    }
}
