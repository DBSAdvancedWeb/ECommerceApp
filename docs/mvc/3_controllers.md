# Controllers

In this tutorial we are going to create a new controller called MyController. We will look at how the MVC Framework maps a Controller to a View and how Models can then be used for getting data and rendering it into our views. In ASP.NET Core, the Controller base class is a fundamental component of the MVC (Model-View-Controller) architectural pattern. It serves as the base class for controller classes in ASP.NET Core applications. Controllers are responsible for handling incoming HTTP requests, executing the appropriate action methods, and returning responses to the client.

The Controller base class provides various properties and methods that are commonly used in controller classes, including:

1. Action methods: These are methods within the controller that handle specific HTTP requests. They are typically annotated with attributes like [HttpGet], [HttpPost], etc., to specify the HTTP verb they respond to.
2. HttpContext: The HttpContext property provides access to information about the current HTTP request, including request and response objects, user identity, and session state.
3. ViewData and ViewBag: These properties are used to pass data from a controller to a view. They provide a way to share data between the controller and the view.
4. RedirectToAction and RedirectToRoute: These methods are used to redirect the client to a different action method or route. 

## Getting Started

1. In the Controllers folder, create a new file called ExampleController.cs
2. Add the following code:

```c#
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Controllers;


public class ExampleController : Controller
{
    public ExampleController() {}
    
    public string Index()
    {
        return "This is the Index method!";
    }

    public string Other()
    {
        return "This is the Other method!";
    }

}
```
3. The code above is just a standard class that implements the base class of Controller. 
4. Along with the base class we also have a single method called Index() which has a return type of string.
5. Start the application using dotnet run and open a browser. 
6. Go to http:/localhost:<PORT>/Example/Index and http://localhost:<PORT>/Example/Other and notice what happens. 
7. Index methods are also considered default so http://localhost:<PORT>/Example will work also. 


## Passing Params

Controllers can also accept params being passed via the URI to each respective method. Lets run through an example of this:

1. In the ExampleController, update the Index() method to accept two params, one for string and the other for an int:
```c#
    public string Index(string day, int num)
    {
        return $"{day} is the day {num} of the week!";
    }
```
2. In the browser, go to http://localhost:5272/Example?day=Wednesday&num=3 and observe the output.

```shell
Wednesday is the day 3 of the week!
```

3. Both day and num are being passed in the URI as day=Wednesday and num=3. The controller is then able to pick these up map to the correct type.
4. It is also good practice to encode any data that comes from the browser for security reasons:

```c#
    public string Index(string day, int num)
    {
        return HtmlEncoder.Default.Encode($"{day} is the day {num} of the week!");
    }    
```

## Deals Controller

Now that we understand the general mapping of Controllers from a URI perspective, let us now update our code so that the controller passes the deals content to the main landing page. 

1. Open up the HomeController and locate the Index() method. 
2. Notice the return type is of type IActionResult and that the page now returns a View() instead of a string.
3. Create a new file in the Models folder called Products and copy the Product class from the Home/Index.cshtml file. You should end up with something like this:

```c#
namespace ECommerceMVC.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public double? Price {get; set;}
}
```

4. You can also remove the Product code from the Home/Index.cshtml view. 
5. Next, go back to the HomeController and create a new method called GetProductsList(). Notice the using ECommerceMVC.Models at the top also - required for the Product class. 

```c#
    using ECommerceMVC.Models; 
    ...

    private IEnumerable<Product> GetProductsList() {
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
```
6. Go back to the Home/Index and remove the code for the List of Products. You also need to remove the @page annotation. You should only have the GetTodaysDate() left inside the functions

```c#
@functions
{    
    public string GetTodaysDate()
    {
        return DateTime.Now.ToString("dddd");
    }
}

...
```
7. You will also notice that the razor syntax is now highlighting the Products item in red. This is something we need to fix. First, go back to the HomeController and lets update our Index method to pass the data to the view. 
8. Add the following code:
```c#
    public IActionResult Index()
    {
        List<Product> Products = GetProductsList();
        return View(Products);
    }
```
9. Next, go back the Home/Index.cshtml view so we can now try and access the data being passed from the Controller. 
10. To access the data being passed we need to let the view know about the type of objects involved. Update the code to have a @model annotatoin on the top of the page:

```c#
@model IEnumerable<ECommerceMVC.Models.Product>
```
11. Next, update the for loop use the keyword of Model instead of Products:
```c#
<div class="row">
    @foreach (var product in Model)
    {
        <div class="col-md-4">
        ...
```
12. Lastly, run/restart the app and go to the home page. You should see the list of products being displayed. 






