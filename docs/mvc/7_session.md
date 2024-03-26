# Session

In this tutorial we will be looking at session and how we can use it to store information for the lifetime of a session. Our example will build out a shopping cart. 

## Getting Started

1. Start by enabling a session in the Program.cs file. Add the following code:

```c#
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "ECommerceMVC.Cart";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```
2. Next, we need to enable it by adding the app.UseSession() code

```c#
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession(); //Add UseSession!

app.UseRouting();

app.UseAuthorization();
```
3. Next, to test and make sure our session is working, load up the application by running the following:

```c#
dotnet run
 ```
4. If no errors occurred, its configured correctly. 
5. Next, we need start to build out and use our new Cookie session.
6. First, we need to add a new Interface/Service to our DI. Add the following:

```c#
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
```
7. The IHttpContextAccessor is what we will use to both access the cookie and add values to it. 
8. In the Services folder, create a new interface called ICartSessionService.cs and add the following:
```c#
using ECommerceMVC.Models;


namespace ECommerceMVC.Services;

public interface ICartSessionService {
    public void AddToCart(Product product);
    public List<Product> GetCart();
    public void RemoveFromCart(string ProductId);
    public bool ClearCart();
}
```
9. The interface will be used to describe the methods we need to implement in our concrete class. 
10. Create a new Service called CartSessionService and add the following:
```c#
using ECommerceMVC.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ECommerceMVC.Services;


public class CartSessionService : ICartSessionService
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
```
11. The above code implements the ICartSessionService interface and by default it will add the NotImplementedException. It is up to us to add the implementation. 
12. Lets start by adding a constructor and dependencies:
```c#
    private const string SHOPPING_CART = "Cart";
    private readonly IHttpContextAccessor _ctxSession;
    
    public CartSessionService(IHttpContextAccessor ctxSession) {
        _ctxSession = ctxSession;
    }
```
13. The **IHttpContextAccessor** provides helpful methods for getting and setting data via a session cookie but has one drawback. We will be working with JSON so we need to both serialize and deserialize data going back and forth. 
14. Luckily dotnet has a tonne of great libs available for this, one of which is Newtonsoft.Json
15. Check for the latest here - https://www.nuget.org/packages/Newtonsoft.Json/
16. Then run the command in the terminal:

```shell
dotnet add package Newtonsoft.Json --version 13.0.3
```
17. Go back to our service CartSessionService.cs and locate the GetCart method. Add the following code:

```c#
    public List<Product> GetCart() 
    {
        string cartJson = _ctxSession?.HttpContext?.Session.GetString(SHOPPING_CART) ?? string.Empty;
        //Deserialize it    
        List<Product> cartItems = JsonConvert.DeserializeObject<List<Product>>(cartJson) ?? new List<Product>();
        return cartItems;        
    }
```
18. This uses the HttpContext to get access to the session and the cart item called SHOPPING_CART that we declare as a constant above. 
19. Next, lets implement the **AddCart** method:
```c#
    public void AddToCart(Product product)
    {
        string cartJson = _ctxSession?.HttpContext?.Session.GetString(SHOPPING_CART) ?? string.Empty;
        List<Product> cartItems = JsonConvert.DeserializeObject<List<Product>>(cartJson) ?? new List<Product>(); 
        cartItems.Add(product);
        
        //serialize it and store in session
        string data = JsonConvert.SerializeObject(cartItems);
        _ctxSession?.HttpContext?.Session.SetString(SHOPPING_CART, data);

    }
```

## Cart Controller

1. Our service is setup for both adding and getting the session values but we need to access it via our app
2. Create a new Controller called CartController
```c#
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;
using ECommerceMVC.Services;


namespace ECommerceMVC.Controllers;


[ApiController]
[Route("/shoppingcart")]
public class CartController : ControllerBase
{
}
```
3. One thing to notice, we are not using the **Controller** base class. 
4. Instead, we will use the Web API of **ControllerBase**
5. Next, add the following dependencies:
```c#
    private readonly ILogger<HomeController> _logger;
    private readonly ICartSessionService _cartSessionService;

    public CartController(ILogger<HomeController> logger, ICartSessionService cartSessionService) {
        _logger = logger;
        _cartSessionService = cartSessionService;
    }
```
6. We will be using our newly created ICartSessionService to allow access to our session
7. Add the following two methods:

