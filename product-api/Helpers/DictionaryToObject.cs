using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using productapi.Models;

namespace productapi.Helpers
{
    public static class DictionaryToProduct
    {
        public static Product Convert(IDictionary<string, string> dict) 
        {
            Product product = new Product();
            foreach (PropertyInfo FI in product.GetType().GetProperties())
            {
                //Don't need to update the Id as we use this to reference the current object in storage
                if (FI.Name != "Id")
                {
                    if (dict.ContainsKey(FI.Name))
                    {
                        FI.SetValue(product, dict[FI.Name].ToString());
                    }
                }
            }

            return product;
        }
    }
}
