using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace BootstrapTutorial.WebUi.Helpers
{
    public static class DatabaseHelper
    {
        private const string DatabaseFileName = "AppData.db";

        public static string ConnectionString => $"Data Source={DatabaseFileName};";

        public static void EnsureDatabase()
        {
            if (!File.Exists(DatabaseFileName))
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Products (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Price REAL NOT NULL,
                            Description TEXT
                        );
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
