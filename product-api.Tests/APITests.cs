using NUnit.Framework;
using productapi.Models;
using System.Collections.Generic;
using productapi.Controllers;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net.Http;

namespace productapi.Tests
{
    [TestFixture]
    public class APITests
    {
        [Test]
        public void AddProductsAddsAProductFromJSONAndReturnsTheSameValidProduct()
        {
            Product templateProduct = TemplateProducts.Products["DYNS1"];

            ProductsController controller = new ProductsController(new Dictionary<string, Product>());
            IHttpActionResult setProductResult = controller.SetProduct(JObject.FromObject(templateProduct));
            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(productResult.Content, templateProduct);
        }

        [Test]
        public void AddProductseReturnsAnErrorIfAllPropsAreNotPassed()
        {
            Product templateProduct = new Product(null, null, null, null);

            ProductsController controller = new ProductsController(new Dictionary<string, Product>());
            IHttpActionResult setProductResult = controller.SetProduct(JObject.FromObject(templateProduct));
            BadRequestErrorMessageResult productResult = setProductResult as BadRequestErrorMessageResult;

            Assert.IsNotNull(productResult); 
            Assert.AreEqual("Posting product failed - Post data was not formatted correctly. Please supply a JSON object the key/value pairs of description:value, brand:string and model:string", productResult.Message);
        }

        [Test]
        public void AddProductseReturnsAnErrorIfIDAlreadyExists()
        {
            Product templateProduct = TemplateProducts.Products["DYNS1"];

            ProductsController controller = new ProductsController(TemplateProducts.Products);
            IHttpActionResult setProductResult = controller.SetProduct(JObject.FromObject(templateProduct));
            BadRequestErrorMessageResult productResult = setProductResult as BadRequestErrorMessageResult;

            Assert.IsNotNull(productResult);
            Assert.AreEqual("A product already exists with the Id:DYNS1", productResult.Message);
        }

        [Test]
        public void GetProductReturnsTheCorrectProduct()
        {
            Product templateProduct = TemplateProducts.Products["DYNS1"];

            ProductsController controller = new ProductsController(TemplateProducts.Products);

            IHttpActionResult setProductResult = controller.GetProduct(templateProduct.Id);
            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(productResult.Content, templateProduct);
        }

        [Test]
        public void GetProductReturnsErrorIfProductNotFound()
        {
            ProductsController controller = new ProductsController(TemplateProducts.Products);

            IHttpActionResult setProductResult = controller.GetProduct("NOT A PRODUCT");

            BadRequestErrorMessageResult productResult = setProductResult as BadRequestErrorMessageResult;
            Assert.AreEqual("Retriving product failed - No product found with the Id:NOT A PRODUCT", productResult.Message);
        }

