using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Based_Learning.Models;
using Project_Based_Learning.Repositories;

namespace Project_Based_Learning.Controllers
{

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
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
    }
}
