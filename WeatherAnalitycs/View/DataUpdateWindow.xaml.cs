using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WeatherAnalitycs.ViewModel;
using WeatherDataParser;

namespace WeatherAnalytics.View
{
    public partial class DataUpdateWindow : Window
    {
        public DataUpdateWindow()
        {
            InitializeComponent();
            DataContext = new DataUpdateViewModel(this);
        }
        
    }
}
