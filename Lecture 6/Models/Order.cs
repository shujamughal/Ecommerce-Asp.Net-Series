using Microsoft.AspNetCore.Identity;

namespace Project_Based_Learning.Models
{
    public class Order
    {
        public int Id { get; set; }  // Primary Key
        public DateTime OrderDate { get; set; }  // Order Date
        public decimal TotalAmount { get; set; }  // Total Amount

        public string UserId { get; set; }  // Foreign Key for ApplicationUser
        public IdentityUser User { get; set; }  // Navigation Property

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }

}