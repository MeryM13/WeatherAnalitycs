using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAnalitycs.Utility;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemCalmcountViewModel : BaseTabItemViewModel
    {
        public TabItemCalmcountViewModel(SearchParamsStore store) : base(store)
        {
            ButtonPressCommand = new RelayCommand(DoCalculations);
        }

        void DoCalculations()
        {

        }
    }
}
