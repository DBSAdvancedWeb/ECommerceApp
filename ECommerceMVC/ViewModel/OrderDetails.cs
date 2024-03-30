namespace ECommerceMVC.ViewModel;
public class OrderDetails
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string? ProductImageUrl { get; set; }
    public double? ProductPrice { get; set; }
}