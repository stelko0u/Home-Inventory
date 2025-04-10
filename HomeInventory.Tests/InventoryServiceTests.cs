using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using HomeInventory;

namespace HomeInventory.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public void GetProductData_ShouldReturnCorrectProducts()
        {
            var mockService = new Mock<IInventoryService>();
            mockService.Setup(service => service.GetProductData())
                .Returns(new List<(string, int, decimal)>
                {
                    ("Laptop", 3, 999.99m),
                    ("Mouse", 5, 25.50m)
                });

            var result = mockService.Object.GetProductData();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal(3, result[0].Quantity);
            Assert.Equal(999.99m, result[0].Price);
        }


        [Fact]
        public void GetCategorySpending_ShouldReturnCorrectTotals()
        {
            var mockService = new Mock<IInventoryService>();
            mockService.Setup(service => service.GetCategorySpending())
                .Returns(new Dictionary<string, double>
                {
                    { "Electronics", 3025.45 },
                    { "Furniture", 1278.00 }
                });

            var result = mockService.Object.GetCategorySpending();

            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey("Electronics"));
            Assert.Equal(3025.45, result["Electronics"]);
        }
    }
}
