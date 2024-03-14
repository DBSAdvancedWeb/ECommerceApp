using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;

namespace ECommerceMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> Products = GetProductsList();
        _logger.LogInformation($"Getting list of products: {Products.Count()} in total");
        return View(Products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    private IEnumerable<Product> GetProductsList() {
        return new List<Product>()
        {
            new Product { Id = 1, Name = "Fitness Trackers", Description = "Aliexpress fintness trackers", ImageUrl = "imgs/aliexpress-fitness-trackers.jpg", Price = 89.99 },
            new Product { Id = 2, Name = "Black Bag", Description = "Black over the shoulder bag", ImageUrl = "imgs/black-bag-over-the-shoulder.jpg", Price = 59.95 },
            new Product { Id = 2, Name = "Runners", Description = "Black Sneakers with white sole", ImageUrl = "imgs/black-sneakers-with-white-sole.jpg", Price = 40.00 },
            new Product { Id = 1, Name = "Headphones", Description = "Volume Control Headphones", ImageUrl = "imgs/volume-control-headphones.jpg", Price = 179.99 },
            new Product { Id = 2, Name = "Sport Runners", Description = "All black sports runners", ImageUrl = "imgs/right-foot-all-black-sneaker.jpg", Price = 44.99 },
            new Product { Id = 2, Name = "QR Codes", Description = "QR Codes In Store", ImageUrl = "imgs/qr-codes-in-store.jpg", Price = 2.99 },
            new Product { Id = 1, Name = "Gemstone Necklace", Description = "Purple Gemstone necklace", ImageUrl = "imgs/purple-gemstone-necklace.jpg", Price = 19.95 },
            new Product { Id = 2, Name = "Modern Watch", Description = "Modern time piece watch", ImageUrl = "imgs/modern-time-pieces.jpg", Price = 45.00 },
            new Product { Id = 2, Name = "Leather Jacket", Description = "Kids Leather Jacket", ImageUrl = "imgs/kids-leather-jacket.jpg", Price = 119.99 }
        };
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
