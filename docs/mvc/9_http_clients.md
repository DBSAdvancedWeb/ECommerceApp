# Http Clients

Now that we have a ProductApi we need to wire up a HttpClient in order to make HTTP calls to the external service. In .Net Core they provide many different ways to create a HttpClient but by far the most effecient is to use the IHttpClientFactory. It can also be implemented in a few different ways such as:

* Basic 
* Named clients
* Typed clients
* Generated clients 

In this example we will use the Typed client approach. 

## Prerequisites:

* Basic understanding of C#, ASP.NET Core
* You need to complete [Web API Lesson 1: Product API](../webapi/1_product_api.md)


## Getting Started

1. First, inside the Services folder, create a new folder called **Api**
2. Inside the Api folder, create a new interface called **IProductApiService** and add the following:
```c#
using ECommerceMVC.Models;

namespace ECommerceMVC.Services.Api;

public interface IProductApiServices
{
    public Task<IEnumerable<Product>> GetListOfProducts();

    public IEnumerable<IGrouping<string?, Product>> GetProductCategories();

    public Task<Product> GetProduct();

    public Task<Product> AddProduct();

    public Task<Product> UpdateProduct();
}
```
3. Next, lets implment the interface by creating a new class called **ProductApiService**
```c#
using ECommerceMVC.Models;

namespace ECommerceMVC.Services.Api;


public class ProductApiService : IProductApiServices
{

    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Product> AddProduct()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetListOfProducts()
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetProduct()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IGrouping<string?, Product>> GetProductCategories()
    {
        throw new NotImplementedException();
    }

    public Task<Product> UpdateProduct()
    {
        throw new NotImplementedException();
    }
}
```
4. We need to also create a constructor that will accept the HttpClient as a parameter for DI
```c#
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
```
5. Next, lets add some configuration to our dev environment to set the ProductApi Base Address. Open the appsettings.Development.json file and add the following:
```json
{
  ...
  "ApiConfig": {
    "ProductApi": {
      "BaseUrl": "http://localhost:<YOUR PORT>"
    }
  }
  ...
}

```
6. Now that we have a HttpClient, we need to tell it where our service lives via a property called BaseAddress. First, lets get access to our Config file changes. We also want to add a logger to our service too:
```c#
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductApiService> _logger; 
    private readonly IConfiguration _configuration;

    public ProductApiService(HttpClient httpClient, ILogger<ProductApiService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;

        var baseUrl = _configuration["ApiConfig:ProductApi:BaseUrl"];

        if(baseUrl == null) {
            _logger.LogError("A base Uri needs to be set for the Product API!");    
        }else{ 
            _httpClient.BaseAddress = new Uri(baseUrl);
        }
    }
```
7. We can now start to implement our first method. Go to the method of **GetListOfProducts** and add the following code:
```c#
    public async Task<IEnumerable<Product>> GetListOfProducts()
    {
     try
        {
            var response = await _httpClient.GetAsync("/api/product");

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error if needed
                throw new Exception($"Failed to retrieve products. Status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        }
        catch (HttpRequestException ex)
        {
            // Handle the HTTP request exception
            throw new Exception("Failed to retrieve products. See inner exception for details.", ex);
        }
    }
```
8. We can now test it out and verify we can call our external service via the HttpClient. We have a few things to do before hand. One is the register our new interface and class in the Program.cs file. Open it up and add the following:
```c#
builder.Services.AddHttpClient<IProductApiServices, ProductApiService>();
```
9. Next, go to the HomeController and lets switch the Index method to use the Product Service API call. 
```c#

    //register the Api Service


    public async Task<IActionResult> Index()
    {
        IEnumerable<Product> Products = await _productApi.GetListOfProducts();
        return View(Products);
    }
```
10. Load up both the MVC app and the Product Api app and go to the home page of the MVC app. 

## Related Resources
HttpClient class:  https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-8.0
IHttpClientFactory: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0