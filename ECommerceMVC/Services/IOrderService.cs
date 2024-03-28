namespace ECommerceMVC.Services;
public interface IOrderService
{
    public void CreateOrder(Guid userId, List<int> productIds);
}