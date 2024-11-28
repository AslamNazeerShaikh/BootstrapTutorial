using BootstrapTutorial.WebUi.Helpers;
using BootstrapTutorial.WebUi.Models;
using Microsoft.Data.Sqlite;

namespace BootstrapTutorial.WebUi.Repositories
{
    public class ProductRepository : IGenericRepository<Product>
    {
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Price, Description FROM Products;";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }
                }
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            Product? product = null;

            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Price, Description FROM Products WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                    }
                }
            }

            return product ?? new Product();
        }

        public async Task<int> AddAsync(Product entity)
        {
            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Products (Name, Price, Description)
                    VALUES (@Name, @Price, @Description);
                ";
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Price", entity.Price);
                command.Parameters.AddWithValue("@Description", entity.Description ?? (object)DBNull.Value);

                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> UpdateAsync(Product entity)
        {
            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Products
                    SET Name = @Name, Price = @Price, Description = @Description
                    WHERE Id = @Id;
                ";
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Price", entity.Price);
                command.Parameters.AddWithValue("@Description", entity.Description ?? (object)DBNull.Value);

                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Products WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", id);

                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
