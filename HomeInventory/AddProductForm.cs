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
        public AddProductForm()
        {
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

            if (DateBox.Value == null)
            {
                MessageBox.Show("Please select a date.");
                return;
            }

            try
            {
                databaseHelper.AddProduct(NameBox.Text, categoryId, price, quantity);
                MessageBox.Show("Product added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void AddProductFormcs_Load(object sender, EventArgs e)
        {

        }
    }
}
