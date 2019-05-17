using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace productapi.Models
{
    public class Product
    {
        public string Id { get; private set; }
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
    }
}