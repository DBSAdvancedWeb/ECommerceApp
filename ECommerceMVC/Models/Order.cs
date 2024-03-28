namespace ECommerceMVC.Models;

public class Order
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
}