```c#
    [HttpGet(Name = "GetCart")]
    public List<Product> Get() 
    {
        List<Product> shoppingCart = _cartSessionService.GetCart();
        return shoppingCart;
    }

    [HttpPost(Name = "AddToCart")]
    public List<Product> Add(Product product)
    {
        _cartSessionService.AddToCart(product);
        return _cartSessionService.GetCart();
    }
```
8. Load up the app and go to the following url: http://localhost:&lt;PORT&gt;/shoppingcart
9. You will most likely get an error, we can fix this by going to our Program.cs file and adding in the required DI
10. Open the Program.cs file and add the following:

```c#
builder.Services.AddScoped<ICartSessionService, CartSessionService>();
```
11. Again, restart the app and go to the http://localhost:&lt;PORT&gt;/shoppingcart and you should see an empty **[]** being returned

## Client side

We want to be able to add items to the cart from the UI. Instead of using the MVC flow we will implement it in the client side using javascript with jQuery.

1. Go to the Views/Home/Products view page 
2. We are going to add a new button to the product page and also append the product data as json to the new button link:
```c#
                    <div class="col-md-4">
                        <div class="card mb-3">
                            <img src="~/@item.ImageUrl" class="card-img-top" alt="@item.Name Image">
                            <div class="card-body">
                                <h5 class="card-title">@item.Name</h5>
                                <p class="card-text">@item.Description</p>
                                    <strong class="text-success">
                                    @if (@item.Price < 50)
                                    {
                                        <i class="bi bi-fire text-danger"></i>    
                                    }
                                    € @item.Price
                                    </strong>
                                    //adding a new button here
                                    <a href="#" data-product="@Json.Serialize(item).ToString()" class="cart-btn btn btn-outline-success btn-sm float-end">
                                        <i class="bi bi-cart-plus"></i> Add 
                                    </a>
                            </div>
                        </div>
                    </div>   
```
3. Load up the application and you should now see an Add button with a shopping cart icon inside it. 
4. Next, we need a way to call our new CartController from the UI - AJAX allows just this
5. In the wwwroot/js folder create a new script called **shoppingcart.js**
6. Add the following code:
```c#
$(document).ready(function() {

   $('.cart-btn').on('click', function(e) {
        e.preventDefault();
        let product = $(this).data('product');

        console.log(product.name);

        $.ajax({
            type: "POST",
            url: "/shoppingcart",
            data: JSON.stringify(product),
            dataType: 'json',
            contentType: "application/json",
            success: function(response) {
                // Handle success response
                console.log("Data successfully sent to /shoppingcart:", response);
            },
            error: function(xhr, status, error) {
                // Handle error response
                console.error("Error:", error);
            }
        });
   });

});
```
7. The above code waits for the DOM to be ready and then assigns an event handler to the click event of the button we just added. 
8. When clicked, it will post the data down to the server side controller of CartController
9. Lastly, we need to add the script to the page for it to be loaded.
10. Go to the Views/Shared/_Layout.cshtml and at the bottom of the page add our new script:
```c#
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    //add our new shoppingcart script!
    <script src="~/js/shoppingcart.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
```
11. Load up the application, login and then go to the /Home/Products page. 
12. Click on the Add button for a few products and then go to the /shoppingcart in the URL
13. If successful, you should see a json list like below:
```json
[
    {
    "id": 1,
    "name": "Fitness Trackers",
    "description": "Aliexpress fintness trackers",
    "category": "Jewellery",
    "imageUrl": "imgs/aliexpress-fitness-trackers.jpg",
    "price": 89.99,
    "dateAdded": "2024-03-19T16:25:14.4918278"
    },
    {
    "id": 7,
    "name": "Gemstone Necklace",
    "description": "Purple Gemstone necklace",
    "category": "Jewellery",
    "imageUrl": "imgs/purple-gemstone-necklace.jpg",
    "price": 19.95,
    "dateAdded": "2024-03-19T16:25:14.4935618"
    },
    {
    "id": 8,
    "name": "Modern Watch",
    "description": "Modern time piece watch",
    "category": "Jewellery",
    "imageUrl": "imgs/modern-time-pieces.jpg",
    "price": 45,
    "dateAdded": "2024-03-19T16:25:14.493562"
    },
    {
    "id": 2,
    "name": "Black Bag",
    "description": "Black over the shoulder bag",
    "category": "Bags",
    "imageUrl": "imgs/black-bag-over-the-shoulder.jpg",
    "price": 59.95,
    "dateAdded": "2024-03-19T16:25:14.4935589"
    }
]
```


## Tasks

Implement the following:

1. Create a Cart View and display all the contents of the Cart
2. Implement the remaining two methods of the ICartSessionServices interface
    * RemoveFromCart
    * ClearCart
3. Create a Cart Icon that appears on the top right of the screen
    * Should be visible 
    * Should show the number of items in the cart
4. Improve the Shopping cart by allowing a user to add Quantity when buying?


