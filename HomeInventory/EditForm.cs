using System;
using System.Linq;
using System.Windows.Forms;
using TSPProject;

namespace HomeInventory
{
    public partial class EditForm : Form
    {
        private int productId;

        public EditForm(int id, string name, string category, int quantity, decimal price, DateTime date)
        {
            InitializeComponent();

            productId = id;
            NameBox.Text = name;
            CategoryBox.Text = category;
            QuantityBox.Text = quantity.ToString();
            PriceBox.Text = price.ToString("0.00");
            DateBox.Value = date;
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                var categories = dbHelper.GetCategories();

                CategoryBox.Items.Clear();

                foreach (var category in categories)
                {
                    CategoryBox.Items.Add(category.Name);
                }

                CategoryBox.SelectedItem = CategoryBox.Items
                    .Cast<string>()
                    .FirstOrDefault(c => c == CategoryBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string updatedName = NameBox.Text;
                string updatedCategory = CategoryBox.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(updatedName))
                {
                    MessageBox.Show("Please enter a valid name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(updatedCategory))
                {
                    MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(QuantityBox.Text, out int updatedQuantity) || updatedQuantity < 0)
                {
                    MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(PriceBox.Text, out decimal updatedPrice) || updatedPrice < 0)
                {
                    MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime updatedDate = DateBox.Value;

                DatabaseHelper dbHelper = new DatabaseHelper();
                int categoryId = dbHelper.GetCategoryId(updatedCategory);

                dbHelper.UpdateProduct(productId, updatedName, categoryId, updatedPrice, updatedQuantity, updatedDate);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void QuantityBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void PriceBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && PriceBox.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}
