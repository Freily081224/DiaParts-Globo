using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace DiaParts_Globo.Database
{
    internal class SQLiteDbContext
    {
        private static readonly string _dbPath = "DiaParts.db";
        private static readonly string _connectionString = $"Data Source={_dbPath};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string createPartesTable = @"
                CREATE TABLE IF NOT EXISTS Partes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    NumeroParte TEXT NOT NULL,
                    Modelo TEXT NOT NULL,
                    ImagenPath TEXT
                );
            ";

                string createModelosTable = @"
                CREATE TABLE IF NOT EXISTS Modelos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL
                );
            ";

                using (var command = new SQLiteCommand(createPartesTable, connection))
                    command.ExecuteNonQuery();

                using (var command = new SQLiteCommand(createModelosTable, connection))
                    command.ExecuteNonQuery();
            }
        }

        public static string GetConnectionString()
        {
            return _connectionString;
        }
    }
}

