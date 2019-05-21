using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace productapi.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }

        public Product()
        {

        }

        public Product(string id, string description, string brand, string model)
        {
            Id = id;
            Description = description;
            Brand = brand;
            Model = model;
        }

        public bool IsComplete()
        {
            foreach (PropertyInfo FI in this.GetType().GetProperties())
            {
                object fieldValue = FI.GetValue(this);

                if (fieldValue == null)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            // If the passed object is null
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Product))
            {
                return false;
            }

            Product otherProduct = obj as Product;

            return (Id == otherProduct.Id && Description == otherProduct.Description && Model == otherProduct.Model && Brand == otherProduct.Brand);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}