using ECommerceMVC.Services;
using ECommerceMVC.Data;
using ECommerceMVC.Models;
using Microsoft.AspNetCore.Identity;
using ECommerceMVC.ViewModel;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
            _context = context;
    }

    public void CreateOrder(Guid userId, List<OrderCreate> orderItems)
    {
        //create our order
        var newOrder = new Order()
        {
            UserId = userId,
            Date = DateTime.Now
        };

        _context.Order.Add(newOrder);
        _context.SaveChanges();
        //for each product, create an order item
        foreach (var order in orderItems) {
            var productItem = new OrderItem
            {
                OrderId = newOrder.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };
            _context.OrderItem.Add(productItem);
        }
        _context.SaveChanges();
    }

    public IEnumerable<IGrouping<int, OrderDetails>> GetOrderDetails(Guid userId)
    {
        var orders = _context.Order
        .Join(_context.OrderItem, 
            o => o.Id,
            oi => oi.OrderId,
            (o, oi) => new { Order = o, OrderItem = oi})
        .Join(_context.Product,
            oi => oi.OrderItem.ProductId,
            p => p.Id,
            (oi, p) => new OrderDetails
            {
                Id = oi.Order.Id,
                UserId = oi.Order.UserId,
                OrderDate = oi.Order.Date,
                ProductId = oi.OrderItem.ProductId,
                Quantity = oi.OrderItem.Quantity,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductImageUrl = p.ImageUrl,
                ProductPrice = p.Price
            }
        )
        .Where(order => order.UserId == userId)
        .OrderByDescending(order => order.OrderDate)
        .ToList();

        return orders.GroupBy(item => item.Id);
    }
}