using System;
using System.Reflection;
using productapi.Models;

namespace productapi.Models
{
    public class HomepageViewModel
    {
        /// <summary>
        /// List of all fields of a product
        /// </summary>
        public PropertyInfo[] ProductParams = typeof(Product).GetProperties();
    }
}
