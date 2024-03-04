# MVC 2: Introducing Razor

This tutorial guides you through building a Razor component to display product listings, using local images stored in the wwwroot directory.

## Prerequisites:

Basic understanding of C#, ASP.NET Core

## Learning Objectives

* Learn & understand Razor templating
* Use for loops and if statements 

## What is Razor?

Razor is a markup syntax used to embed server-side code (typically C# or VB.NET) directly within web pages. This allows developers to combine the flexibility of HTML with the power of server-side programming, resulting in dynamic and user-interactive web experiences. Razor pages can be described as follows:

* They required the @page directive at the top of the file. 
* The can also optionally have the @functions section 

Razor leverages two primary components:

* __Razor Markup:__ This is the HTML-like syntax used for writing the layout and structure of the web page. It includes elements like &lt;div&gt;, &lt;p&gt;, and &lt;img&gt;, along with Razor syntax elements (@ symbol) for embedding code.

* __Server-Side Code:__ This refers to the C# or VB.NET code embedded within Razor markup using the @ symbol. This code can access server-side resources, perform logic, and generate dynamic content.


__NOTE:__
Blazor, introduced in ASP.NET Core 3.0, is a web framework built on top of .NET that allows developers to create interactive web UIs using C# and Razor syntax.


## Getting Started

1. In the ECommerceMVC directory, locate the Views folder and Open the Home -> Index.cshtml file.
2. The Index.cshtml file is the home page and we want to change this to show todays top deals. 

```html
@{
    ViewData["Title"] = "Home Page";
}

<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <p>Learn about <a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>
</div>
```
3. Remove the ViewData and its @{} tags and replace with @page
4. Directly below the @page tag, add a new line and add @function {}
5. In the header h1 tag change the __Welcome__ to __Deals of the Day__
6. Remove the contents of the paragraph tag &lt;p&gt; below the h1 header;

```html
@page
@functions
{

}
<div class="text-center">
    <h1 class="display-4">Deals of the Day!</h1>
    <p></p>
</div>
```

7. Run the app and view the results:

```shell
dotnet run
```

## Dynamic Date

Websites by their nature are static. MVC Applications like ASP.NET Core allows us to pass data to Views which can be based off functions or code to represent more up to date information. 

1. Lets go back to the Index.cshtml file and update the code to get todays date
2. In the @functions {} section add the following function

```html
@page
@functions
{
    public string GetTodaysDate()
    {
        return DateTime.Now.ToString("dddd");
    }   
}
<div class="text-center">
    <h1 class="display-4">Deals of the Day!</h1>
    <p></p>
</div>
```
3. The code GetTodaysDate returns the text of the day. Using Razor syntax we can now display this in our page:
4. In the &lt;p&gt; add the following:

```html
<p><strong>Today's Deals for @GetTodaysDate()</strong></p>
```
5. Now, instead of running dotnet run, use dotnet watch instead. This will load the site automatically based on changes
6. Check the Home page and you should now see the updated title along with the subtitle of showing the current day.

## Adding Content

Our page is pretty bare at the present so let's change that by adding some products. Open the Index.cshtml page from the Views/Home directory. 

1. Within the @functions we are going to create a Class called Product
2. Add the Produc class directly below the GetTodaysDate method:

```c#
@page
@functions
{    
    public string GetTodaysDate()
    {
        return DateTime.Now.ToString("dddd");
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double? Price {get; set;}
    };
}
```
3. Next, we need to create a list of products based on our Product class
4. Directly under the Product class, Create a new List of Products:

```c#
    private List<Product> Products { get; set; } = new List<Product>()
    {
        new Product { Id = 1, Name = "Fitness Trackers", Description = "Aliexpress fintness trackers", ImageUrl = "imgs/aliexpress-fitness-trackers.jpg", Price = 89.99 },
        new Product { Id = 2, Name = "Black Bag", Description = "Black over the shoulder bag", ImageUrl = "imgs/black-bag-over-the-shoulder.jpg", Price = 59.95 },
        new Product { Id = 2, Name = "Runners", Description = "Black Sneakers with white sole", ImageUrl = "imgs/black-sneakers-with-white-sole.jpg", Price = 40.00 },
        new Product { Id = 1, Name = "Headphones", Description = "Volume Control Headphones", ImageUrl = "imgs/volume-control-headphones.jpg", Price = 179.99 },
        new Product { Id = 2, Name = "Sport Runners", Description = "All black sports runners", ImageUrl = "imgs/right-foot-all-black-sneaker.jpg", Price = 44.99 },
        new Product { Id = 2, Name = "QR Codes", Description = "QR Codes In Store", ImageUrl = "imgs/qr-codes-in-store.jpg", Price = 2.99 },
        new Product { Id = 1, Name = "Gemstone Necklace", Description = "Purple Gemstone necklace", ImageUrl = "imgs/purple-gemstone-necklace.jpg", Price = 19.95 },
        new Product { Id = 2, Name = "Modern Watch", Description = "Modern time piece watch", ImageUrl = "imgs/modern-time-pieces.jpg", Price = 45.00 },
        new Product { Id = 2, Name = "Leather Jacket", Description = "Kids Leather Jacket", ImageUrl = "imgs/kids-leather-jacket.jpg", Price = 119.99 }
    };
```

5. Lastly, we need to render this in our page and make it display all the data from our Products list
6. Directly under the title, add the following code snippet

```html
<div class="row">
    @foreach (var product in Products)
    {
        <div class="col-md-4">
            <div class="card mb-3">
                <img src="@product.ImageUrl" class="card-img-top" alt="@product.Name Image">
                <div class="card-body">
                    <h5 class="card-title">@product.Name</h5>
                    <p class="card-text">@product.Description</p>
                    <div class="text-end">
                       <strong>€ @product.Price</strong>
                    </div>
                </div>
            </div>
        </div>    
    }
</div>
```
7. The above snippet is a bootstrap card. ASP.NET Apps are built using bootstrap for its UI. The component above is the Card component. You will see we are able to iterate over the Product list by using Razor's @foreach syntax.  
8. View the output in your browser. You should see 9 cards in a 3 x 3 formation. 
9. The images used are all stored locally in the wwwroot/imgs folder. 


## Conditionals

1. Lets say we wanted to add a fire icon based on a price if it is under €50 euros. Rayzor synax also has an @if directive to allow us to check values data types
2. First, we need to add the icons. Copy the below URL and then paste it directly under the bootstrap.css link in the Views/Shared/_Layout.cshtml file

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
```
3. Go back to the Views/Home and update the code that renders the products:

```html
<div class="row">
    @foreach (var product in Products)
    {
        <div class="col-md-4">
            <div class="card mb-3">
                <img src="@product.ImageUrl" class="card-img-top" alt="@product.Name Image">
                <div class="card-body">
                    <h5 class="card-title">@product.Name</h5>
                    <p class="card-text">@product.Description</p>
                    <div class="text-end">
                        <strong class="text-success">
                            @if (@product.Price < 50)
                            {
                                <i class="bi bi-fire text-danger"></i>
                            }
                            € @product.Price
                        </strong>
                    </div>
                </div>
            </div>
        </div>    
    }
</div>
```
4. You can see the @if directive that checks the products price to see if it is under 50 euro. 
5. Load the home page in your browser and you should now see the fire icon for all products under €50


### Related Articles

* Razor: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-7.0&tabs=visual-studio-code
* Blazor: https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-7.0
* Views: https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-7.0
* Bootstrap Cards: https://getbootstrap.com/docs/5.0/components/card/

