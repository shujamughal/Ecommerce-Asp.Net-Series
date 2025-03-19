namespace Project_Based_Learning.Models
{
    public class Category
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Category Name

        // Navigation Property for One-to-Many Relationship
        public ICollection<Product> Products { get; set; }
    }

}
