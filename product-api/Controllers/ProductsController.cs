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
    public class ProductsController : ApiController
    {

        ProductService ProductService;

        public ProductsController()
        {
            ProductService = new ProductService((ProductData.Products));
        }

        //Provide a custom Dictionary for product storage
        public ProductsController(IDictionary<string, Product> products)
        {
            ProductService = new ProductService(products);
        }


        [HttpGet]
        public ICollection<Product> GetProducts()
        {
            ICollection<Product>  product = ProductService.GetAll();
            int test = product.Count;
            return product;
        }

        [HttpPost]
        public IHttpActionResult SetProducts([FromBody]JObject data)
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
