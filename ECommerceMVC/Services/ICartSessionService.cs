using ECommerceMVC.Models;


namespace ECommerceMVC.Services;

public interface ICartSessionService {
    public void AddToCart(Product product);
    public List<Product> GetCart();
    public void RemoveFromCart(string ProductId);
    public bool ClearCart();
}