using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TSPProject;

namespace HomeInventory
{
    public partial class AddCategory : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private HomeForm homeForm;

        public AddCategory(HomeForm form)
        {
            InitializeComponent();
            homeForm = form;
        }


        private void AddCategory_Load(object sender, EventArgs e)
        {
            // Any initialization code if needed
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string categoryName = textBox1.Text.Trim();
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    MessageBox.Show("Category name cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dbHelper.AddCategory(categoryName);
                MessageBox.Show("Category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                this.Close();

                homeForm.LoadCategories();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
