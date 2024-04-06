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