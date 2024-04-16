# Paging Data

Our currnent application only has a small handful of data to test and verify our changes. But now, we need to think about larger datasets and how that can effect our API. We wil begin by loading data from a csv file that contains over 250,000 records. 


## Getting Started

1. Let's start by importing some data - choose the script that works for you based on your database!
2. You will need to the data file, it is too big to put on Github but it can be located [here]()
3. Step into the data folder and run the script by running:
```shell
python import-data-mssql.py
```
or
```shell
python import-data-sqlite.py
```
4. The data import will being and will take about 10 mins to complete. 

## Testing the Product API

1. After the data has been imported, open the terminal and run the app:
```shell
dotnet run
```
2. Next, open your browser and go to the swagger UI - http://localhost:&lt;YOUR PORT&gt;/swagger/index.html
3. Click on the GET request for /api/Product, pass in the productType of 'book' and click the Try it out button and then Execute
4. You will most likely hear your computer getting louder and notice your browser is not responding.
5. So what happened? Why did it crash and become unresponsive. The issue is down to data - more specifically the ammount of it. The data import we just done added over 250,000 records into our database. When calling the GET request for /api/Product it tried to return all these records which is nearly 80 megabyes in size!

## Paging API Data

1. Let's start with the GET Request for getting a list of products. 
2. Open the ECommerceCommon folder and inside it, create a new folder called Responses. 
3. Create a new class called Paging.cs and add the following code:
```c#
namespace ECommerceCommon.Responses;

public class Paging
{
    public int page {get; set;}
    public int pageSize {get; set;}
    public int totalPages {get; set;}
    public int total {get; set;}
}
``` 
4. Next, create another file called ProductListResponse.cs and add the following code:
```c#
using ECommerceCommon.Models;

namespace CommonLibrary.Responses;
public class ProductListResponse<T>
{
    public Paging paging {get; set;}

    public IEnumerable<T> data {get; set;}
}
```
5. The T for the IEnumerable is for a Generic object. Considering we have a Book and Fashion item, we will need this to allow different types to be queried and returned.
6. Next, go to our ProductController.cs inside the ProductApi folder. Locate the method called GetProductType and remove it from the Controller. We are going to create a new Service class to handle the interactions with the database. 
7. Let us start with an Interface first. Create a new folder called Services and add a file called IProductService.cs. Update the file with the following code:
```c#
using ECommerceCommon.Responses;

namespace ProductApi.Services;

public interface IProductService
{
    public Task<ProductListResponse<T>> GetListOfProductsByType<T>(int page, int pageSize);
}
```
8. Next, create a new file called ProductSevrvice.cs and add the following:
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ECommerceCommon.Models;
using ECommerceCommon.Responses;
using ProductApi.Data;


namespace ProductApi.Services;

public class ProductService : IProductService
{

    private readonly ProductsDbContext _context;

    public ProductService(ProductsDbContext context){
        _context = context;
    }

    public async Task<ProductListResponse<T>> GetListOfProductsByType<T>(int page, int pageSize)
    {   
        IQueryable<Product> productsQuery = _context.Products;

        int totalCount = await productsQuery.OfType<T>().CountAsync();
        var productList = await productsQuery.OfType<T>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        return new ProductListResponse<T>
        {
            paging = new Paging() { 
                page = page,
                pageSize = pageSize,
                totalPages = 0,
                total = 100   
            },
            data = productList
        };
    }

}
```
9. We will also register our new Interface & Service class in the Program.cs file:
```c#
builder.Services.AddScoped<IProductService, ProductService>();
```
10. Go back to the ProductController and lets fix up the code to make it use our new ProductService. 
```c#
//add reference to new service
using ProductApi.Services;
...

namespace ProductApi.Controllers
{
    //update our route to use the new paths
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductsDbContext _context;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, ProductsDbContext context, IProductService productService)
        {
            _logger = logger;
            _context = context;
            _productService = productService;
        }
```
11. Next, we will create two new method called GetBooks and GetFashions:
```c#
[HttpGet("books")]
public async Task<ActionResult<ProductListResponse<Book>>> GetBooks(int page = 1, int pageSize = 10)
{
    var productList = await _productService.GetListOfProductsByType<Book>(page, pageSize);
    return Ok(productList);
}

[HttpGet("fashion")]
public async Task<ActionResult<ProductListResponse<Fashion>>> GetFashion(int page = 1, int pageSize = 10)
{
    var productList = await _productService.GetListOfProductsByType<Fashion>(page, pageSize);
    return Ok(productList);
}
```
12. Load up the application by running dotnet run and go to the Swagger UI page. You should not see two new endpoints for both /books and /fashions. Test them out to verify everything works correctly. 
