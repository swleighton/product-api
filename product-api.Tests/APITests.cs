using NUnit.Framework;
using System;
using productapi.Models;
using productapi.Services;
using System.Collections.Generic;
using productapi.Controllers;

namespace productapi.Tests
{
    [TestFixture]
    public class APITests
    {

        [Test]
        public void GetAllProducts()
        {
            IList<Product> templateProducts = CreateProducts();
            ProductsController controller = new ProductsController(templateProducts);

            IList<Product> products = controller.GetProducts();
            Assert.AreEqual(products.Count, 2);
            Assert.Contains(templateProducts[0], products as List<Product>);
            Assert.Contains(templateProducts[1], products as List<Product>);
        }

        private IList<Product> CreateProducts()
        {
            return new List<Product> {
                new Product("1", "Dyson Cyclone V10 Animal", "Dyson", "226419-01"),
                new Product("2", "Samsung Galaxy S9+ 64GB (Midnight Black)", "Samsung", "1091005388")
            };
        }
    }
}
