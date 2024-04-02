# Entity Relationships

So far our database is pretty simple. We only have a Products table that handles the retreival and storage of product data. Given that we have now included a shopping cart, we should also have the ability to place an order. 

## Getting Started

1. We will start by creating an Order model. Open the Models folder and create a new Order.cs file and add the following code:
```c#
namespace ECommerceMVC.Models;

public class Order
{
    public int Id { get; set; }
    public Guid UserId {get; set;}
    public DateTime Date { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
```
2. Next, we will create an OrderItem model. Again, in the Models folder create a new file called OrderItem.cs and add the following code:
```c#
namespace ECommerceMVC.Models;

public class OrderItem
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }

    public Product Product { get; set; }
    public Order Order { get; set; }
}
```
3. Next, we now need to make a relationship between our Product and OrderItem tables. Open the Product.cs file and add a new attribute:
```c#
public ICollection<OrderItem>? OrderItems { get; set; }
```
4. Go to the Data/ApplicationDbContext file and add the following directly under the DbSet&lt;Product$gt;
```c#
    public DbSet<Order> Order { get; set; } = default!;
    public DbSet<OrderItem> OrderItem { get; set; } = default!;
```
5. Lastly, we need to add a new override to setup our relationships. Directly under the 3 DbSet<> declarations add the following: 
```c#

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //we use a composite key here of OrderId and ProductId
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.ProductId });

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);
    } 
```
6. We are now good to create our new migration and then apply it to our database. Open the terminal and run the following:
```shell
    dotnet ef migrations add Orders
```
7. Check and verify the migration files have content and if so, run the update command:
```shell
dotnet ef database update
```
8. Open your database and confirm the new tables of Order and OrderItem are present. 

## Order Service

1. We can now create our Order Service. In the Services folder create a new interface called IOrderService with one method:
```c#
namespace ECommerceMVC.Services;
public interface IOrderService
{
    public void CreateOrder(Guid userId, List<int> productIds);
}
```
2. Next, create a new class called OrderService which implements the IOrderService:
3. The OrderService will need to get access to the database context so include the DI for it:
```c#
using ECommerceMVC.Services;
using ECommerceMVC.Data;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
            _context = context;
    }
    public void CreateOrder(List<int> productIds)
    {
        throw new NotImplementedException();
    }
}
```
4. Next, lets work on the implementation for the CreateOrder method. Remove the Exception code and add the following:
```c#
    public void CreateOrder(Guid userId, List<int> productIds)
    {
        //create order
        var order = new Order()
        {
            UserId = userId,
            Date = DateTime.Now
        };

        _context.Order.Add(order);
        _context.SaveChanges();
        //for each product, create an order item
        foreach (var productId in productIds) {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = productId,
                Quantity = 1
            };
            _context.OrderItem.Add(orderItem);
        }
        _context.SaveChanges();
    }
```
5. The above code will create a single Order along with OrderItems. The number of order items will depend on the amount of product ids being passed.
6. Finally, add our new Service to the dependecy injection via Program.cs:
```c#
builder.Services.AddScoped<IOrderService, OrderService>();
``` 

## Order Controller

1. In the Controllers folder create a new Controller called OrdersController.cs
2. Add the following code:
```c#
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerceMVC.Services;


namespace ECommerceMVC.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }
}
```
3. Next, we need to create a new method to handle a POST request from the UI. Add the following code:
```c#
    [HttpPost]
    public IActionResult Create(List<int> productIds)
    {
        if(productIds == null) {
            return RedirectToAction("Products", "Home");
        }
        //lets get our user Id!
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(userId !=  null) {
            _orderService.CreateOrder(Guid.Parse(userId), productIds);
        }

        return RedirectToAction("Confirmation");
    }
```
4. We are introducing something new here. The User.FindFirstValue is a way to get the current logged in user in session. This is exposed via the Controller base class. 
5. The code above will parse this string into a Guid which is a UUID type and then pass it down to our service to store the Order and OrderItems. 

