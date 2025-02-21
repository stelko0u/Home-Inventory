using HomeInventory.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TSPProject;

namespace HomeInventory
{
    public partial class AddProductForm : Form
    {
        private readonly DatabaseHelper databaseHelper;
        private readonly ListView mainFormListView;
        public AddProductForm(ListView listView)
        {
            mainFormListView = listView;
            databaseHelper = new DatabaseHelper();
            InitializeComponent();
            LoadCategories();
        }


        private void LoadCategories()
        {
            try
            {
                List<Categories> categories = databaseHelper.GetCategories();
                
                CategoryBox.DataSource = categories;
                CategoryBox.DisplayMember = "Name";
                CategoryBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading categories: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void AddToDatabase(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Please enter a name.");
                return;
            }

            if (CategoryBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            if (string.IsNullOrWhiteSpace(QuantityBox.Text))
            {
                MessageBox.Show("Please enter a quantity.");
                return;
            }

            if (string.IsNullOrWhiteSpace(PriceBox.Text))
            {
                MessageBox.Show("Please enter a price.");
                return;
            }

            decimal price;
            int quantity;

            if (!decimal.TryParse(PriceBox.Text, out price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            if (!int.TryParse(QuantityBox.Text, out quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            var selectedCategory = CategoryBox.SelectedItem as Categories;
            if (selectedCategory == null)
            {
                MessageBox.Show("Invalid category selected.");
                return;
            }
            int categoryId = selectedCategory.Id;

            DateTime selectedDate = DateBox.Value;

            try
            {
                databaseHelper.AddProduct(NameBox.Text, categoryId, price, quantity, selectedDate);

                RefreshListView();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void RefreshListView()
        {
            mainFormListView.Items.Clear();

            List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> productList = databaseHelper.GetProducts();

            List<Product> products = MapToProducts(productList);

            foreach (var product in products)
            {
                var listViewItem = new ListViewItem(new string[]
                {
            product.ID.ToString(),
            product.Name,
            product.Category.Name,
            product.Quantity.ToString(),
            product.Price.ToString("C"),
            product.Date.ToString("yyyy-MM-dd")
                });
                mainFormListView.Items.Add(listViewItem);
            }
        }






        private void QuantityBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void PriceBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; 
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private List<Product> MapToProducts(List<(int Id, string Name, string Category, decimal Price, int Quantity, string Date)> productList)
        {
            var products = new List<Product>();

            foreach (var item in productList)
            {
                var product = new Product
                {
                    ID = item.Id,
                    Name = item.Name,
                    Category = new Categories { Name = item.Category },
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Date = DateTime.Parse(item.Date) 
                };
                products.Add(product);
            }

            return products;
        }



        private void AddProductFormcs_Load(object sender, EventArgs e)
        {

        }
    }
}
