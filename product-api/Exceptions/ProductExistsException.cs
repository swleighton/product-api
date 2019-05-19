using System;

public class ProductExistsException : Exception
{
    public ProductExistsException(string message) : base(message)
    {
    }
}