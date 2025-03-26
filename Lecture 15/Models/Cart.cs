namespace Project_Based_Learning.Models
{
    public class Cart
    {
        public string? UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
