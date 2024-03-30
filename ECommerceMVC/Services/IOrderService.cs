using ECommerceMVC.ViewModel;

namespace ECommerceMVC.Services;
public interface IOrderService
{
    public void CreateOrder(Guid userId, List<OrderCreate> orderItems);
    public IEnumerable<IGrouping<int, OrderDetails>> GetOrderDetails(Guid userId);
}