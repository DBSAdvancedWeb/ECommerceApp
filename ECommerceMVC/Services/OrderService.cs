using ECommerceMVC.Services;
using ECommerceMVC.Data;
using System.Security.Claims;
using ECommerceMVC.Models;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
            _context = context;
    }
    public void CreateOrder(Guid userId, List<int> productIds)
    {
        //create our order
        var order = new Order()
        {
            UserId = userId,
            Date = DateTime.Now
        };

        _context.Order.Add(order);
        _context.SaveChanges();
        //for each product, create an order item
        foreach (var productId in productIds) {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = productId,
                Quantity = 1
            };
            _context.OrderItem.Add(orderItem);
        }
        _context.SaveChanges();
    }
}