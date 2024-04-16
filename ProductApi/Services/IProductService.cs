using ECommerceCommon.Responses;

namespace ProductApi.Services;

public interface IProductService
{
    public Task<ProductListResponse<T>> GetListOfProductsByType<T>(int page, int pageSize);
}