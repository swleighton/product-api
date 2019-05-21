using productapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace productapi.Services
{
    public class ProductService
    {
        private IDictionary<string, Product> Products;


        public ProductService(IDictionary<string, Product> products) 
        {
            Products = products;
        }

        public Product Add(Product product)
        {
            if(!product.IsComplete())
                throw new InvalidProductException("Posting product failed - Post data was not formatted correctly. Please supply a JSON object the key/value pairs of description:value, brand:string and model:string");
            else if (Products.ContainsKey(product.Id))
                throw new ProductExistsException("A product already exists with the Id:" + product.Id);
            
            Products.Add(product.Id, product);
            return product;

        }

        public ICollection<Product> GetAll()
        {
            return GetAll(new Product());
        }

        public ICollection<Product> GetAll(Product product)
        {
            ICollection<Product> products = Products.Values;


            foreach (PropertyInfo FI in product.GetType().GetProperties())
            {
                //Don't need to update the Id as we use this to reference the current object in storage
                if (FI.Name != "Id")
                {
                    object fieldValue = FI.GetValue(product);

                    if (fieldValue != null)
                    {
                        var dn = GetDynamicQueryWithExpresionTrees(FI.Name, FI.GetValue(product).ToString());
                        products = Products.Values.Where(dn).ToList();
                    }
                }
            }

            return products;
        }

        private static Func<Product, bool> GetDynamicQueryWithExpresionTrees(string propertyName, string val)
        {
            //x =>
            var param = Expression.Parameter(typeof(Product), "x");
            //val ("Curry")
            var valExpression = Expression.Constant(val, typeof(string));
            //x.LastName == "Curry"
            Expression body = Expression.Equal(Expression.Property(param, propertyName), valExpression);
            //x => x.LastName == "Curry"
            var final = Expression.Lambda<Func<Product, bool>>(body: body, parameters: param);
            //compiles the expression tree to a func delegate
            return final.Compile();
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