        [Test]
        public void DeleteValidProductReturnsSucess()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "SMSNG1", TemplateProducts.Products["SMSNG1"] },
               { "DYNS1", TemplateProducts.Products["DYNS1"] }
            };

            ProductsController controller = new ProductsController(products);

            IHttpActionResult setProductResult = controller.DeleteProduct("DYNS1");
            OkResult productResult = setProductResult as OkResult;

            Assert.IsNotNull(productResult);

            Assert.AreEqual(new Dictionary<string, Product>{
               { "SMSNG1", TemplateProducts.Products["SMSNG1"] }
            }, products);
        }

        [Test]
        public void UpdateProductFromJSONObjectUpdatesProductCorrectly()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "MCAIR", new Product("MCAIR", "N/A", "N/A", "N/A") }
            };

            Product updatedProduct = new Product("MCAIR", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A");

            IHttpActionResult setProductResult = new ProductsController(products).UpdateProduct(JObject.FromObject(updatedProduct));

            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(updatedProduct, productResult.Content);
        }

        [Test]
        public void UpdateProductFromJSONObjectReturnsErrorIfProductNotFound()
        {
            ProductsController controller = new ProductsController(TemplateProducts.Products);

            IHttpActionResult setProductResult = controller.UpdateProduct(JObject.FromObject(new Product("MCAIR", "N/A", "N/A", "N/A")));

            BadRequestErrorMessageResult productResult = setProductResult as BadRequestErrorMessageResult;
            Assert.AreEqual("Updating the product failed - No product found with the Id:", productResult.Message);
        }

        [Test]
        public void UpdateProductFromJSONObjectAndIDUpdatesProductCorrectly()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "MCAIR", new Product("MCAIR", "N/A", "N/A", "N/A") }
            };

            Product updatedProduct = new Product("MCAIR", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A");

            IHttpActionResult setProductResult = new ProductsController(products).UpdateProduct("MCAIR", JObject.FromObject(updatedProduct));

            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(updatedProduct, productResult.Content);
        }

        [Test]
        public void UpdateProductUsesIDinURLOverJSONBodyObject()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "MCAIR", new Product("MCAIR", "N/A", "N/A", "N/A") }
            };

            Product updatedProduct = new Product("NOT AN ID", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A");

            IHttpActionResult setProductResult = new ProductsController(products).UpdateProduct("MCAIR", JObject.FromObject(new Product("NOT AN ID", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A")));

            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(new Product("MCAIR", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A"), productResult.Content);
        }

        [Test]
        public void UpdateProductAllowsPartialFields()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "MCAIR", new Product("MCAIR", "N/A", "N/A", "MQD32X/A") }
            };

            JObject updatedProduct =  JObject.Parse("{\"Description\": \"Apple MacBook Air 13-inch 128GB\", \"Brand\": \"Apple\" }");

            IHttpActionResult setProductResult = new ProductsController(products).UpdateProduct("MCAIR", updatedProduct);

            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(new Product("MCAIR", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A"), productResult.Content);
        }

        [Test]
        public void UpdateProductHandlesInvalidJSONObjects()
        {
            Product product = new Product("MCAIR", "Apple MacBook Air 13-inch 128GB", "Apple", "MQD32X/A");
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "MCAIR", product }
            };

            JObject updatedProduct = JObject.Parse("{\"NOTAFIELD\": \"Apple MacBook Air 13-inch 128GB\" }");

            IHttpActionResult setProductResult = new ProductsController(products).UpdateProduct("MCAIR", updatedProduct);

            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(product, productResult.Content);
        }

        [Test]
        public void UpdateProductFromJSONObjectAndIDReturnsErrorIfProductNotFound()
        {
            ProductsController controller = new ProductsController(TemplateProducts.Products);

            IHttpActionResult setProductResult = controller.UpdateProduct("MCAIR", JObject.FromObject(new Product("MCAIR", "N/A", "N/A", "N/A")));

            BadRequestErrorMessageResult productResult = setProductResult as BadRequestErrorMessageResult;
            Assert.AreEqual("Updating the product failed - No product found with the Id:", productResult.Message);
        }


        [Test]
        public void DeleteProductRemovesProductCorrectly()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>{
               { "SMSNG1", TemplateProducts.Products["SMSNG1"] },
               { "DYNS1", TemplateProducts.Products["DYNS1"] }
            };

            ProductsController controller = new ProductsController(products);

            controller.DeleteProduct("DYNS1");

            Assert.AreEqual(new Dictionary<string, Product>{
               { "SMSNG1", TemplateProducts.Products["SMSNG1"] }
            }, products);
        }

        [Test]
        public void DeleteProductReturnsErrorIfProductNotFound()
        {
            ProductsController controller = new ProductsController(TemplateProducts.Products);

            IHttpActionResult setProductResult = controller.DeleteProduct("NOT A PRODUCT");

            BadRequestErrorMessageResult productResult = setProductResult as BadRequestErrorMessageResult;
            Assert.AreEqual("Deleting product failed - Unable to find product by Id", productResult.Message);

        }

        [Test]
        public void GetAllProductsReturnsExpectedProducts()
        {
            ProductsController controller = new ProductsController(TemplateProducts.Products)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            Assert.AreEqual(new List<Product>(TemplateProducts.Products.Values), new List<Product>(controller.GetProducts()));
        }

        [Test]
        public void GetAllProductsWithFilterReturnsExpectedProducts()
        {
            ProductsController controller = new ProductsController(TemplateProducts.Products)
            {
                Request = new System.Net.Http.HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/?Brand=Dyson"),
                Configuration = new HttpConfiguration()
            };
            Assert.AreEqual(new List<Product> { TemplateProducts.Products["DYNS1"] }, new List<Product>(controller.GetProducts()));
        }


    }
}
