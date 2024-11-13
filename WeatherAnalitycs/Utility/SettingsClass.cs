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
        readonly JsonSerializerOptions options = new() { WriteIndented = true };

        public bool DarkMode { get; set; }
        public bool RewriteOnUpdate { get; set; }
        public DateTime StartingDate { get; set; } 
        public string WindRoseDataType { get; set; } 
        public bool DifferentiateRoseByDefault { get; set; } 
        public List<int> DifferentiateRoseSpeeds { get; set; }
        public int DatabaseServer { get; set; } 
        public bool UseDefaultConnectionString { get; set; }
        public string SQLiteDefaultConnectionString { get; set; }
        public string SQLiteConnectionString { get; set; }
        public string MSSQLDefaultConnectionString { get; set; }
        public string MSSQLConnectionString { get; set; }
        
        public string DatabaseConnectionString
        {
            get
            {
                return DatabaseServer switch
                {
                    1 => SQLiteConnectionString,
                    2 => MSSQLConnectionString,
                    _ => SQLiteConnectionString,
                };
            }
            set
            {
                switch (DatabaseServer)
                {
                    case 1:
                        {
                            SQLiteConnectionString = value; break;
                        }
                    case 2:
                        {
                            MSSQLConnectionString = value; break;
                        }
                    default:
                        {
                            SQLiteConnectionString = value; break;
                        }
                }
            }
        }

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
            DatabaseServer = 1;
            UseDefaultConnectionString = true;
            SQLiteDefaultConnectionString = @"Data Source=WeatherDatabase; Foreign Keys=True";
            SQLiteConnectionString = SQLiteDefaultConnectionString;
            MSSQLDefaultConnectionString = @"Data Source=localhost;Initial Catalog=WeatherDatabase;Integrated Security=True;TrustServerCertificate=True";
            MSSQLConnectionString = MSSQLDefaultConnectionString;
            ExcelSavePath = @"C:\WeatherDataSheets";
            DivideExcelTable = true;
            ExcelTableDivider = DateInterval.Year;

            using FileStream fs = new(_settingsFilePath, FileMode.OpenOrCreate);
            JsonSerializer.Serialize(fs, this, options);
        }

        public void SaveToFile()
        {
            using FileStream fs = new(_settingsFilePath, FileMode.Truncate);
            JsonSerializer.Serialize(fs, this, options);
        }

        public void LoadFromFile()
        {
            if (!File.Exists(_settingsFilePath))
                CreateFile();

            using FileStream fs = new(_settingsFilePath, FileMode.Open);
            try
            {
                SettingsClass settings = JsonSerializer.Deserialize<SettingsClass>(fs);

                DarkMode = settings.DarkMode;
                RewriteOnUpdate = settings.RewriteOnUpdate;
                StartingDate = settings.StartingDate;
                WindRoseDataType = settings.WindRoseDataType;
                DifferentiateRoseByDefault = settings.DifferentiateRoseByDefault;
                DifferentiateRoseSpeeds = settings.DifferentiateRoseSpeeds;
                DatabaseServer = settings.DatabaseServer;
                UseDefaultConnectionString = settings.UseDefaultConnectionString;
                SQLiteDefaultConnectionString = settings.SQLiteDefaultConnectionString;
                SQLiteConnectionString = settings.SQLiteConnectionString;
                MSSQLDefaultConnectionString = settings.MSSQLDefaultConnectionString;
                MSSQLConnectionString = settings.MSSQLConnectionString;
                DatabaseConnectionString = settings.DatabaseConnectionString;
                ExcelSavePath = settings.ExcelSavePath;
                DivideExcelTable = settings.DivideExcelTable;
                ExcelTableDivider = settings.ExcelTableDivider;
            }
            catch (Exception ex)
            {
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
