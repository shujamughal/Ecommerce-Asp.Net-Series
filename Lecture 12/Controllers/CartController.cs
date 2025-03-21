using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Based_Learning.Helpers;
using Project_Based_Learning.Helpers.Project_Based_Learning.Helpers;
using Project_Based_Learning.Models;
using Project_Based_Learning.Repositories;

[Authorize(Policy = "CustomerOnly")]
public class CartController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public CartController(IProductRepository productRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _productRepository = productRepository;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    private string GetCartCookieKey()
    {
        var user = _userManager.GetUserId(User);
        return string.IsNullOrEmpty(user) ? "GuestCart" : $"Cart_{user}";
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        string cartKey = GetCartCookieKey();
        List<CartItem> cart = Request.GetObjectFromJson<List<CartItem>>(cartKey) ?? new List<CartItem>();

        var existingItem = cart.FirstOrDefault(x => x.ProductId == id);
        if (existingItem != null)
        {
            existingItem.Quantity++;
            if (existingItem.Quantity > product.Quantity)
            {
                TempData["WarnMessage"] = "Not enough stock!";
                return RedirectToAction("ProductsShopping", "Product");
            }
        }
        else
        {
            if (product.Quantity < 1)
            {
                TempData["WarnMessage"] = "Out of stock!";
                return RedirectToAction("ProductsShopping", "Product");
            }

            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            });

        }

        Response.SetObjectAsJson(cartKey, cart);
        TempData["Message"] = $"{product.Name} added to cart!";
        return RedirectToAction("ProductsShopping", "Product");
    }

    public IActionResult Cart()
    {
        string cartKey = GetCartCookieKey();
        var cart = Request.GetObjectFromJson<List<CartItem>>(cartKey) ?? new List<CartItem>();
        decimal cartTotal = cart.Sum(x => x.Price * x.Quantity);
        ViewBag.CartTotal = cartTotal;
        return View(cart);
    }

    public IActionResult Remove(int id)
    {
        string cartKey = GetCartCookieKey();
        List<CartItem> cart = Request.GetObjectFromJson<List<CartItem>>(cartKey) ?? new List<CartItem>();
        var itemToRemove = cart.FirstOrDefault(x => x.ProductId == id);

        if (itemToRemove != null)
        {
            cart.Remove(itemToRemove);
            Response.SetObjectAsJson(cartKey, cart);
        }

        return RedirectToAction("Cart");
    }

    public IActionResult ClearCart()
    {
        string cartKey = GetCartCookieKey();
        Response.RemoveCookie(cartKey);
        TempData["Message"] = "Cart cleared!";
        return RedirectToAction("Cart");
    }

    public IActionResult UpdateCart(int id, int quantity)
    {
        if (quantity < 1) quantity = 1;

        string cartKey = GetCartCookieKey();
        List<CartItem> cart = Request.GetObjectFromJson<List<CartItem>>(cartKey) ?? new List<CartItem>();
        var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

        if (existingItem != null)
        {
            existingItem.Quantity = quantity;
            if (existingItem.Quantity > 10)
            {
                TempData["WarnMessage"] = "Maximum quantity is 10!";
                return RedirectToAction("Cart");
            }
        }

        Response.SetObjectAsJson(cartKey, cart);
        return RedirectToAction("Cart");
    }
}
