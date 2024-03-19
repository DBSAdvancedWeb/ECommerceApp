# Services

Having static code in a controller works fine but its not very reusable. If for example we wanted to create new page and have similar data we would need to copy and paste the same code multiple times. This is ineffeciant and not something we want. Our data will eventually come from our DB so lets change this to use a Service class. 

## Service 

1. Create a new folder called Services and inside it create a new class called DealService.cs
2. Copy and paste the following code:

```c#
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;

namespace ECommerceMVC.Services;

public class DealsService
{

    public static IEnumerable<Product> GetListOfProducts(){
        return new List<Product>()
        {
            new Product { Id = 1, Name = "Fitness Trackers", Description = "Aliexpress fintness trackers", ImageUrl = "imgs/aliexpress-fitness-trackers.jpg", Price = 89.99 },
            new Product { Id = 2, Name = "Black Bag", Description = "Black over the shoulder bag", ImageUrl = "imgs/black-bag-over-the-shoulder.jpg", Price = 59.95 },
            new Product { Id = 3, Name = "Runners", Description = "Black Sneakers with white sole", ImageUrl = "imgs/black-sneakers-with-white-sole.jpg", Price = 40.00 },
            new Product { Id = 4, Name = "Headphones", Description = "Volume Control Headphones", ImageUrl = "imgs/volume-control-headphones.jpg", Price = 179.99 },
            new Product { Id = 5, Name = "Sport Runners", Description = "All black sports runners", ImageUrl = "imgs/right-foot-all-black-sneaker.jpg", Price = 44.99 },
            new Product { Id = 6, Name = "QR Codes", Description = "QR Codes In Store", ImageUrl = "imgs/qr-codes-in-store.jpg", Price = 2.99 },
            new Product { Id = 7, Name = "Gemstone Necklace", Description = "Purple Gemstone necklace", ImageUrl = "imgs/purple-gemstone-necklace.jpg", Price = 19.95 },
            new Product { Id = 8, Name = "Modern Watch", Description = "Modern time piece watch", ImageUrl = "imgs/modern-time-pieces.jpg", Price = 45.00 },
            new Product { Id = 9, Name = "Leather Jacket", Description = "Kids Leather Jacket", ImageUrl = "imgs/kids-leather-jacket.jpg", Price = 119.99 }
        };
    }
}
```
3. We have created a static method called GetListOfProducts(). 
4. Next, lets update our Controller to pull in the Service class
5. Add the using directive at the top:
```c#
using ECommerceMVC.Services;
```
6. And update our method to use the new static class
```c#
    public IActionResult Index()
    {
        IEnumerable<Product> Products = DealsService.GetListOfProducts();
        return View(Products);
    }
```
7. Reload the app and go to the home page. The page should work as normal. 


## Product Categories Page

Now that we have our new Service, lets reuse it to provide the data in a different format - categories. 

1. Update the Models/Product.cs class to have an extra attribute called Category
```c#
namespace ECommerceMVC.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category {get; set; }
    public string? ImageUrl { get; set; }
    public double? Price {get; set;}
};
```
2. Go to our service class of DealService and lets add a new method called GetProductCagegories
```c#
    public static IEnumerable<IGrouping<string?, Product>> GetProductCategories(){
        IEnumerable<Product> products = GetListOfProducts();
        if(products ==  null){
            return Enumerable.Empty<IGrouping<string, Product>>();
        }
        var categories = products.GroupBy(item => item.Category);
        return categories;
    } 
```
3. Next, in the same Service class we also need to update the data to use the new Category attribute we added: 
```c#

    public static IEnumerable<Product> GetListOfProducts(){
        return new List<Product>()
        {
            new Product { Id = 1, Name = "Fitness Trackers", Category = "Jewellery", Description = "Aliexpress fintness trackers", ImageUrl = "imgs/aliexpress-fitness-trackers.jpg", Price = 89.99 },
            new Product { Id = 2, Name = "Black Bag", Category = "Bags", Description = "Black over the shoulder bag", ImageUrl = "imgs/black-bag-over-the-shoulder.jpg", Price = 59.95 },
            new Product { Id = 3, Name = "Runners", Category = "Runners", Description = "Black Sneakers with white sole", ImageUrl = "imgs/black-sneakers-with-white-sole.jpg", Price = 40.00 },
            new Product { Id = 4, Name = "Headphones", Category = "Headphones", Description = "Volume Control Headphones", ImageUrl = "imgs/volume-control-headphones.jpg", Price = 179.99 },
            new Product { Id = 5, Name = "Sport Runners", Category = "Runners", Description = "All black sports runners", ImageUrl = "imgs/right-foot-all-black-sneaker.jpg", Price = 44.99 },
            new Product { Id = 6, Name = "QR Codes", Category = "Tech", Description = "QR Codes In Store", ImageUrl = "imgs/qr-codes-in-store.jpg", Price = 2.99 },
            new Product { Id = 7, Name = "Gemstone Necklace", Category = "Jewellery", Description = "Purple Gemstone necklace", ImageUrl = "imgs/purple-gemstone-necklace.jpg", Price = 19.95 },
            new Product { Id = 8, Name = "Modern Watch", Category = "Jewellery", Description = "Modern time piece watch", ImageUrl = "imgs/modern-time-pieces.jpg", Price = 45.00 },
            new Product { Id = 9, Name = "Leather Jacket", Category = "Clothes", Description = "Kids Leather Jacket", ImageUrl = "imgs/kids-leather-jacket.jpg", Price = 119.99 }
        };
    }
```
4. This method gets the same products list but it also then groups the data based on the new categories we just added!
5. Grouping the data will then allow us to iterate over it in our new Products page. 
6. Go to the Views/Home page and add a new view called Products.cshtml
7. Inside the Products.cshtml add the following:
```c#
@model IEnumerable<IGrouping<string, ECommerceMVC.Models.Product>>

<div class="text-center">
    <h1 class="display-4">Product Categories</h1>
</div>

@foreach (var group in Model)
{
    <h3>@group.Key</h3>
    <div class="row">
        @foreach (var item in group)
        {
    <div class="col-md-4">
            <div class="card mb-3">
                <img src="~/@item.ImageUrl" class="card-img-top" alt="@item.Name Image">
                <div class="card-body">
                    <h5 class="card-title">@item.Name</h5>
                    <p class="card-text">@item.Description</p>
                    <div class="text-end">
                        <strong class="text-success">
                        @if (@item.Price < 50)
                        {
                            <i class="bi bi-fire text-danger"></i>    
                        }
                        € @item.Price
                        </strong>
                    </div>
                </div>
            </div>
        </div>   
        }
    </div>
}
```
8. Lastly, open the Views/Shared/_Layout.cshtml page and add a link for the new Products page:
```html
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Products">Products</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                        </li>
                    </ul>
```
9. Reload the app and go to the Products page. You should now see a new Product Categories page!
