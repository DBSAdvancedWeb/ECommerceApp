# Entity Relationship Tasks

## Tasks
1. Create a new View to display all Orders completed by the current user.
2. Allow a user to order more than 1 Quantity of each product.  
3. The OrderService works under the assumtion that placing an order will work. What happens if something goes wrong. How can we make the service better? 

## Orders View

1. Before starting anything we first need to break down our problem. The requirement is to display all order by the current user. We can first run a query in the database to confirm the data that we need. 
2. Open up the SQL Editor window and create a SQL statement to pull back details of the Order, OrderItem & Product information. 
```sql
SELECT 
    "o"."Id", 
    "o"."UserId", 
    "o"."Date" AS "OrderDate", 
    "o0"."ProductId", 
    "o0"."Quantity", 
    "p"."Name" AS "ProductName", 
    "p"."Description" AS "ProductDescription", 
    "p"."ImageUrl" AS "ProductImageUrl", 
    "p"."Price" AS "ProductPrice"
FROM "Order" AS "o"
INNER JOIN "OrderItem" AS "o0" ON "o"."Id" = "o0"."OrderId"
INNER JOIN "Product" AS "p" ON "o0"."ProductId" = "p"."Id"
```
3. This will provide information about the order and what items in terms of products were bought. We will also need to include a WHERE clause to make it filter by the current user e.g WHERE o.UserId = USER_ID
4. We have one other issue and that is we need to group the data based on the order ID. For example, if a user orders 3 products and two products another day, we need to make sure the order details shows this as two seperate orders. 
5. Language Integrated Query (LINQ) is a set of language extensions to C# that enables querying data from various sources. It allows developers to write queries directly in their C# code. 
6. Open up the IOrderService.cs file and add a new method called GetOrderDetails like below:
```c#
using ECommerceMVC.ViewModel;

namespace ECommerceMVC.Services;

public interface IOrderService
{
    public void CreateOrder(Guid userId, List<int> productIds);
    public IEnumerable<IGrouping<int, OrderDetails>> GetOrderDetails(Guid userId);
}
``` 
7. Notice that we are returning an IEnumerable IGrouping of int and OrderDetails. The int in this case will represent the OrderId.
8. OrderDetails has not been created yet, but this is something we need to change. Create a new folder called ViewModel and inside that create the new file of OrderDetails.cs and add the following:
```c#
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
```
9. In the context of ASP.NET MVC (Model-View-Controller), a ViewModel (not ModelView) is a pattern used to prepare data for presentation to the user interface. It serves as an intermediary between the controller and the view. This will allow us to map data from our three tables of Product, Order & OrderItem.  
10. Next, open up the OrderService.cs class and lets implement the GetOrderDetails method. 
```c#
    //Remeber to include reference to our new ViewModel!
    using ECommerceMVC.ViewModel;
    
    ...

    public IEnumerable<IGrouping<int, OrderDetails>> GetOrderDetails(Guid userId)
    {
        var orders = _context.Order
        .Join(_context.OrderItem, 
            o => o.Id,
            oi => oi.OrderId,
            (o, oi) => new { Order = o, OrderItem = oi})
        .Join(_context.Product,
            oi => oi.OrderItem.ProductId,
            p => p.Id,
            (oi, p) => new OrderDetails
            {
                Id = oi.Order.Id,
                UserId = oi.Order.UserId,
                OrderDate = oi.Order.Date,
                ProductId = oi.OrderItem.ProductId,
                Quantity = oi.OrderItem.Quantity,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductImageUrl = p.ImageUrl,
                ProductPrice = p.Price
            }
        ).Where(order => order.UserId == userId).ToList();

        return orders.GroupBy(item => item.Id);
    }
```
11. As you can see we are essentially creating the SQL statement via LINQ to query the Order table and then join both the OrderItem and Product tables. The output is created by creating a new OrderDetails class. The Where clause is used to filter by the current user and the final return statement uses he GroupBy to aggregate based on the Order Id.
12. We now need to call this method via our OrderController. Open it up and create a new method called Orders:
```c#
    //add reference for the OrderDetails from the ViewModel namespace
    ...
    using ECommerceMVC.ViewModel;

    public IActionResult Orders() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null) {
          return RedirectToAction("Products", "Home");  
        }

        IEnumerable<IGrouping<int, OrderDetails>> orders = _orderService.GetOrderDetails(Guid.Parse(userId));
        return View(orders);
    }
    ...
``` 
13. In the code above, we check for the current logged in user and get the Guid and then pass onto our new OrderService method of GetOrderDetails. The return is the View(orders) which now also needs to be created.  
14. In the Views/Order folder, create a new View called Orders.cshtml and add the following:
```c#
@model IEnumerable<IGrouping<int, OrderDetails>>;
@{
    ViewData["Title"] = "Your Orders";
}

<h2>Your Orders</h2>

@foreach (var order in Model)
{
<div class="card">
  <div class="card-header d-flex w-100 justify-content-between">
    <span>Ordered On: @order.First().OrderDate</span>
    <span>Order # @order.Key</span>
  </div>
    <div class="list-group list-group-flush">
        @foreach (var item in order)
        {
            <a href="#" class="list-group-item list-group-item-action flex-column align-items-start">
                <div class="d-flex w-100 justify-content-between">
                    <h5 class="mb-1">@item.ProductName</h5>
                    <small>€ @item.ProductPrice</small>
                </div>
                <p class="mb-1">@item.ProductDescription</p>
                <small>Quantity: @item.Quantity</small>
            </a>
        }
    </div>
  <div class="card-footer d-flex w-100 justify-content-between">
    <strong>Total:</strong> 
    <strong>€ @order.Sum(od => od.ProductPrice)</strong>
  </div> 
</div>
<br />
<br />
} 
```
15. Try and build the project by running dotnet build. You may notice you get a load of errors showing that **OrderDetails cannot be found**. Let us fix this. 
16. In the Views/Shared folder, open the _ViewImports.cshtml and observe the code:
```c#
@using ECommerceMVC
@using ECommerceMVC.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
``` 
17. As you can see there is no reference to the new ViewModel folder we created. Lets fix this by adding our ViewModel to the _ViewImports. Add the following line directly after the EcomereceMVC.Models on line 2.
```c#
@using ECommerceMVC.Models.ViewModel
```
18. Build the app again and you should have no errors. 
19. Run the app by running dotnet run and go to the /Order/Orders page. 
20. You should see a list of **Your Orders**. (Make sure you have at least 2 or 3 orders placed)
21. We have one minor thing to change. The orders are currently presented in an ascending order but we want to see the last orders being on top - as in descending. Open up the OrderService and lets change that:
```c#
        .Where(order => order.UserId == userId)
        .OrderByDescending(order => order.OrderDate)
        .ToList();
``` 
22. By adding the OrderByDescending and applying the order.OrderDate it will now show orders most recent on top. 


