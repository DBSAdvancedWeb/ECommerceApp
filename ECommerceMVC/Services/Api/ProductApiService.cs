using System.Net.Http.Json;
using ECommerceMVC.Models;

namespace ECommerceMVC.Services.Api;


public class ProductApiService : IProductApiServices
{
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
    public Task<Product> AddProduct()
    {
        throw new NotImplementedException();
    }

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