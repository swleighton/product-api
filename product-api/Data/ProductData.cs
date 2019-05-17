using System;
using System.Collections.Generic;
using productapi.Models;

namespace productapi.Data
{
    public static class ProductData
    {
        private static List<Product> Cache;
        private static object CacheLock = new object();
        public static List<Product> Products
        {
            get
            {
                lock (CacheLock)
                {
                    if (Cache == null)
                    {
                        Cache = new List<Product>();
                    }
                    return Cache;
                }
            }
        }
    }
}
