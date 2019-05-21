using NUnit.Framework;
using productapi.Models;
using productapi.Services;
using System;
using System.Collections.Generic;

namespace productapi.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        [Test]
        public void AddProductReturnsAValidProductsThatMatchsTheFieldsPassed()
        {
            Product templateProduct = TemplateProducts.Products["DYNS1"];
            ProductService productService = new ProductService(new Dictionary<string, Product>());

            Product product = productService.Add(templateProduct.Id, templateProduct.Description, templateProduct.Brand, templateProduct.Model);

            Assert.AreEqual(templateProduct, product);
        }

        [Test]
        public void AddProductThrowsAProductExistsExceptionForDuplicateIDs()
        {
            Product templateProduct = TemplateProducts.Products["DYNS1"];
            ProductService productService = new ProductService(new Dictionary<string, Product> {
                { "DYNS1",  TemplateProducts.Products["DYNS1"] }
            });

            Assert.Throws<ProductExistsException>(() => productService.Add(templateProduct.Id, templateProduct.Description, templateProduct.Brand, templateProduct.Model));
        }

        [Test]
        public void GetAllProductsReturnsTheCorrectListOfProductsAsCollection()
        {
            ProductService productService = new ProductService((TemplateProducts.Products));

            ICollection<Product> products = productService.GetAll();

            Assert.AreEqual(TemplateProducts.ComparisonProducts.Values , products);
        }

        [Test]
        public void GetProductReturnsTheCorrectProducts()
        {
            ProductService productService = new ProductService((TemplateProducts.Products));

            Product product = productService.Get("DYNS1");

            Assert.AreEqual(TemplateProducts.ComparisonProducts["DYNS1"], product);
        }

        [Test]
        public void GetProductKeyNotFoundExceptionIfProductNotFound()
        {
            ProductService productService = new ProductService((TemplateProducts.Products));

            Assert.Throws<KeyNotFoundException>(() => productService.Get("NO PRODUCT"));
        }

        [Test]
        public void UpdateProductReturnsTheCorrectProduct()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "MCAIR", new Product("MCAIR", "N/A", "N/A", "N/A") }
            };

            Product updatedProduct = new Product("MCAIR", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A");

            Product productService = new ProductService(products).Update(updatedProduct);

            Assert.AreEqual(updatedProduct, products["MCAIR"]);
        }

        [Test]
        public void UpdateProductThrowKeyNotFoundExceptionIfProductNotFound()
        {
            ProductService productService = new ProductService((TemplateProducts.Products));
            Assert.Throws<KeyNotFoundException>(() => productService.Update(TemplateProducts.FakeProduct));
        }

        [Test]
        public void DeleteProductReturnsTrueAndRemovesTheCorrectProduct()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "SMSNG1", TemplateProducts.Products["SMSNG1"] },
               { "DYNS1", TemplateProducts.Products["DYNS1"] }
            };

            ProductService productService = new ProductService(products);

            Assert.IsTrue(productService.Delete("DYNS1"));

            Assert.AreEqual(products, new Dictionary<string, Product>
            {
                { "SMSNG1", TemplateProducts.Products["SMSNG1"] }
            });
        }

        [Test]
        public void DeleteProductReturnsFalseIfProductNotFound()
        {
            ProductService productService = new ProductService((TemplateProducts.Products));
            Assert.IsFalse(productService.Delete("NO PRODUCT"));
        }
    }
}
