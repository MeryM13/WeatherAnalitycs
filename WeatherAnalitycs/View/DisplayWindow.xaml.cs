using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WeatherAnalitycs.ViewModel;

namespace WeatherAnalitycs.View
{
    /// <summary>
    /// Логика взаимодействия для DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        public DisplayWindow(string title, DataTable table)
        {
            InitializeComponent();
            this.DataContext = new DisplayTableWindowViewModel(table, title);

            Binding binding = new()
            {
                Source = this.DataContext,
                Path = new PropertyPath("Table")
            };

            DataGrid grid = new()
            {
                Margin = new Thickness(0, 125, 0, 0),
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                CanUserReorderColumns = false,
                ClipToBounds = true
            };
            BindingOperations.SetBinding(grid, DataGrid.ItemsSourceProperty, binding);

            mainGrid.Children.Add(grid);
        }

        public DisplayWindow(string title, Dictionary<DateTime, decimal> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayChartWindowViewModel(title, data);

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

        public DisplayWindow(string title, Dictionary<decimal, decimal> data)
        {
            InitializeComponent();
            this.DataContext = new DisplayRoseWindowViewModel(title, data);

            Binding seriesBinding = new()
            {
                Source = this.DataContext,
                Path = new PropertyPath("Series")
            };

            Binding axesBinding = new()
            {
                Source = this.DataContext,
                Path = new PropertyPath("AngleAxes")
            };

            Binding rotationBinding = new()
            {
                Source = this.DataContext,
                Path = new PropertyPath("ChartRotation")
            };

            PolarChart chart = new()
            {
                Margin = new Thickness(0, 125, 0, 0)
            };
            BindingOperations.SetBinding(chart, PolarChart.SeriesProperty, seriesBinding);
            BindingOperations.SetBinding(chart, PolarChart.AngleAxesProperty, axesBinding);
            BindingOperations.SetBinding(chart, PolarChart.InitialRotationProperty, rotationBinding);

            mainGrid.Children.Add(chart);
        }
    }
}
