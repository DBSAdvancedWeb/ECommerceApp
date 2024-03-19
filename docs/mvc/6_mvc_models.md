# Models

So far all of our Product data has been static. Lets change this to use models and database tables. Open up the Product.cs Model and add a new attribute for when the product was added:

```c#
using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category {get; set; }
    public string? ImageUrl { get; set; }
    public double? Price {get; set;}
    
    [DataType(DataType.Date)]
    public DateTime? DateAdded {get; set;}
};
```

## CRUD

CRUD or Create, Read, Update and Delete are core to any web application. They allow for the adding, editing and removing of content. In .Net Core scaffolding can be used to generate pages based on simple POCO  (Plain Old C Objects) such as a model. 

## Getting Started

1. First, we need to install some NuGet packages. Open up the terminal and make sure you are inside the ECommerceMVC folder:
2. Run the following commands:
```shell
dotnet tool uninstall --global dotnet-aspnet-codegenerator 
dotnet tool install --global dotnet-aspnet-codegenerator --version <YOUR VERSION>
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version <YOUR VERSION>
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.17
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.17
dotnet add package Microsoft.EntityFrameworkCore.SQLite --version 7.0.17
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 7.0.12
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.17
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.17
```
3. The above versions can depend on what versions you have installed. You can check what the latest is on the Nuget package website - https://www.nuget.org/ 
4. Next, we need to run a script using the code generator:

```shell
dotnet aspnet-codegenerator controller -name ProductsController -m Product -dc ECommerceMVC.Data.ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
```
5. A few things have happened here. The cli generates a few files:
* ProductsController.cs file
* Views for Products - Create, Edit, Details, Delete and Index. 
* Updates the ApplicationDbContext file to include the Products context

6. Take a look athe above files in the IDE to see what was generated.
7. Next, load the application by running **dotnet run**
8. As we now have a Products Controller, the URL of /Products should render out. 
9. In the browser go to /Products, you may see an error!
10. So what happened? Our page threw an error as the ProductsController is looking to get data from the Database Context. Let's fix this:
11. Open the terminal and add the following:
```shell
dotnet ef migrations add ProductsSchema
```
12. This generates migration scripts inside the Data Migrations folder. Open it up and observe the two files:
13. The _ProductsSchema.cs file creates code to create a database table using Entity Framework. It has two main methods of Up and Down. 
```c#
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
```
14. Our next step is to apply these migrations to the database. Open the terminal once more and add type the following:
```shell
dotnet ef database update
```
15. This applys the migration script and will in turn create the Products table. 
16. Reload the application and open it in the browser. 
```shell
dotnet run
```
17. Go to the /Products page and you should see a Page with Index as its title followed by a list with empty results. 
18. Using scaffolding allows us to create the CRUD operations, lets test this out to confirm its working as expected.
19. On the /Products page, locate and click the **Create New** link. 
20. This brings you to a new page of /Products/Create which contains a form for entering in a product. 
21. Enter in the following information and then click on **Create**
```shell
    Name = "Fitness Trackers"
    Category = "Jewellery"
    Description = "Aliexpress fintness trackers"
    ImageUrl = "imgs/aliexpress-fitness-trackers.jpg"
    Price = 89.99 
``` 
22. You should now be redirected back to the /Product page and the new Product will be displayed in the list.
23. We can also test the update functionality - click on the Edit link on the right side of the product listing. 
24. You should now be on the /Products/Edit/1 page.
25. Update the Cateory to Sports Wearables instead of Jewellery and then click the **Save** button.
26. The Product object is now updated with the latest data. 

## Seeding Data

Now that we have a working database for our Products, lets do a bit of a cleanup and also introduce seeding data so we have our current static product list store into our database. 

1. Open the DealService.cs file and first add a new attributed called DataAdded to the Product list:

```c#
    new Product { 
        Id = 1, 
        Name = "Fitness Trackers", 
        Category = "Jewellery", 
        Description = "Aliexpress fintness trackers", 
        ImageUrl = "imgs/aliexpress-fitness-trackers.jpg", 
        Price = 89.99, 
        DateAdded = DateTime.Now //New Attribute to each Product item
        },
    ...
```
2. Next, create a new file called SeedData.cs inside the Models directory and add the following:
```c#
using ECommerceMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()
        ))
        {
            if(context.Product.Any())
            {
                return;
            }

            context.Product.AddRange(
                //Copy and paste the Product list in here!
            );

            context.SaveChanges();
        }
    }
}
```
3. Next, copy the product list and past in the .AddRange method. You can also remove the Product list code and just keep the return new List<Product>() inside GetListOfProducts() method of DealService.cs. 
```c#
    public static IEnumerable<Product> GetListOfProducts(){
        return new List<Product>();
    }
```
4. We now need to add some logic to seed our data upon startup. Open the Program.cs file and add the following:
```c#
//add a reference to our Models
using ECommerceMVC.Models;

//make sure the below is after both the app and builder declarations!
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

```
5. Start up the app by running dotnet run or dotnet watch and then go to the /Products page. 
6. You may notice there is still only 1 product. The reason for this is our logic in the SeedData file. 
```c#
    if(context.Product.Any())
    {
        return;
    }
```
7. This checks to see if there are currently any Products in the database, if yes it stops the data from being seeded. Let's fix this. 
8. With the app still running, on the /Products page click the Delete link on the right. 
9. Next, click the Delete button to confirm. 
10. The list should now be blank. 
11. Stop the app from the command line and then restart it by running dotnet run. 
12. Go back to the /Products page and observe the output
13. You should now see a list of products!

## Cleanup

1. With the app still running click on the Products link in the top navigation bar. 
2. You will notice that the page is now blank as the method was changed to only return a blank list. 
3. Let's fix this us so that it gets the data from the database instead. 
4. Open the DealService file and add the following:
```c#
    //add reference to the Data context folder
    using ECommerceMVC.Data;

    private readonly ApplicationDbContext _context;

    public DealsService(ApplicationDbContext context)
    {
            _context = context;
    }
```
5. Next, we need to update our method to get a List of Products using the _context:
```c#
    public IEnumerable<IGrouping<string?, Product>> GetProductCategories(){
        IEnumerable<Product> products = GetListOfProducts();
        if(products ==  null){
            return Enumerable.Empty<IGrouping<string, Product>>();
        }
        var categories = products.GroupBy(item => item.Category);
        return categories;
    } 

    public IEnumerable<Product> GetListOfProducts(){
        return _context.Product.ToList();
    }
```
6. Notice also that we changed from using static methods. This also means that we need to change our HomeController methods.
7. First, add a reference to the DealsService and use DI for the Constructor:
```c#
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DealsService _dealService;

    public HomeController(ILogger<HomeController> logger, DealsService dealsService)
    {
        _logger = logger;
        _dealService = dealsService;
    }
```
8. Next, update the methods to use the _dealsService field as the service class is no longer static:

```c#
    public IActionResult Index()
    {
        IEnumerable<Product> Products = _dealService.GetListOfProducts();
        return View(Products);
    }

    public IActionResult Products(){
        var categories = _dealService.GetProductCategories();
        return View(categories);
    }
```
9. One final thing to do is to register our DealService upon startup. Open the Program.cs file and add the following:

```c#
builder.Services.AddScoped<DealsService>();
```
10. Startup the app and load the /Home and /Home/Products page. 

