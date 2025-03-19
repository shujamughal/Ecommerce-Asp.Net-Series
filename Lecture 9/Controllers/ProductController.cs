using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Based_Learning.Models;
using Project_Based_Learning.Repositories;
using System.Linq;

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

        public IActionResult Index()
        {
            var products = _productRepository.GetAllProducts();
            foreach (var product in products)
            {
                product.Category = _categoryRepository.GetCategoryById(product.CategoryId);
            }
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    if (!ValidateImage(product.ImageFile))
                    {
                        return View(product);
                    }

                    product.ImageUrl = GetPath(product.ImageFile);
                }
                else {
                    product.ImageUrl = "Uploads/default.jpg";
                }

                _productRepository.AddProduct(product);
                return RedirectToAction("Index");
            }

            var categories = _categoryRepository.GetAllCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(product);

        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
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
                _productRepository.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(product);
        }

       

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
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
                ModelState.AddModelError("ImageFile", "File size must be less than 2MB.");
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
