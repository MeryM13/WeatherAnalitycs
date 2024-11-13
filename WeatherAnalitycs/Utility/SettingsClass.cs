using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using MvvmHelpers;

namespace WeatherAnalitycs.Utility
{
    public class SettingsClass
    {
        readonly string _settingsFilePath = "../../../settings.json";

        public bool DarkMode { get; set; }
        public bool RewriteOnUpdate { get; set; }
        public DateTime StartingDate { get; set; } 
        public string WindRoseDataType { get; set; } 
        public bool DifferentiateRoseByDefault { get; set; } 
        public List<int> DifferentiateRoseSpeeds { get; set; }
        public string DatabaseServer { get; set; } 
        public bool UseDefaultConnectionString { get; set; } 
        public string DatabaseConnectionString { get; set; } 
        public string ExcelSavePath { get; set; } 
        public bool DivideExcelTable { get; set; } 
        public DateInterval ExcelTableDivider { get; set; } 

        public void CreateFile()
        {
            DarkMode = false;
            RewriteOnUpdate = false;
            StartingDate = DateTime.Parse("01.01.2015");
            WindRoseDataType = "Percent";
            DifferentiateRoseByDefault = true;
            DifferentiateRoseSpeeds = [20, 15, 10, 8, 6, 4, 2];
            DatabaseServer = "SQLite";
            UseDefaultConnectionString = true;
            DatabaseConnectionString = "";
            ExcelSavePath = @"C:\WeatherDataSheets";
            DivideExcelTable = true;
            ExcelTableDivider = DateInterval.Year;
            using (FileStream fs = new(_settingsFilePath, FileMode.OpenOrCreate))
            {
                JsonSerializer.SerializeAsync(fs, this);
            }
        }

        public void SaveToFile()
        {
            using (FileStream fs = new(_settingsFilePath, FileMode.Truncate))
            {
                JsonSerializer.SerializeAsync(fs, this);
            }
        }

        public void LoadFromFile()
        {
            if (!File.Exists(_settingsFilePath))
                CreateFile();

            using (FileStream fs = new(_settingsFilePath, FileMode.Open))
            {
                try
                {
                    SettingsClass settings = JsonSerializer.DeserializeAsync<SettingsClass>(fs).Result;
                    DarkMode = settings.DarkMode;
                    RewriteOnUpdate = settings.RewriteOnUpdate;
                    StartingDate = settings.StartingDate;
                    WindRoseDataType = settings.WindRoseDataType;
                    DifferentiateRoseByDefault = settings.DifferentiateRoseByDefault;
                    DatabaseServer = settings.DatabaseServer;
                    DatabaseConnectionString = settings.DatabaseConnectionString;
                    ExcelSavePath = settings.ExcelSavePath;
                    DivideExcelTable = settings.DivideExcelTable;
                    ExcelTableDivider = settings.ExcelTableDivider;
                }
                catch (Exception ex)
                {
                }
            }
        }

        public SettingsClass()
        {
        }

        public SettingsClass(string path)
        {
            _settingsFilePath = path;
        }
    }
}
