using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WeatherAnalitycs.Utility;
using WeatherAnalitycs.ViewModel;

namespace WeatherAnalitycs.View
{
    /// <summary>
    /// Логика взаимодействия для DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        public DisplayWindow(string title, int entries, DataTable table)
        {
            InitializeComponent();
            this.DataContext = new DisplayTableWindowViewModel(table, entries, title);

            Binding binding = new()
            {
                Source = this.DataContext,
                Path = new PropertyPath("Table")
            };

            DataGrid grid = new()
            {
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                CanUserReorderColumns = false,
                ClipToBounds = true,
            };
            BindingOperations.SetBinding(grid, DataGrid.ItemsSourceProperty, binding);

            mainGrid.Children.Add(grid);
        }

        public DisplayWindow(string title, int entries, Dictionary<DateTime, decimal> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayChartWindowViewModel(title, entries, data);

            Binding seriesBinding = new()
            {
                Source = this.DataContext,
                Path = new PropertyPath("Series")
            };

            Binding axesBinding = new()
            {
                Source = this.DataContext,
                Path = new PropertyPath("XAxes")
            };

            CartesianChart chart = new()
            {
                ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.Both,
                Margin = new Thickness(0, 125, 0, 0)
            };
            BindingOperations.SetBinding(chart, CartesianChart.SeriesProperty, seriesBinding);
            BindingOperations.SetBinding(chart, CartesianChart.XAxesProperty, axesBinding);
            ScrollViewer.SetHorizontalScrollBarVisibility(chart, ScrollBarVisibility.Auto);

            mainGrid.Children.Add(chart);
        }

        public DisplayWindow(string title, int entries, Dictionary<decimal, decimal> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayRoseWindowViewModel(title, entries);
            mainGrid.Children.Add(new Rose(new Dictionary<int, Dictionary<decimal, decimal>>() { { 0, data } }));
        }

        public DisplayWindow(string title, int entries, Dictionary<int, Dictionary<decimal, decimal>> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayRoseWindowViewModel(title, entries);
            mainGrid.Children.Add(new Rose(data));
        }
    }
}
