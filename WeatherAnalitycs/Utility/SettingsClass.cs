using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace WeatherAnalitycs.Utility
{
    internal class SettingsClass
    {
        string _settingsFilePath = "../../../settings.json";

        public DateTime StartingDate { get; set; } = DateTime.Parse("01.01.2015");
        public string WindRoseDataType { get; set; } = "Проценты";
        public bool DifferentiateRoseByDefault { get; set; } = true;
        public string DatabaseServer { get; set; } = "SQLite";
        public bool UseDefaultConnectionString { get; set; } = true;
        public string DatabaseConnectionString { get; set; } = "";
        public string ExcelSavePath { get; set; } = "";
        public bool DivideExcelTable { get; set; } = true;
        public DateInterval ExcelTableDivider { get; set; } = DateInterval.Year;

        public async Task SaveToFile()
        {
            using (FileStream fs = new(_settingsFilePath, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync(fs, this);
            }
        }

        public async Task LoadFromFile()
        {
            if (!File.Exists(_settingsFilePath))
                await SaveToFile();

            using (FileStream fs = new(_settingsFilePath, FileMode.Open))
            {
                try
                {
                    SettingsClass settings = await JsonSerializer.DeserializeAsync<SettingsClass>(fs);
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
