using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project_Based_Learning.Data;

public class UniqueProductNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Console.WriteLine("UniqueProductName validation executed..."); // Check if this appears in logs

        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("Product name is required.");
        }

        var _context = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
        if (_context == null)
        {
            throw new InvalidOperationException("Database context is not available.");
        }

        string productName = value.ToString().Trim().ToLower();
        Console.WriteLine($"Checking for duplicate: {productName}");

        var instance = validationContext.ObjectInstance;
        var productIdProperty = instance.GetType().GetProperty("Id");

        int productId = productIdProperty != null ? (int)productIdProperty.GetValue(instance) : 0;

        bool exists = _context.Products
            .AsNoTracking()
            .Any(p => p.Name.ToLower() == productName && p.Id != productId);

        if (exists)
        {
            Console.WriteLine("Duplicate product found!");
            return new ValidationResult("Product name already exists. Choose a different name.");
        }

        Console.WriteLine("Product name is unique.");
        return ValidationResult.Success;
    }

}
