using productapi.Models;
using productapi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Runtime;
using Newtonsoft.Json.Linq;
using System.Collections;
using productapi.Data;
using System.Web;

namespace productapi.Controllers
{
    /// <summary>
    /// HttpRequest handlers for product API
    /// </summary>
    public class ProductsController : ApiController
    {

        ProductService ProductService;

        /// <summary>
        /// Creates a Product Controller instance with the default global singleton Dictionary for product storage
        /// </summary>
        public ProductsController()
        {
            ProductService = new ProductService((ProductData.Products));
        }

        /// <summary>
        /// Creates a Product Controller instance with a custom Dictionary for product storage
        /// </summary>
        /// <param name="products">The IDictionary of products</param>
        public ProductsController(IDictionary<string, Product> products)
        {
            ProductService = new ProductService(products);
        }



        /// <summary>
        /// Gets an ICollection of all current products
        /// </summary>
        [HttpGet]
        public ICollection<Product> GetProducts()
        {
            ICollection<Product>  product = ProductService.GetAll();
            int test = product.Count;
            return product;
        }

        [HttpGet]
        public IHttpActionResult GetProduct(string id)
        {
            try
            {
                return Ok(ProductService.Get(id));
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Retriving product failed - No product found with the Id:" + id);
            }
        }

        /// <summary>
        /// Creates a new product entry
        /// </summary>
        /// <param name="data">The JSON Object of a product</param>
        /// <returns>Http 200 Ok and a copy of the product sucessfully, a http 400 Bad Request and error message if unsucessful</returns>  
        [HttpPost]
        public IHttpActionResult SetProduct([FromBody]JObject data)
        {
            try
            {
                Product product = ProductService.Add(data.GetValue("Id").ToString(), data.GetValue("Description").ToString(), data.GetValue("Brand").ToString(), data.GetValue("Model").ToString());
                return Ok(product);
            }
            catch (NullReferenceException)
            {
                return BadRequest("Posting product failed - Post data was not formatted correctly. Please supply a JSON object the key/value pairs of description:value, brand:string and model:string");
            }
            catch (ProductExistsException)
            {
                return BadRequest("Posting product failed - A product with that Id already exists.");
            }
        }
    }
}
