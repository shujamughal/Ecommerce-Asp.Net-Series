namespace Lecture2_Setting_Up_the_E_Commerce_Application.Models
{
    public class Product
    {
        public int Id { get; set; }  // Unique identifier for each product
        public string Name { get; set; }  // Product name
        public string Description { get; set; }  // Short product description
        public decimal Price { get; set; }  // Product price
        public string ImageUrl { get; set; }  // URL for product image
    }

}
