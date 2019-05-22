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

        /// <summary>
        /// Checks if all product properties have values
        /// </summary>
        /// <returns><c>true</c>, if all properties have values, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:productapi.Models.Product"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:productapi.Models.Product"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:productapi.Models.Product"/>; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Serves as a hash function for a <see cref="T:productapi.Models.Product"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}