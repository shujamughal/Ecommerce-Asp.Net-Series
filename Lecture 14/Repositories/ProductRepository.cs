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
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            const string cacheKey = "AllProducts";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Product>? products))
            {
                products = await _context.Products.Include(p => p.Category).ToListAsync();

                if (products != null)
                {
                    _cache.Set(cacheKey, products, _cacheOptions);
                }
            }

            return products!;
        }

        // **Eager Loading with Caching**: Cache single product lookup
        public async Task<Product> GetProductByIdAsync(int id)
        {
            string cacheKey = $"Product_{id}";

            if (!_cache.TryGetValue(cacheKey, out Product? product))
            {
                product = await _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    _cache.Set(cacheKey, product, _cacheOptions);
                }
            }

            return product!;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            _cache.Remove("AllProducts"); // Clear cache to ensure updated list
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            _cache.Remove("AllProducts"); // Clear list cache
            _cache.Remove($"Product_{product.Id}"); // Remove specific product cache
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _cache.Remove("AllProducts"); // Clear all products cache
                _cache.Remove($"Product_{id}"); // Remove specific product cache
            }
        }

        // **Explicit Loading Example with Lazy Loading**
        public async Task<Product?> GetProductWithCategoryLazyAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();
            }

            return product;
        }
    }
}
