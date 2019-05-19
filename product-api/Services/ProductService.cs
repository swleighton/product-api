using productapi.Models;
using System;
using System.Collections.Generic;

namespace productapi.Services
{
    public class ProductService
    {
        private IDictionary<string, Product> Products;

        public ProductService(IDictionary<string, Product> products) 
        {
            Products = products;
        }

        public Product Add(string id, string description, string brand, string model)
        {
            if (Products.ContainsKey(id))
                throw new ProductExistsException("A product already exists with the Id:" + id);
            
            Product product = new Product(id, description, brand, model);
            Products.Add(product.Id, product);
            return product;

        }

        public ICollection<Product> GetAll()
        {
            return Products.Values;
        }
    }
}
