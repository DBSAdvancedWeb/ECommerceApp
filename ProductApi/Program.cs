using Microsoft.EntityFrameworkCore;
using ECommerceCommon.Models;
using ProductApi.Data;
using ProductApi.Services;


var builder = WebApplication.CreateBuilder(args);

//Add and configure database connection
var connectionString = builder.Configuration
    .GetConnectionString("ProductApiDbConnection") ?? 
    throw new InvalidOperationException("Connection string 'ProductApiDbConnection' not found.");

builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
