using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInventory
{
    public interface IInventoryService
    {
        List<(string Name, int Quantity, decimal Price)> GetProductData();
        Dictionary<string, double> GetCategorySpending();
    }
}
