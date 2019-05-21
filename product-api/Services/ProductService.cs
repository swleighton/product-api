using productapi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

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

        public Product Get(string id)
        {
            return Products[id];
        }

        public Product Update(Product product)
        {
            foreach (PropertyInfo FI in product.GetType().GetProperties())
            {
                //Don't need to update the Id as we use this to reference the current object in storage
                if (FI.Name != "Id")
                {
                    object fieldValue = FI.GetValue(product);

                    if (fieldValue != null)
                    {
                        FI.SetValue(Products[product.Id], fieldValue.ToString());
                    }
                }
            }

            return Products[product.Id];
        }

        public bool Delete(string id)
        {
            return Products.Remove(id);
        }
    }
}
