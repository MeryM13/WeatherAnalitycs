using System;
using System.Threading.Tasks;
using System.Windows;
using WeatherAnalitycs.ViewModel;
using WeatherDataParser;
using WeatherClasses = WeatherDataParser.CLASSES;

namespace WeatherAnalytics.View
{
    public partial class AddStationWindow : Window
    {
        public AddStationWindow()
        {
            InitializeComponent();
            DataContext = new AddStationViewModel(this);
        }
    }
}
