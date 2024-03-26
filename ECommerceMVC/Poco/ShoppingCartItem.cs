namespace ECommerceMVC.Poco;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}