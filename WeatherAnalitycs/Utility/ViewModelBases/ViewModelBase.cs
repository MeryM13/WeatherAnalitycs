using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalitycs.Utility.ViewModelBases
{
    public class ViewModelBase : BaseViewModel
    {
        public SettingsClass Settings { get; set; }
        public bool DarkMode { get; set; }
        public ViewModelBase(SettingsClass settings)
        {
            Settings = settings;
            DarkMode = Settings.DarkMode;
        }
    }
}
