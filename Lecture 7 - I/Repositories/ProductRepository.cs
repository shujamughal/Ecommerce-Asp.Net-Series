using Microsoft.EntityFrameworkCore;
using Project_Based_Learning.Data;
using Project_Based_Learning.Models;

namespace Project_Based_Learning.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // **Eager Loading**: Includes Category when fetching all products
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        // **Eager Loading**: Fetches product with its category if needed immediately
        public Product GetProductById(int id)
        {
            return _context.Products
                           .Include(p => p.Category) // Eager loading for Category
                           .FirstOrDefault(p => p.Id == id)!;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        // **Lazy Loading**: Loads related data only when accessed
        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id); // Lazy loading (Category is not loaded)
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        // **Explicit Loading Example**: Loading related data on demand
        public Product? GetProductWithCategoryLazy(int id)
        {
            var product = _context.Products.Find(id); // Product is loaded without Category

            if (product != null)
            {
                _context.Entry(product).Reference(p => p.Category).Load(); // Load category explicitly
            }

            return product;
        }
    }
}
