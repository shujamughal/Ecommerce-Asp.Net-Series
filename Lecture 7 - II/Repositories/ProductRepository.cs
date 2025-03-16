using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project_Based_Learning.Data;
using Project_Based_Learning.Models;

namespace Project_Based_Learning.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public ProductRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;

            // Define cache expiration policies
            _cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache expires in 10 minutes
                SlidingExpiration = TimeSpan.FromMinutes(5) // Reset expiration if accessed within 5 minutes
            };
        }

        // **Eager Loading with Caching**: Store products in memory to reduce DB calls
        public IEnumerable<Product> GetAllProducts()
        {
            const string cacheKey = "AllProducts";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product>? products))
            {
                products = _context.Products.Include(p => p.Category).ToList();

                if (products != null)
                {
                    _cache.Set(cacheKey, products, _cacheOptions);
                }
            }

            return products ?? new List<Product>();
        }

        // **Eager Loading with Caching**: Cache single product lookup
        public Product GetProductById(int id)
        {
            string cacheKey = $"Product_{id}";

            if (!_cache.TryGetValue(cacheKey, out Product? product))
            {
                product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.Id == id);

                if (product != null)
                {
                    _cache.Set(cacheKey, product, _cacheOptions);
                }
            }

            return product!;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            _cache.Remove("AllProducts"); // Clear cache to ensure updated list
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();

            _cache.Remove("AllProducts"); // Clear list cache
            _cache.Remove($"Product_{product.Id}"); // Remove specific product cache
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

                _cache.Remove("AllProducts"); // Clear all products cache
                _cache.Remove($"Product_{id}"); // Remove specific product cache
            }
        }

        // **Explicit Loading Example with Lazy Loading**
        public Product? GetProductWithCategoryLazy(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Entry(product).Reference(p => p.Category).Load();
            }

            return product;
        }
    }
}
