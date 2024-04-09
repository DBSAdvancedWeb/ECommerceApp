using Microsoft.EntityFrameworkCore;
using ECommerceCommon.Models;
   
namespace ProductApi.Data;
public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Fashion> Fashions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().UseTphMappingStrategy();
    }
}