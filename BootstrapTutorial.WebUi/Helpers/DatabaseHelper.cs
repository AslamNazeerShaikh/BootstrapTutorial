using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace BootstrapTutorial.WebUi.Helpers
{
    public static class DatabaseHelper
    {
        public static string? ConnectionString { get; private set; }

        public static void EnsureDatabase(string? connectionString)
        {
            ConnectionString = connectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            string filePath = ExtractFilePathFromConnectionString(connectionString);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The file 'LocalDatabase.sqlite3' does not exist on the filesystem.");

                using (var connection = new SqliteConnection(connectionString))
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
            else
            {
                Console.WriteLine("The file 'LocalDatabase.sqlite3' exists.");
            }
        }

        /// <summary>
        /// Extracts the file path from the connection string.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>The extracted file path.</returns>
        private static string ExtractFilePathFromConnectionString(string connectionString)
        {
            // Look for "Data Source=" and extract the path until the next semicolon
            const string dataSourceKeyword = "Data Source=";
            int startIndex = connectionString.IndexOf(dataSourceKeyword, StringComparison.OrdinalIgnoreCase) + dataSourceKeyword.Length;
            int endIndex = connectionString.IndexOf(';', startIndex);

            // If no semicolon exists, take the remainder of the string
            if (endIndex == -1) endIndex = connectionString.Length;

            // Get the substring containing the file path
            string relativePath = connectionString.Substring(startIndex, endIndex - startIndex);

            // Convert the relative path to an absolute path
            return Path.GetFullPath(relativePath);
        }
    }
}
