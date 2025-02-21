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

    public partial class Form1 : Form
    {
        public ListView listView => _storage;
        private readonly ListView _storage;
        private readonly ProductListManager productManager;
        DatabaseHelper dbHelper = new DatabaseHelper();

        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.Columns.Add("ID", 50);
            listView1.Columns.Add("Name", 100);
            listView1.Columns.Add("Category", 100);
            listView1.Columns.Add("Quantity", 70);
            listView1.Columns.Add("Price", 70);
            listView1.Columns.Add("Date", 120);
            productManager = new ProductListManager(listView1);
            PopulateListView();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void PopulateListView()
        {
            productManager.AddItemsToListView();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var addProductWindow = new AddProductForm();
            addProductWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) 
            {
                int productId = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text); 
                var confirmResult = MessageBox.Show("Are you sure you want to delete this product?",
                                                     "Confirm Delete Product",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    dbHelper.DeleteProduct(productId);

                    listView1.Items.Remove(listView1.SelectedItems[0]);

                    // moje da go mahnem 
                    MessageBox.Show("The product has been successfully deleted.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }


    }
}
