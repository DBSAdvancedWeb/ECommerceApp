using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerceMVC.Services;
using ECommerceMVC.ViewModel;


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

    public IActionResult Orders() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null) {
          return RedirectToAction("Products", "Home");  
        }

        IEnumerable<IGrouping<int, OrderDetails>> orders = _orderService.GetOrderDetails(Guid.Parse(userId));
        return View(orders);
    }

    [HttpPost]
    public IActionResult Create([FromBody] List<OrderCreate> orderItems)
    {
        if(orderItems.Count == 0) {
            return RedirectToAction("Products", "Home");
        }
        //lets get our user Id!
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(userId !=  null) {
            _orderService.CreateOrder(Guid.Parse(userId), orderItems);
        }

        return RedirectToAction("Confirmation");
    }

    public IActionResult Confirmation(){
        return View();
    }

}