using System;
using System.Reflection;
using productapi.Models;

namespace productapi.Models
{
    public class HomepageViewModel
    {
        public PropertyInfo[] ProductParams = typeof(Product).GetProperties();
    }
}
