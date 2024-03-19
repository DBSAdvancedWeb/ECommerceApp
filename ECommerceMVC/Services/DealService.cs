using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;
using ECommerceMVC.Data;
using System.Reflection.Metadata.Ecma335;

namespace ECommerceMVC.Services;

public class DealsService
{
    private readonly ApplicationDbContext _context;

    public DealsService(ApplicationDbContext context)
    {
            _context = context;
    }

    public IEnumerable<IGrouping<string?, Product>> GetProductCategories(){
        IEnumerable<Product> products = GetListOfProducts();
        if(products ==  null){
            return Enumerable.Empty<IGrouping<string, Product>>();
        }
        var categories = products.GroupBy(item => item.Category);
        return categories;
    } 

    public IEnumerable<Product> GetListOfProducts(){
        return _context.Product.ToList();
    }
}