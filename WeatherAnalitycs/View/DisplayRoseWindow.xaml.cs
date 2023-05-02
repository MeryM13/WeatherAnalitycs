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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherAnalitycs.ViewModel;

namespace WeatherAnalitycs.View
{
    /// <summary>
    /// Логика взаимодействия для DisplayRoseWindow.xaml
    /// </summary>
    public partial class DisplayRoseWindow : Window
    {
        public DisplayRoseWindow(string title, Dictionary<decimal,decimal> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayRoseWindowViewModel(title, data);
            this.Title = title;
        }

        public DisplayRoseWindow(string title, Dictionary<decimal, int> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayRoseWindowViewModel(title, data);
            this.Title = title;
        }
    }
}
