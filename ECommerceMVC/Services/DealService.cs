using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Models;

namespace ECommerceMVC.Services;

public class DealsService
{

    public static IEnumerable<IGrouping<string?, Product>> GetProductCategories(){
        IEnumerable<Product> products = GetListOfProducts();
        if(products ==  null){
            return Enumerable.Empty<IGrouping<string, Product>>();
        }
        var categories = products.GroupBy(item => item.Category);
        return categories;
    } 

    public static IEnumerable<Product> GetListOfProducts(){
        return new List<Product>()
        {
            new Product { Id = 1, Name = "Fitness Trackers", Category = "Jewellery", Description = "Aliexpress fintness trackers", ImageUrl = "imgs/aliexpress-fitness-trackers.jpg", Price = 89.99 },
            new Product { Id = 2, Name = "Black Bag", Category = "Bags", Description = "Black over the shoulder bag", ImageUrl = "imgs/black-bag-over-the-shoulder.jpg", Price = 59.95 },
            new Product { Id = 3, Name = "Runners", Category = "Runners", Description = "Black Sneakers with white sole", ImageUrl = "imgs/black-sneakers-with-white-sole.jpg", Price = 40.00 },
            new Product { Id = 4, Name = "Headphones", Category = "Headphones", Description = "Volume Control Headphones", ImageUrl = "imgs/volume-control-headphones.jpg", Price = 179.99 },
            new Product { Id = 5, Name = "Sport Runners", Category = "Runners", Description = "All black sports runners", ImageUrl = "imgs/right-foot-all-black-sneaker.jpg", Price = 44.99 },
            new Product { Id = 6, Name = "QR Codes", Category = "Tech", Description = "QR Codes In Store", ImageUrl = "imgs/qr-codes-in-store.jpg", Price = 2.99 },
            new Product { Id = 7, Name = "Gemstone Necklace", Category = "Jewellery", Description = "Purple Gemstone necklace", ImageUrl = "imgs/purple-gemstone-necklace.jpg", Price = 19.95 },
            new Product { Id = 8, Name = "Modern Watch", Category = "Jewellery", Description = "Modern time piece watch", ImageUrl = "imgs/modern-time-pieces.jpg", Price = 45.00 },
            new Product { Id = 9, Name = "Leather Jacket", Category = "Clothes", Description = "Kids Leather Jacket", ImageUrl = "imgs/kids-leather-jacket.jpg", Price = 119.99 }
        };
    }
}