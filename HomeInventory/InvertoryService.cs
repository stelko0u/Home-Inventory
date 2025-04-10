using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace HomeInventory
{
    internal class InvertoryService : IInventoryService
    {
        private string connectionString = "Data Source=../../../storage.db;Version=3;";

        public List<(string Name, int Quantity, decimal Price)> GetProductData()
        {
            var products = new List<(string, int, decimal)>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Name, Quantity, Price FROM Product;";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add((reader.GetString(0), reader.GetInt32(1), reader.GetDecimal(2)));
                    }
                }
            }

            return products;
        }

        public Dictionary<string, double> GetCategorySpending()
        {
            var spending = new Dictionary<string, double>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT c.Name, SUM(p.Price * p.Quantity) AS TotalSpent
                    FROM Product p
                    JOIN Category c ON p.CategoryId = c.Id
                    GROUP BY c.Name;";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string category = reader.GetString(0);
                        double total = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                        spending[category] = total;
                    }
                }
            }

            return spending;
        }
    }
}
