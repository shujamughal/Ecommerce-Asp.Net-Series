using Microsoft.AspNetCore.Mvc;
using Project_Based_ASP.NET_Core_MVC.Models;

namespace Project_Based_ASP.NET_Core_MVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 1200, ImageUrl = "/images/laptop.jpg" },
            new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone model", Price = 800, ImageUrl = "/images/phone.jpg" }
        };

            return View(products); // Passing product list to the view
        }
    }
}
