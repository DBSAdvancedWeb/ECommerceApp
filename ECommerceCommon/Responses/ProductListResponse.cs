using System.Collections.Generic;
using ECommerceCommon.Models;

namespace ECommerceCommon.Responses;
public class ProductListResponse<T>
{
    public Paging paging {get; set;}

    public List<T> data {get; set;}
}