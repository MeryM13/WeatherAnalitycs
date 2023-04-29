using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using WeatherDataParser;
using System.Data;
using WeatherAnalitycs.ViewModel;

namespace WeatherAnalytics.View
{
    /// <summary>
    /// Логика взаимодействия для DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        public DisplayWindow()
        {
            InitializeComponent();
        }

        public DisplayWindow(string title, DataTable table)
        {
            DataGrid grid = new DataGrid();
            viewer.Content = grid;
        }

        public DisplayWindow(string title, Dictionary<DateTime, decimal> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayWindowGraphViewModel(title,data);
        }
    }
}