## Order More Quantity of a Product

1. We will begin by going to the IOrderService.cs and figure out what we need for allowing users to post data for a product but also its quanity. 
2. The current method accepts a List of ints for productIds. Lets change this by creating a new object as parameter. 
3. Go to the ViewModel folder and create a new class called OrderCreate.cs and add the following:
```c#
namespace ECommerceMVC.ViewModel;

public class OrderCreate
{
    public int ProductId {get; set;}
    public int Quantity {get; set;}
}
```
4. This object will be combined with a List to allow us to send multiple products of different quantities. 
5. Open the IOrderService and update the CreateOrder method to the following:
```c#
    public void CreateOrder(Guid userId, List<OrderCreate> orderItems);
```
6. Next, open the class of OrderService and update the method parameter and also the code itself. 
```c#
    public void CreateOrder(Guid userId, List<OrderCreate> orderItems)
    {
        //create our order
        var newOrder = new Order()
        {
            UserId = userId,
            Date = DateTime.Now
        };

        _context.Order.Add(newOrder);
        _context.SaveChanges();
        //for each product, create an order item
        foreach (var order in orderItems) {
            var productItem = new OrderItem
            {
                OrderId = newOrder.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };
            _context.OrderItem.Add(productItem);
        }
        _context.SaveChanges();
    }
```
7. All we have changed here is the data that is being passed in from the orderItems list. It now will contain the Quantity for each Product. Read through it so it makes sense. 
8. Next, lets move to the OrderController and change its method param to accept our new ViewModel of OrderCreate. 
9. Locate the Create method and update the code:
```c#
    [HttpPost]
    public IActionResult Create([FromBody] List<OrderCreate> orderItems)
    {
        if(orderItems.Count == 0) {
            return RedirectToAction("Products", "Home");
        }
        //lets get our user Id!
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(userId !=  null) {
            _orderService.CreateOrder(Guid.Parse(userId), orderItems);
        }

        return RedirectToAction("Confirmation");
    }
```
10. One thing to notice is the FromBody being added. We need this as we are going to POST the data in JSON format via the HTTP Body. Notice also we are using the List of OrderCreate. 
11. Our final area to fix up is the view itself. Open up the Home/Cart View. 
12. We need to make some changes. In order to POST JSON from the UI we will need to use Javascript and AJAX. Add the following changes to the view:
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
      <p class="product-item">
        <input type="hidden" class="productId" value="@product.Id" /> 
        <label for="quantity">Quantity:</label>
        <input class="quantity form-control-sm" type="number" min="1" value="1">
      </p>
    </a>
  }
