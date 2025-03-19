using Microsoft.AspNetCore.Mvc;
using Project_Based_Learning.Helpers;
using Project_Based_Learning.Helpers.Project_Based_Learning.Helpers;
using Project_Based_Learning.Models;
using Project_Based_Learning.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly IProductRepository _productRepository;
    private const string CartCookieKey = "Cart";

    public CartController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        List<CartItem> cart = Request.GetObjectFromJson<List<CartItem>>(CartCookieKey) ?? new List<CartItem>();

        var existingItem = cart.FirstOrDefault(x => x.ProductId == id);
        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            });
        }

        Response.SetObjectAsJson(CartCookieKey, cart);
        TempData["Message"] = $"{product.Name} added to cart!";
        return RedirectToAction("ProductsShopping", "Product");
    }

    public IActionResult Cart()
    {
        var cart = Request.GetObjectFromJson<List<CartItem>>(CartCookieKey) ?? new List<CartItem>();
        return View(cart);
    }

    public IActionResult Remove(int id)
    {
        List<CartItem> cart = Request.GetObjectFromJson<List<CartItem>>(CartCookieKey) ?? new List<CartItem>();
        var itemToRemove = cart.FirstOrDefault(x => x.ProductId == id);

        if (itemToRemove != null)
        {
            cart.Remove(itemToRemove);
            Response.SetObjectAsJson(CartCookieKey, cart);
        }

        return RedirectToAction("Cart");
    }

    public IActionResult ClearCart()
    {
        Response.RemoveCookie(CartCookieKey);
        TempData["Message"] = "Cart cleared!";
        return RedirectToAction("Cart");
    }

    

    public IActionResult UpdateCart(int id, int quantity)
    {
        if (quantity < 1) quantity = 1; // Prevent negative or zero quantity

        List<CartItem> cart = Request.GetObjectFromJson<List<CartItem>>(CartCookieKey) ?? new List<CartItem>();
        var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

        if (existingItem != null)
        {
            existingItem.Quantity = quantity;
        }

        Response.SetObjectAsJson(CartCookieKey, cart);
        return RedirectToAction("Cart");
    }

}
