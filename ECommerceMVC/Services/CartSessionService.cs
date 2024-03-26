using ECommerceMVC.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ECommerceMVC.Services;


public class CartSessionService : ICartSessionService
{

    private const string SHOPPING_CART = "Cart";
    private readonly IHttpContextAccessor _ctxSession;
    
    public CartSessionService(IHttpContextAccessor ctxSession) {
        _ctxSession = ctxSession;
    }

    public List<Product> GetCart() 
    {
        string cartJson = _ctxSession?.HttpContext?.Session.GetString(SHOPPING_CART) ?? string.Empty;    
        List<Product> cartItems = JsonConvert.DeserializeObject<List<Product>>(cartJson) ?? new List<Product>();
        return cartItems;        
    }
    
    public void AddToCart(Product product)
    {
        string cartJson = _ctxSession?.HttpContext?.Session.GetString(SHOPPING_CART) ?? string.Empty;
        List<Product> cartItems = JsonConvert.DeserializeObject<List<Product>>(cartJson) ?? new List<Product>(); 
        cartItems.Add(product);
        
        //serialize it and store in session
        string data = JsonConvert.SerializeObject(cartItems);
        _ctxSession?.HttpContext?.Session.SetString(SHOPPING_CART, data);

    }

    public bool ClearCart()
    {
        throw new NotImplementedException();
    }

    public void RemoveFromCart(string ProductId)
    {
        throw new NotImplementedException();
    }
}