</div>
<hr />
<div class="d-flex w-100 justify-content-between">
  <h5 class="mb-1">Total:</h5>
  <strong>€ @Model.Sum(item => item.Price)</strong>
</div>
<br />
<div class="d-flex w-100 justify-content-between">
  <a class="btn">Continue Shopping</a>
  <button id="submitOrder" class="btn btn-success">Purchase</button>
</div>
```
13. The code above removes the form and adds a new set of classes for css and also the new quantity input to allow a user to choose the ammount of products they want. 
14. The final step is to add a new javascript file called createorder.js. Open up the wwwroot/js folder and create the new file and add the following:
```js
$(document).ready(function () {
    
    $('#submitOrder').click(function (e) {
        e.preventDefault();
        // create an order array
        let orderData = [];
        // Iterate through each selected product
        $('.product-item').each(function () {
            var productId = $(this).find('.productId').val();
            var quantity = $(this).find('.quantity').val();
            orderData.push({ 
                productId: parseInt(productId), 
                quantity: parseInt(quantity) 
            });
        });

        $.ajax({
            type: 'POST',
            url: '/Order/Create',
            contentType: 'application/json',
            data: JSON.stringify(orderData),
            success: function (response) {
                window.location.href = '/Order/Confirmation';
            },
            error: function (xhr, status, error) {
                // Handle error
            }
        });
    });

});
```
15. The script above will add an event handler to the submit button and then collect all the data wrapped by the product-item class. Each input such as the product id and quantity is added into a JSON Object like below:
```json
[
    {
        "productId": 3,
        "quantity": 3
    },
    {
        "productId": 4,
        "quantity": 3
    }
]
```
16. Load up the application by running dotnet run and verify that the purschase works for an order. 
17. You should now see that the Order you have place will store the correct quantity for any given product, but wait. Take a look at the Price values and the total price. It is not calculating the correct price * quantity and the total. Lets fix this. 
18. Go to the Products page and locate and Genstone Necklace and click the Add to cart button. 
19. Next click on the shopping cart which will then take us to the Cart.cshtml page. On this page, change the quantity to 3. Notice that the  Price and Total do not change. It should show the Price as €59.85. 
20. Look at the code on the Cart.cshtml and locate where the price and total are rendered:
```html
//Product Price
      <div class="d-flex w-100 justify-content-between">
        <h5 class="mb-1">@product.Name</h5>
        <small>€ @product.Price</small>
      </div>

//Total Price
<div class="d-flex w-100 justify-content-between">
  <h5 class="mb-1">Total:</h5>
  <strong>€ @Model.Sum(item => item.Price)</strong>
</div>      
```
21. We need to make some adjustments to the markup to allow for us to show the Unit and total price based on the quantity. Update the markup inside the for loop to have the following:
```html
    <a href="#" class="list-group-item list-group-item-action flex-column align-items-start">
      <div class="d-flex w-100 justify-content-between">
        <h5 class="mb-1">@product.Name</h5>
        <small>
            <span>Each: €</span><span class="unit-price">@product.Price</span>
            <br />
            <span><span>€</span><em class="unit-quantity">@product.Price</em></span>
        </small>
      </div>
      <p class="mb-1">@product.Description</p>
      <p class="product-item">
        <input type="hidden" class="productId" value="@product.Id" /> 
        <label for="quantity">Quantity:</label>
        <input class="quantity form-control-sm" type="number" min="1" value="1">
      </p>
    </a>
```
22. Also, update the markup for the total price:
```html
<div class="d-flex w-100 justify-content-between">
  <h5 class="mb-1">Total:</h5>
  <strong>€ <span id="totalpurchase">@Model.Sum(item => item.Price)</span></strong>
</div>
```
23. Open up the wwwroot/js folder and locate the shoppingcart.js file. Inside the document read function add a new event handler to list for change across all the Quantity input fields:
```javascript
   $('.quantity').on('change', function(e) {
        
        let $this = $(e.target);    
        let quantity = $($this).val();
        let unitPrice = parseFloat($($this).parent().parent().find('.unit-price').text());
        let unitByQuantity = $($this).parent().parent().find('.unit-quantity');
        
        let total = unitPrice * quantity;
        let formattedPrice = total.toFixed(2);
        unitByQuantity.text(formattedPrice);
        
        //calculate total
        let totalPrice = $('#totalpurchase');
        let totalCost = 0;
    
        $('.unit-quantity').each(function() {
            totalCost += parseFloat($(this).text());
        });
        
        totalPrice.text(totalCost.toFixed(2));
   });
```
24. The above code adds event handlers for all quanity inputs and upon change it will then update the price for that product and also the total price. 
25. Load the app and test the results. 