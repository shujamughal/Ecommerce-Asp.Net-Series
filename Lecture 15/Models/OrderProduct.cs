namespace Project_Based_Learning.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }  // Navigation Property

        public int ProductId { get; set; }
        public Product Product { get; set; }  // Navigation Property
    }

}
