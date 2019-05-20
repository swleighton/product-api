using NUnit.Framework;
using productapi.Models;
using productapi.Services;
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
        public void GetProductReturnsTheCorrecProducts()
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
    }
}
