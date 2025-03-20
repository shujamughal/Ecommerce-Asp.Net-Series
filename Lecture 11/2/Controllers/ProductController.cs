using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Based_Learning.Models;
using Project_Based_Learning.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Based_Learning.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _env = env;
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();
            foreach (var product in products)
            {
                product.Category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            }
            return View(products);
        }


        [Authorize]
        public async Task<IActionResult> ProductsShopping()
        {
            var products = await _productRepository.GetAllProductsAsync();
            foreach (var product in products)
            {
                product.Category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            }
            return View(products);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(Product product)
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
                return RedirectToAction("Index");
            }

            
            return View(product);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(Product product)
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
                return RedirectToAction("Index");
            }
            
            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

        private bool ValidateImage(IFormFile picture)
        {
            if (picture == null)
            {
                ModelState.AddModelError("ImageFile", "Please select an image.");
                return false;
            }

            if (picture.Length > 1 * 1024 * 1024) // 1MB limit
            {
                ModelState.AddModelError("ImageFile", "File size must be less than 1MB.");
                return false;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(picture.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImageFile", "Only .jpg, .jpeg, .png, .gif files are allowed.");
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
            string UniqueFileName = Guid.NewGuid().ToString() + "_" + picture.FileName;
            if (picture != null && picture.Length > 0)
            {
                path = Path.Combine(path, UniqueFileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    picture.CopyTo(fileStream);
                }
            }
            return Path.Combine("Uploads", UniqueFileName);
        }
    }
}
