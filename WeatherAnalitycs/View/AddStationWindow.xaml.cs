﻿using System;
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
using WeatherDataParser;
using WeatherClasses = WeatherDataParser.CLASSES;

namespace WeatherAnalytics.View
{
    /// <summary>
    /// Логика взаимодействия для AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        WeatherClasses.Station _station;
        Parser _parser;
        public AddStationWindow()
        {
            InitializeComponent();
            _parser = new Parser();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _station = _parser.FindStation(int.Parse(txtStationID.Text));
                txtInfo.Text = $"Название станции: {_station.Name}; Расположение: {_station.Location}; Географические координаты: широта: {_station.Latitude}, долгота: {_station.Longitude}; Высота над уровнем моря: {_station.Height} м.";
                btnAdd.IsEnabled = true;
            }
            catch (Exception ex)
            {
                btnAdd.IsEnabled = false;
                MessageBox.Show(ex.Message);
                txtInfo.Text = "Название станции:  Расположение:  Географические координаты: широта:  долгота:  Высота над уровнем моря:";
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TO DO: Progress bar
                _parser.AddStation(_station);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}