## View

1. From the previous lesson you were asked to create a shopping cart view which would show all the items in a users cart. If you have this created already you can reuse what you have otherwise you can use the code below. 
2. In the View/Home folder create a new file called Cart.cshtml and add the following code:

```c#
@model IEnumerable<ECommerceMVC.Models.Product>;
@{
    ViewData["Title"] = "Shopping Cart";
}
<h1>@ViewData["Title"]</h1>

<p>Select your products you wish to purchase</p>

<div class="list-group">
  @foreach (var product in Model)
  {
      
    <a href="#" class="list-group-item list-group-item-action flex-column align-items-start">
      <div class="d-flex w-100 justify-content-between">
        <h5 class="mb-1">@product.Name</h5>
        <small>€ @product.Price</small>
      </div>
      <p class="mb-1">@product.Description</p>
      <small>Quantity: 1</small>
    </a>
  }
</div>

<hr />
@using (Html.BeginForm("Create", "Order", FormMethod.Post))  
{
  <div>
      @foreach (var product in Model)
      {
        <input type="hidden" name="productIds" value="@product.Id" />    
      }  
  </div>
  <div class="d-flex w-100 justify-content-between">
      <h5 class="mb-1">Total:</h5>
      <strong>€ @Model.Sum(item => item.Price)</strong>
  </div>
  <br />
  <div class="d-flex w-100 justify-content-between">
      <a class="btn">Continue Shopping</a>
      <button type="submit" class="btn btn-success">Purchase</button>
  </div>
}
```
3. The code above does a few things in that it renders what a user has selected via thier shopping cart (session).
4. We need also to direct the user to this view. In the HomeController, add the following method:
```c#
    public IActionResult Cart(){
        IEnumerable<Product> shoppingList = _cartService.GetCart();
        return View(shoppingList);
    }
```
5. Next, update our DI and Constructor to have the following:
```c#
    private readonly ILogger<HomeController> _logger;
    private readonly DealsService _dealService;
    private readonly ICartSessionService _cartService;

    public HomeController(ILogger<HomeController> logger, DealsService dealsService, ICartSessionService cartService)
    {
        _logger = logger;
        _dealService = dealsService;
        _cartService = cartService;
    }
```
6. The code above allows the HomeController to get access to the Cart Session data. 
7. Lastly, we need to allow a user to get to their cart page in order to make a purchase. 
8. In the Shared/_LoginPartial.cshml file, we want to add a shopping cart icon that will appear only if a user is logged in. Go to the file and add the following: (Make sure its wrapped in the if conditon of SignInManager.IsSignedIn(User))
```c#
    <li>
        <a  class="nav-link text-dark" asp-controller="Home" asp-action="Cart" title="Shopping Cart">
            <i class="bi bi-cart-plus"></i>
        </a>
    </li>
```
9. Start the application, login, Go go the Products page, add some items to the cart then click on the cart. 
10. If successful you should see all the items you added.

## Order Confirmation

1. We have one final thing to complete. On the cart page there is a Purchase button. This is connected to our new OrderController to accept our Order and place it in the database. 
2. In the Views folder, create a new folder called Order and inside that create a new file called Confirmation.cshtml.
3. Add the following code snippet:
```c#
@{
    ViewData["Title"] = "Order Confirmation";
}

<h1>Nice! An order has been placed for you!</h1>
```
4. Restart the app and login, add some items to your cart and then click on cart icon, and finally the Purchase button. 
5. You should then see the message **"Nice! An order has been placed for you!"** on the screen. 
6. Lastly, open up the database in the IDE and observe the data. 


## Tasks

1. Create a new View to display all Orders completed by the current user.
2. Allow a user to order more than 1 Quantity of each product.  
3. The OrderService works under the assumtion that placing an order will work. What happens if something goes wrong. How can we make the service better? 


