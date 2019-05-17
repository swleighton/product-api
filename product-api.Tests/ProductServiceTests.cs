using NUnit.Framework;
using System;
using productapi.Models;
using productapi.Services;
using System.Collections.Generic;

namespace productapi.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        [Test]
        public void AddProduct()
        {
            ProductService productService = new ProductService(new List<Product>());
            Product product = productService.Add("Sony CH700N Wireless Noise Cancelling Headphones (Black)", "Sony", "WHCH700NB");
            Assert.AreEqual(product.Id, "1");
            Assert.AreEqual(product.Description, "Sony CH700N Wireless Noise Cancelling Headphones (Black)");
            Assert.AreEqual(product.Brand, "Sony");
            Assert.AreEqual(product.Model, "WHCH700NB");
        }

        [Test]
        public void GetAllProducts()
        {
            ProductService productService = new ProductService(new List<Product>());
            Product productA = productService.Add("Dyson Cyclone V10 Animal", "Dyson", "226419-01");
            Product productB = productService.Add("Samsung Galaxy S9+ 64GB (Midnight Black)", "Samsung", "1091005388");

            List<Product> products = productService.GetAll() as List<Product>;
            Assert.AreEqual(products.Count, 2);
            Assert.Contains(productA, products);
            Assert.Contains(productB, products);
        }
    }
}
