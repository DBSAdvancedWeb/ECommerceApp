using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;
using ECommerceMVC.Services;

namespace ECommerceMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DealsService _dealService;
    private readonly ICartSessionService _cartService;

    public HomeController(ILogger<HomeController> logger, DealsService dealsService, ICartSessionService cartService)
    {
        _logger = logger;
        _dealService = dealsService;
        _cartService = cartService;
    }
    public IActionResult Index()
    {
        IEnumerable<Product> Products = _dealService.GetListOfProducts();
        return View(Products);
    }

    public IActionResult Products(){
        var categories = _dealService.GetProductCategories();
        return View(categories);
    }

    public IActionResult Cart(){
        IEnumerable<Product> shoppingList = _cartService.GetCart();
        return View(shoppingList);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
