using System.Reflection.Emit;
using System.Windows.Controls;
using System.Windows.Shapes;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.ViewModel;
using static OfficeOpenXml.ExcelErrorValue;

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
            settings.LoadFromFile();
            InitializeComponent();
            DataContext = new MainViewModel(settings);
        }
    }
}