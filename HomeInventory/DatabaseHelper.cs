using HomeInventory.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace TSPProject
{
    public class DatabaseHelper
    {
        private static string dbPath = @"../../../storage.db";
        private static string connectionString = $"Data Source={dbPath};Version=3;";



        public DatabaseHelper()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                CreateTables();
            }
        }



        public void CreateTables()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string categoryTableQuery = @"
            CREATE TABLE IF NOT EXISTS Category (
                Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                Name TEXT UNIQUE NOT NULL
            );";

                string productTableQuery = @"
            CREATE TABLE IF NOT EXISTS Product (
                Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                Name TEXT NOT NULL, 
                CategoryId INTEGER NOT NULL, 
                Price REAL NOT NULL CHECK (Price >= 0), 
                Quantity INTEGER NOT NULL CHECK (Quantity >= 0),
                Date TEXT NOT NULL,  -- Ensure the Date column is added
                FOREIGN KEY (CategoryId) REFERENCES Category(Id) ON DELETE CASCADE
            );";

                using (var command = new SQLiteCommand(categoryTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(productTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }



        public void AddCategory(string categoryName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Category WHERE Name = @Name;";
                using (var checkCommand = new SQLiteCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", categoryName);

                    long count = (long)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        throw new InvalidOperationException("Category with the same name already exists.");

                    }
                }

                string insertQuery = "INSERT INTO Category (Name) VALUES (@Name);";
                using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Name", categoryName);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
        public void AddProduct(string name, int categoryId, decimal price, int quantity, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");

            if (price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Product (Name, CategoryId, Price, Quantity, Date) VALUES (@Name, @CategoryId, @Price, @Quantity, @Date);";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("No rows were affected in the database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                throw;
            }
        }


        public void UpdateProduct(int productId, string name, int categoryId, decimal price, int quantity, DateTime date)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
        UPDATE Product 
        SET Name = @Name, CategoryId = @CategoryId, Price = @Price, Quantity = @Quantity, Date = @Date
        WHERE Id = @ProductId;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteProduct(int productId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Product WHERE Id = @ProductId;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.ExecuteNonQuery();
                }
            }
        }


        public List<Categories> GetCategories()
        {
            List<Categories> categories = new List<Categories>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name FROM Category;";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Categories
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return categories;
        }

        public List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> GetProducts()
        {
            List<(int, string, string, decimal, int, string)> products = new List<(int, string, string, decimal, int, string)>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
        SELECT Product.Id, Product.Name, Category.Name AS Category, Product.Price, Product.Quantity, Product.Date
        FROM Product 
        JOIN Category ON Product.CategoryId = Category.Id;";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add((reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDecimal(3), reader.GetInt32(4), reader.GetString(5)));
                    }
                }
            }
            return products;
        }


        public List<(int Id, string Name, decimal Price, int Quantity, string Date)> GetProductsByCategory(int categoryId)
        {
            List<(int, string, decimal, int, string)> products = new List<(int, string, decimal, int, string)>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Price, Quantity, Date FROM Product WHERE CategoryId = @CategoryId;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add((reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetInt32(3), reader.GetString(4)));
                        }
                    }
                }
            }
            return products;
        }

        public int GetCategoryId(string categoryName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id FROM Category WHERE Name = @CategoryName";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", categoryName);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }


        public List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> GetProductsSortedByPrice(bool ascending)
        {
            string order = ascending ? "ASC" : "DESC";
            string query = $@"
        SELECT Product.Id, Product.Name, Category.Name AS Category, Product.Price, Product.Quantity, Product.Date
        FROM Product 
        JOIN Category ON Product.CategoryId = Category.Id
        ORDER BY Product.Price {order};";
            return GetSortedProducts(query);
        }

        public List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> GetProductsSortedByQuantity(bool ascending)
        {
            string order = ascending ? "ASC" : "DESC";
            string query = $@"
        SELECT Product.Id, Product.Name, Category.Name AS Category, Product.Price, Product.Quantity, Product.Date
        FROM Product 
        JOIN Category ON Product.CategoryId = Category.Id
        ORDER BY Product.Quantity {order};";
            return GetSortedProducts(query);
        }

        public List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> GetProductsSortedByDate(bool newestFirst)
        {
            string order = newestFirst ? "DESC" : "ASC";
            string query = $@"
        SELECT Product.Id, Product.Name, Category.Name AS Category, Product.Price, Product.Quantity, Product.Date
        FROM Product 
        JOIN Category ON Product.CategoryId = Category.Id
        ORDER BY Product.Date {order};";
            return GetSortedProducts(query);
        }

        private List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> GetSortedProducts(string query)
        {
            List<(int, string, string, decimal, int, string)> products = new List<(int, string, string, decimal, int, string)>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add((reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDecimal(3), reader.GetInt32(4), reader.GetString(5)));
                    }
                }
            }
            return products;
        }

        public List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> GetProductsByCategoryId(int categoryId)
        {
            List<(int, string, string, decimal, int, string)> products = new List<(int, string, string, decimal, int, string)>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT p.Id, p.Name, c.Name AS Category, p.Price, p.Quantity, p.Date 
            FROM Product p
            JOIN Category c ON p.CategoryId = c.Id
            WHERE p.CategoryId = @categoryId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryId", categoryId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add((
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDecimal(3),
                                reader.GetInt32(4),
                                reader.GetString(5)
                            ));
                        }
                    }
                }
            }
            return products;
        }


        public int GetCategoryIdByName(string categoryName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id FROM Category WHERE Name = @categoryName";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryName", categoryName);

                    object result = command.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
    }
}