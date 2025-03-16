namespace Project_Based_Learning.Models
{
    public class Product
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Product Name
        public string Description { get; set; }  // Short Description
        public decimal Price { get; set; }  // Product Price
        public string ImageUrl { get; set; }  // Image Path

        // Foreign Key for Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }  // Navigation Property
    }

}
