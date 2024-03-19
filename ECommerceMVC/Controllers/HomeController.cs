using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;
using ECommerceMVC.Services;

namespace ECommerceMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DealsService _dealService;

    public HomeController(ILogger<HomeController> logger, DealsService dealsService)
    {
        _logger = logger;
        _dealService = dealsService;
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
