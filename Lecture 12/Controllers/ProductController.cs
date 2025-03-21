using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Project_Based_Learning.Models;
using Project_Based_Learning.Repositories;

namespace Project_Based_Learning.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment env,
            ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _env = env;
            _logger = logger;
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                foreach (var product in products)
                {
                    product.Category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
                }
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product list");
                return StatusCode(500, "An error occurred while fetching the products.");
            }
        }

        public async Task<IActionResult> ProductsShopping()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                foreach (var product in products)
                {
                    product.Category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
                }
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shopping products");
                return StatusCode(500, "An error occurred while fetching the shopping products.");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found", id);
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product details for ID {Id}", id);
                return StatusCode(500, "An error occurred while fetching product details.");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create product form");
                return StatusCode(500, "An error occurred while loading the create product form.");
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var categories = await _categoryRepository.GetAllCategoriesAsync();
                    ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

                    if (product.ImageFile != null)
                    {
                        if (!ValidateImage(product.ImageFile))
                        {
                            return View(product);
                        }
                        product.ImageUrl = GetPath(product.ImageFile);
                    }
                    else
                    {
                        product.ImageUrl = "Uploads/default.jpg";
                    }

                    await _productRepository.AddProductAsync(product);
                    _logger.LogInformation("Product {Name} created successfully", product.Name);
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "An error occurred while creating the product.");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found", id);
                    return NotFound();
                }

                var categories = await _categoryRepository.GetAllCategoriesAsync();
                ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form for product ID {Id}", id);
                return StatusCode(500, "An error occurred while loading the edit form.");
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var categories = await _categoryRepository.GetAllCategoriesAsync();
                    ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

                    if (product.ImageFile != null)
                    {
                        if (!ValidateImage(product.ImageFile))
                        {
                            return View(product);
                        }
                        product.ImageUrl = GetPath(product.ImageFile);
                    }
                    else
                    {
                        product.ImageUrl = "Uploads/default.jpg";
                    }

                    await _productRepository.UpdateProductAsync(product);
                    _logger.LogInformation("Product {Name} updated successfully", product.Name);
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product ID {Id}", product.Id);
                return StatusCode(500, "An error occurred while updating the product.");
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
                _logger.LogInformation("Product ID {Id} deleted successfully", id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the product.");
            }
        }

        private bool ValidateImage(IFormFile picture)
        {
            if (picture == null)
            {
                _logger.LogWarning("Image file is missing.");
                ModelState.AddModelError("ImageFile", "Please select an image.");
                return false;
            }

            if (picture.Length > 1 * 1024 * 1024) // 1MB limit
            {
                _logger.LogWarning("Image file size exceeded limit.");
                ModelState.AddModelError("ImageFile", "File size must be less than 1MB.");
                return false;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(picture.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Invalid image file extension.");
                ModelState.AddModelError("ImageFile", "Only .jpg, .jpeg, .png, .gif, .webp files are allowed.");
                return false;
            }

            return true;
        }

        private string GetPath(IFormFile picture)
        {
            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "Uploads");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + picture.FileName;
            string filePath = Path.Combine(path, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                picture.CopyTo(fileStream);
            }

            return Path.Combine("Uploads", uniqueFileName);
        }
    }
}
