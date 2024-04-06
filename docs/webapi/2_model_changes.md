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
7. One other column you may have noticed is the Discriminator column - this will be used to filter the data based on its type. 

## Related Articles

Entity Framework Inheritance: https://learn.microsoft.com/en-us/ef/core/modeling/inheritance

