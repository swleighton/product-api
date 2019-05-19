using NUnit.Framework;
using productapi.Models;
using System.Collections.Generic;
using productapi.Controllers;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Web.Http.Results;

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
            IHttpActionResult setProductResult = controller.SetProducts(JObject.FromObject(templateProduct));
            OkNegotiatedContentResult<Product> productResult = setProductResult as OkNegotiatedContentResult<Product>;

            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(productResult.Content), Newtonsoft.Json.JsonConvert.SerializeObject(templateProduct));
        }

        [Test]
        public void GetAllProductsReturnsExpectedProducts()
        {
            ProductsController controller = new ProductsController(TemplateProducts.Products);
            Assert.AreEqual(new List<Product>(TemplateProducts.Products.Values), new List<Product>(controller.GetProducts()));
           
        }
    }
}
