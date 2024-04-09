# Model Changes

Our Products class is pretty basic and given that we want to store multiple types of products such as Books, Fashion & Tech, our current model is not suted for purpose. We want to extend our Product model so it can have additinal attributes based upon other classes. For example lets say we want to store info on a Book. Some attributes of a book are ISBN, Genre, Author and Publisher. While a Fashion object will have attributes like Colour, Sizes and Gender. 

In C# we can represent this as inheritance of our classes. For example, the Product class becomes a base class that the others will extend from. All the common attributes of Product will be available to all the ones in the sub class of Book and Fashion. This leads into how this will be represented in the database. By default Entity Framework will map these to a single table using table-per-hierarchy mapping strategy. TPH uses a single table with a discriminator column to indicate the correct type - Book, Fashion etc. EF also has other alternatives such as table-per-type (TPT) and table-per-concrete-type (TPC). 

Which one you need to pick will normally boil down to speed and performance and overall ease of use. TPH tends to be the best in terms of performance given that all data is stored in a single table and there is not need for joins. Given that its the default, we will run with that. 

## Table-per-hierarchy (TPH) Mapping Strategy

1. Open the Products API and go to the Models folder. 
2. Create two new classes called Book.cs and Fashion.cs.
```c#
namespace ProductApi.Models;

public class Book : Product
{
    public string? ISBN {get;set;}
    public string? Author{get;set;}
    public int? Year {get; set;}
    public string? Publisher {get;set;}
}

-----------------------------------

namespace ProductApi.Models;

public class Fashion : Product
{
    public string? Type { get; set; }
    public string? Gender { get;set; }
    public string? Brand { get; set; }
    public string? Size { get;set; }
    public string? Age { get; set; }
    public string? Colour { get;set; }
}

```
3. We also want to update our Product model with being abstract and add a few more attributes:
```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Models;

public abstract class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category {get; set; }
    public string? SubCategory {get; set;}
    public string? ImageSmall { get; set; }
    public string? ImageMedium { get; set; }
    public string? ImageLarge { get; set; }
    
    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price {get; set;}
    
    [DataType(DataType.Date)]
    public DateTime? DateAdded {get; set;}
};
```
4. Next, open the Data/ProductDbContext.cs file and add the following:
```c#
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
   
namespace ProductApi.Data;
public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Fashion> Fashions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().UseTphMappingStrategy();
    }
}
```
5. Open up the terminal and run the following:
```shell
dotnet ef migrations add BooksFashionsTypes
dotnet ef database update
```
6. Open up a SQL Editor of your choice and notice how the Product table has gotten a lot bigger. It should now contain all the columns for Product, Book & Fashion. 
7. One other column you may have noticed is the Discriminator column - this will be used to filter the data based on its type. At present it is currently empty and this needs to have a value otherwise it will return an error. 
8. Open up the database and go to the SQL Editor view. Run the following query:
```sql
UPDATE Products
SET Discriminator = 'Fashion'
WHERE 1 = 1; 
```
9. This will update all 9 records. 
10. Load up the Product Api service and go to the Swagger UI to test the Get Request endpoint for /api/Product
11. Click the Try it out button and then execute. 
12. Observe the feed that comes back:
```json
[
  {
    "id": "1a01d87b-e2a0-4671-835f-2ff092f3a692",
    "name": "Black Bag",
    "description": "Black over the shoulder bag",
    "category": "Bags",
    "subCategory": null,
    "imageSmall": null,
    "imageMedium": null,
    "imageLarge": null,
    "price": 59.95,
    "dateAdded": "2024-04-06T13:53:37.8452386"
  },
  ...
]
```
13. You will notice that none of the attributes for Fashion return back. This is something we need to fix. Open the ProductController.cs and add a new parameter called **productType**:
```c#
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? productType)
        {
            var productList = await GetProductType(productType);

            if(productList == null)
            {
                return BadRequest($"Invalid type of '{productType}'.");
            }

            return productList;
        }
``` 
14. The GetProductType method also needs to be implemented. In the same file ProductController.cs add a new private method:
```c#
    private async Task<ActionResult<IEnumerable<Product>>> GetProductType(string? productType)
    {
        IQueryable<Product> query = _context.Products;

        switch(productType.ToLower()){
            case "fashion":
                return await query.OfType<Fashion>().ToListAsync();
            case "books":
                return await query.OfType<Book>().ToListAsync();;
            default:
                //type does not exist
                return null;
        }

    }
```
15. The above code will check the value being passed in for the productType and then return the correct list based on its type. Load up the Swagger UI to test and verify. 

## Related Articles

Entity Framework Inheritance: https://learn.microsoft.com/en-us/ef/core/modeling/inheritance

