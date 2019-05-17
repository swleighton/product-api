using productapi.Models;
using System.Collections.Generic;

namespace productapi.Services
{
    public class ProductService
    {
        private IList<Product> Products;

        public ProductService(IList<Product> products)
        {
            Products = products;
        }

        public Product Add(string description, string brand, string model)
        {
            Product product = new Product("1", description, brand, model);
            Products.Add(product);
            return product;
        }

        public IList<Product> GetAll()
        {
            return Products;
        }
    }
}
