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
        int totalPages = totalCount / pageSize;

        var productList = await productsQuery.OfType<T>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        return new ProductListResponse<T>
        {
            paging = new Paging() { 
                page = page,
                pageSize = pageSize,
                totalPages = totalPages,
                total = totalCount   
            },
            data = productList
        };
    }
}
