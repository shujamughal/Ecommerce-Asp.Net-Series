using Project_Based_Learning.Data;
using System.ComponentModel.DataAnnotations;

namespace Project_Based_Learning.Models
{
    public class Product
    {
        public int Id { get; set; }  // Primary Key

        [UniqueProductName]
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Product Name must be between 2 and 100 characters", MinimumLength = 2)]
        public string Name { get; set; }  // Product Name

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }  // Short Description

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000, ErrorMessage = "Price must be between $1 and $10,000")]
        public decimal Price { get; set; }  // Product Price

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid image URL format")]
        public string ImageUrl { get; set; }  // Image Path

        // Foreign Key for Category
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }  // Navigation Property
    }

    public class UniqueProductNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var existingProduct = _context.Products.Any(p => p.Name == value.ToString());

            if (existingProduct)
            {
                return new ValidationResult("Product name already exists. Choose a different name.");
            }

            return ValidationResult.Success;
        }
    }

}
