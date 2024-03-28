using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerceMVC.Services;


namespace ECommerceMVC.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost]
    public IActionResult Create(List<int> productIds)
    {
        if(productIds == null) {
            return RedirectToAction("Products", "Home");
        }
        //lets get our user Id!
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(userId !=  null) {
            _orderService.CreateOrder(Guid.Parse(userId), productIds);
        }

        return RedirectToAction("Confirmation");
    }

    public IActionResult Confirmation(){
        return View();
    }

}