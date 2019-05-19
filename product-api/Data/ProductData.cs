using System;
using System.Collections.Generic;
using productapi.Models;

namespace productapi.Data
{
    public static class ProductData
    {
        private static Dictionary<string, Product> Cache;
        private static object CacheLock = new object();
        public static Dictionary<string, Product> Products
        {
            get
            {
                lock (CacheLock)
                {
                    if (Cache == null)
                    {
                        Cache = new Dictionary<string, Product>();
                    }
                    return Cache;
                }
            }
        }
    }
}
