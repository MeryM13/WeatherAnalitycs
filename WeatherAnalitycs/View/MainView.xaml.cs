using System.Windows.Controls;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.ViewModel;

namespace WeatherAnalitycs.View
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            SettingsClass settings = new();
            InitializeComponent();
            DataContext = new MainViewModel(settings);
        }
    }
}
