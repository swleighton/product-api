using System;
using System.Collections.Generic;
using productapi.Models;

namespace productapi.Tests
{
    public class TemplateProducts
    {

        public static Dictionary<string, Product> Products = new Dictionary<string, Product>
       {
            {"DYNS1", new Product("DYNS1", "Dyson Cyclone V10 Animal", "Dyson", "226419-01") },
            {"SMSNG1", new Product("SMSNG1", "Samsung Galaxy S9+ 64GB (Midnight Black)", "Samsung", "1091005388")}
        };

        //Clone of Products property - used if the original is passed to unit tests as the in memory dictionary that will be manipulated during execution 
        public static Dictionary<string, Product> ComparisonProducts = new Dictionary<string, Product>
       {
            {"DYNS1", new Product("DYNS1", "Dyson Cyclone V10 Animal", "Dyson", "226419-01") },
            {"SMSNG1", new Product("SMSNG1", "Samsung Galaxy S9+ 64GB (Midnight Black)", "Samsung", "1091005388")},
        };

        public static Product FakeProduct = new Product("I Don't Exist", "", "", "");
    };
}
