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
using productapi.Helpers;
using System.Web.Http.Description;

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
        /// Gets an ICollection of all current products filtered by query params of product props if specified
        /// </summary>
        /// 
        [ResponseType(typeof(ICollection<Product>))]
        [HttpGet]
        public ICollection<Product> GetProducts()
        {
            Product product = new Product();
            Dictionary<string, string> filterParams = this.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

            if (filterParams.Count > 0)
            {
                product = DictionaryToProduct.Convert(filterParams);
            }

            return ProductService.GetAll(product);
        }

        /// <summary>
        /// Retrives the product with the given ID
        /// </summary>
        /// <param name="id">Id of the product to retrive</param>
        /// <returns>Http 200 Ok and a copy of the product if successful, a http 400 Bad Request and error message if unsuccessful</returns>  

        [HttpGet]
        [ResponseType(typeof(Product))]
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
        /// <returns>Http 200 Ok and a copy of the product if successfully created, a http 400 Bad Request and error message if unsuccessful</returns>  
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult SetProduct([FromBody]Product data)
        {
            try
            {
                return Ok(ProductService.Add(data));
            }
            catch (ProductExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidProductException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Updates the product with the matching ID's fields with those passed in the request body         
        /// </summary>
        /// <param name="data">JSON structure of the key/value pairs of the product to update</param>
        /// <returns>Http 200 Ok if updated successfully, a http 400 Bad Request and error message if unsuccessful</returns>  
        [HttpPut]
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult UpdateProduct([FromBody]Product data)
        {
            try
            {
                return Ok(ProductService.Update(data));
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Updating the product failed - No product found with the Id:" + data.Id);
            }

        }

        /// <summary>
        /// Updates the product with the Id Passed in the url with the fields passed in the request body (excluding the Id)
        /// </summary>
        /// <param name="id">ID of the product to remove</param>
        /// <param name="data">JSON structure of the key/value pairs of the product to update</param>
        /// <returns>Http 200 Ok if updated successfully, a http 400 Bad Request and error message if unsuccessful</returns>  
        [HttpPut]
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult UpdateProduct(string id, [FromBody]Product data)
        {
            data.Id = id;
            return UpdateProduct(data);
        }

        /// <summary>
        /// Removes the product with the Id Passed
        /// </summary>
        /// <param name="id">ID of the product to remove</param>
        /// <returns>Http 200 Ok if deleted successfully, a http 400 Bad Request and error message if unsuccessful</returns>  
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteProduct(string id)
        {
            if (ProductService.Delete(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Deleting product failed - Unable to find product by Id");
            }
        }
    }
}
