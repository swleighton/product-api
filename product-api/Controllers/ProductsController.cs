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

        public ProductsController(IList<Product> products)
        {
            ProductService = new ProductService(products);
        }


        [HttpGet]
        public IList<Product> GetProducts()
        {
            return ProductService.GetAll();
        }

        [HttpPost]
        public IHttpActionResult SetProducts([FromBody]JObject data)
        {
            try
            {
                Product product = ProductService.Add(data.GetValue("description").ToString(), data.GetValue("brand").ToString(), data.GetValue("model").ToString());
                return Ok(product);
            }
            catch (NullReferenceException)
            {
                return BadRequest("Post data was not formatted correctly. Please supply a JSON object the key/value pairs of description:value, brand:string and model:string");
            }

        }
    }
}
