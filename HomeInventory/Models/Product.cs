using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TSPProject;

namespace HomeInventory.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Categories Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }

    public class ProductListManager
    {
        private ListView _listView;
        private readonly DatabaseHelper _databaseHelper;

        public ProductListManager(ListView listView)
        {
            Products = new ObservableCollection<Product>();
            _listView = listView;
            _databaseHelper = new DatabaseHelper();
        }

        public ObservableCollection<Product> Products { get; private set; }
        public void AddItemsToListView()
        {
            _listView.Items.Clear();
            var products = _databaseHelper.GetProducts();
            foreach (var product in products)
            {
                Console.WriteLine(product.Name);
                var item = new ListViewItem(product.Id.ToString());
                item.SubItems.Add(product.Name);
                item.SubItems.Add(product.Category.ToString());
                item.SubItems.Add(product.Quantity.ToString());
                item.SubItems.Add(product.Price.ToString("C"));
                item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd"));

                _listView.Items.Add(item);
            }
        }
        public void RemoveProduct(Product product)
        {
            foreach (ListViewItem item in _listView.Items)
            {
                if (item.Text == product.ID.ToString())
                {
                    _listView.Items.Remove(item);
                    break;
                }
            }
        }

    }
}
