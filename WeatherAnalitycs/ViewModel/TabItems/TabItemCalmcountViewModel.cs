using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAnalitycs.Utility;
using WeatherDataParser;

namespace WeatherAnalitycs.ViewModel.TabItems
{
    internal class TabItemCalmcountViewModel : BaseTabItemViewModel
    {
        SearchParamsStore _store;
        int _calmCount, _lowCount;
        decimal _calmPeriodicity, _lowPeriodicity;
        bool _getCalmCount, _getCalmPeriodicity, _getLowCount, _getLowPeriodicity;

        public int CalmCount
        {
            get => _calmCount;
            set
            {
                _calmCount = value;
                OnPropertyChanged(nameof(CalmCount));
            }
        }
        public int LowCount
        {
            get => _lowCount;
            set
            {
                _lowCount = value;
                OnPropertyChanged(nameof(LowCount));
            }
        }
        public decimal CalmPeriodicity
        {
            get => _calmPeriodicity;
            set
            {
                _calmPeriodicity = value;
                OnPropertyChanged(nameof(CalmPeriodicity));
            }
        }
        public decimal LowPeriodicity
        {
            get => _lowPeriodicity;
            set
            {
                _lowPeriodicity = value;
                OnPropertyChanged(nameof(LowPeriodicity));
            }
        }

        public bool GetCalmCount
        {
            get => _getCalmCount;
            set
            {
                _getCalmCount = value;
                OnPropertyChanged(nameof(GetCalmCount));
            }
        }

        public bool GetCalmPeriodicity
        {
            get => _getCalmPeriodicity;
            set
            {
                _getCalmPeriodicity = value;
                OnPropertyChanged(nameof(GetCalmPeriodicity));
            }
        }

        public bool GetLowCount
        {
            get => _getLowCount;
            set
            {
                _getLowCount = value;
                OnPropertyChanged(nameof(GetLowCount));
            }
        }

        public bool GetLowPeriodicity
        {
            get => _getLowPeriodicity;
            set
            {
                _getLowPeriodicity = value;
                OnPropertyChanged(nameof(GetLowPeriodicity));
            }
        }

        public TabItemCalmcountViewModel(SearchParamsStore store) : base(store)
        {
            _store = store;
            ButtonPressCommand = new RelayCommand(DoCalculations);
        }

        void DoCalculations()
        {
            Statistics stat = new Statistics(_store.From, _store.To, _store.StationId);
            CalmCount = 0;
            CalmPeriodicity = 0;
            LowCount = 0;
            LowPeriodicity = 0;

            if (GetCalmCount)
                CalmCount = stat.GetCalmCount();
            if (GetLowCount)
                LowCount = stat.GetWeakCount();
            if (GetCalmPeriodicity)
                CalmPeriodicity = stat.GetCalmPeriodicity();
            if (GetLowPeriodicity)
                LowPeriodicity = stat.GetWeakPeriodicity();

        }
    }
}
