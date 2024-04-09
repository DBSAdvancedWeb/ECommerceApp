using ECommerceCommon.Models;

namespace CommonLibrary.Responses;
public class ProductListResponse
{
    public Paging paging {get; set;}

    public IEnumerable<T> data {get; set;}
}