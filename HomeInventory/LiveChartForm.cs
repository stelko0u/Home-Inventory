using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using TSPProject;

namespace HomeInventory
{
    public partial class LiveChartForm : Form
    {
        private string connectionString = "Data Source=../../../storage.db;Version=3;";

        public LiveChartForm()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            InitializeUI();
            InitializeCharts();

        }

        private void InitializeUI()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            button1.Text = "Quantity Chart";
            button2.Text = "Price Chart";
            button3.Text = "Category Chart";
        }

        private void InitializeCharts()
        {
            cartesianChart1.Visible = false;
            cartesianChart2.Visible = false;
            cartesianChart3.Visible = false;

            cartesianChart1.Series = new SeriesCollection();
            cartesianChart2.Series = new SeriesCollection();
            cartesianChart3.Series = new SeriesCollection();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadQuantityChart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadPriceChart();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadCategorySpendingChart();
        }

        private void LoadQuantityChart()
        {
            List<(string Name, int Quantity, decimal Price)> productData = GetProductData();
            if (productData.Count == 0)
            {
                MessageBox.Show("No data available for the chart.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] productNames = productData.Select(p => p.Name).ToArray();
            double[] quantities = productData.Select(p => (double)p.Quantity).ToArray();

            cartesianChart1.Series.Clear();
            cartesianChart1.Series.Add(new ColumnSeries
            {
                Title = "Quantity",
                Values = new ChartValues<double>(quantities),
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243))
            });

            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Products",
                Labels = productNames
            });

            cartesianChart1.AxisY.Clear();
            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Quantity",
                LabelFormatter = value => value.ToString("N0")
            });

            cartesianChart1.Visible = true;
            cartesianChart2.Visible = false;
            cartesianChart3.Visible = false;
        }

        private void LoadPriceChart()
        {
            List<(string Name, int Quantity, decimal Price)> productData = GetProductData();
            if (productData.Count == 0)
            {
                MessageBox.Show("No data available for the chart.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] productNames = productData.Select(p => p.Name).ToArray();
            double[] prices = productData.Select(p => (double)p.Price).ToArray();

            cartesianChart2.Series.Clear();
            cartesianChart2.Series.Add(new ColumnSeries
            {
                Title = "Price",
                Values = new ChartValues<double>(prices),
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 67, 54))
            });

            cartesianChart2.AxisX.Clear();
            cartesianChart2.AxisX.Add(new Axis
            {
                Title = "Products",
                Labels = productNames
            });

            cartesianChart2.AxisY.Clear();
            cartesianChart2.AxisY.Add(new Axis
            {
                Title = "Price",
                LabelFormatter = value => value.ToString("C2")
            });

            cartesianChart1.Visible = false;
            cartesianChart2.Visible = true;
            cartesianChart3.Visible = false;
        }

        private void LoadCategorySpendingChart()
        {
            Dictionary<string, double> categorySpending = GetCategorySpending();

            if (categorySpending.Count == 0)
            {
                MessageBox.Show("No data available for the chart.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cartesianChart3.Series.Clear();
            cartesianChart3.AxisX.Clear();
            cartesianChart3.AxisY.Clear();

            cartesianChart3.Series.Add(new ColumnSeries
            {
                Title = "Money Spent",
                Values = new ChartValues<double>(categorySpending.Values),
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80))
            });

            cartesianChart3.AxisX.Add(new Axis
            {
                Title = "Categories",
                Labels = categorySpending.Keys.ToList(),
                Separator = new Separator { Step = 1, IsEnabled = false }
            });

            cartesianChart3.AxisY.Add(new Axis
            {
                Title = "Total Spent ($)",
                LabelFormatter = value => value.ToString("C2")
            });

            cartesianChart1.Visible = false;
            cartesianChart2.Visible = false;
            cartesianChart3.Visible = true;
        }


        private List<(string Name, int Quantity, decimal Price)> GetProductData()
        {
            List<(string, int, decimal)> products = new List<(string, int, decimal)>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Name, Quantity, Price FROM Product;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add((reader.GetString(0), reader.GetInt32(1), reader.GetDecimal(2)));
                    }
                }
            }
            return products;
        }

        private Dictionary<string, double> GetCategorySpending()
        {
            Dictionary<string, double> categorySpending = new Dictionary<string, double>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT c.Name, SUM(p.Price * p.Quantity) AS TotalSpent
                    FROM Product p
                    JOIN Category c ON p.CategoryId = c.Id
                    GROUP BY c.Name;
                ";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string category = reader.GetString(0);
                        double totalSpent = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                        categorySpending[category] = totalSpent;
                    }
                }
            }

            return categorySpending;
        }

        private void LiveChartForm_Load(object sender, EventArgs e)
        {

        }

    }
}