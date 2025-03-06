using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace HomeInventory
{
    public partial class LiveChartForm : Form
    {
        private string connectionString = "Data Source=../../../storage.db;Version=3;";

        public LiveChartForm()
        {
            InitializeComponent();
            InitializeUI();
            InitializeCharts();
        }

        private void InitializeUI()
        {
            button1.Text = "Quantity Chart";
            button2.Text = "Price Chart";
        }

        private void InitializeCharts()
        {
            cartesianChart1.Visible = false;
            cartesianChart2.Visible = false;
            cartesianChart1.Series = new SeriesCollection();
            cartesianChart2.Series = new SeriesCollection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadQuantityChart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadPriceChart();
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
    }
}