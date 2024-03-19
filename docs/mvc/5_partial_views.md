# Partial Views

We have already created some views and mapped them to the controller. Let's look at creating some partial views and how common items can be reused in our application. 

Our Product Category page lists all products based on category but it will also be good to show this in a left hand navigation menu. 

## Getting Started

1. Open the Views/Shared folder and create a new partial called _LeftHandNav.cshtml 
2. Next, we want to create a menu. We can use the List Group component from bootstrap
3. Add the following code snippet:
```c#
@model IEnumerable<IGrouping<string, ECommerceMVC.Models.Product>>

<ul class="list-group"> 
@foreach(var product in Model)
{
  <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center">
    @product.Key
    <span class="badge bg-success">@product.Count()</span>
  </li>
}
</ul>
```
4. The above code looks for an IEnumerable grouping of the products. We can use this to print out both the category name and its count. 
5. Next, go to the Products.cshtml view
6. We want to update the markup to allow for the left hand navigation bar:
```html
<div class="row">
    <div class="col-2">
        <h3 class="display-4">Categories</h3>
    </div>
    <div class="col-10">
        <h1 class="display-4">Product Categories</h1>
    </div>
</div>

<div class="row">
    <div class="col-2">

    </div>
    <div class="col-10">
        
    </div>
</div>
```
7. Next, we want to add in our parital. In the second div class of row add the following:
```html
<div class="row">
    <div class="col-2">
        <h3 class="display-4">Categories</h3>
    </div>
    <div class="col-10">
        <h1 class="display-4">Product Categories</h1>
    </div>
</div>

<div class="row">
    <div class="col-2">
        <partial name="_LeftHandNav" />
    </div>
    <div class="col-10">
        
    </div>
</div>
```
8. Lastly, add in the for loop which prints out the categories:
```html
<div class="row">
    <div class="col-2">

    </div>
    <div class="col-10">
        <h1 class="display-6">Product Categories</h1>
    </div>
</div>

<div class="row">
    <div class="col-2">
        <partial name="_LeftHandNav" />
    </div>
    <div class="col-10">

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
    </div>
</div>
```
9. Render the page in the browser and you should see a navigation menu with badge counts in green along side the categories of products. 

## Related Articles

Partial Views: https://learn.microsoft.com/en-us/aspnet/core/mvc/views/partial?view=aspnetcore-7.0