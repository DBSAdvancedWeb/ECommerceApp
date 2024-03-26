using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;
using ECommerceMVC.Services;


namespace ECommerceMVC.Controllers;


[ApiController]
[Route("/shoppingcart")]
public class CartController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICartSessionService _cartSessionService;

    public CartController(ILogger<HomeController> logger, ICartSessionService cartSessionService) {
        _logger = logger;
        _cartSessionService = cartSessionService;
    }

    [HttpGet(Name = "GetCart")]
    public List<Product> Get() 
    {
        List<Product> shoppingCart = _cartSessionService.GetCart();
        return shoppingCart;
    }

    [HttpPost(Name = "AddToCart")]
    public List<Product> Add(Product product)
    {
        _cartSessionService.AddToCart(product);
        return _cartSessionService.GetCart();
    }

}