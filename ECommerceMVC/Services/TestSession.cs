using ECommerceMVC.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ECommerceMVC.Services;


public class TestSession : ICartSessionService
{
    public void AddToCart(Product product)
    {
        throw new NotImplementedException();
    }

    public bool ClearCart()
    {
        throw new NotImplementedException();
    }

    public List<Product> GetCart()
    {
        throw new NotImplementedException();
    }

    public void RemoveFromCart(string ProductId)
    {
        throw new NotImplementedException();
    }
}