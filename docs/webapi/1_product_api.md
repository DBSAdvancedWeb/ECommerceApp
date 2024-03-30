# Product API



## Web API
1. Let us start by creating a new Web API via the dotnet cli:

```shell
dotnet new webapi -o ProductApi --no-https -f net7.0
```
2. Step into the ProductApi directory and then load up the application by running dotnet run.
3. Next, open the browser and go to the following URL: http://localhost:<YOUR PORT>/swagger/index.html
4. Swagger UI is a Open API documentation tool to showcase how your API works and what endpoints it exposes. As you can see from the Swagger UI there is one endpoint generated on the ProductApi called WeatherForecast. Click on the GET /WeatherForcecast. An accordion dropdown will open and show details about the endpoint. 
5. Click the Try it out button on the top right inside the /WeatherForecast. A large blue button with Execute will appear - Click it. 
6. When its executed it will make a HTTP GET request and then receive a response in JSON with a list of Weather Forecast data.
7. You can learn more about Open API via their website here: https://www.openapis.org/



## Getting started

In this lesson we want to build a new Web API for Products and then have the MVC application talk to this instead of using its own Service file and Database. The Product API will have its own Database that will run on its own port number seperate from the MVC application. This allows for greater flexiblity in terms of scaling but it does also introduce some added complexity. 

1. Before we begin, we first want to setup our Dockerfile so we can spin up a MSSQL database locally for the service. 
2. In the root of the ProductApi create a new file called Dockerfile and add the following content:
```shell
# Use the official SQL Server 2019 Express base image from Microsoft
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=P@ssw0rd!

# Expose port 1433 for SQL Server
EXPOSE 1433

# Create a directory inside the container for your database files
RUN mkdir -p /var/opt/mssql/data

# Grant permissions for the SQL Server user
RUN chown -R mssql:mssql /var/opt/mssql/data

# Set the working directory
WORKDIR /var/opt/mssql

# Copy any database scripts or backups to initialize the database
#COPY your_database_scripts_or_backups /var/opt/mssql/data

# Start SQL Server when the container starts
CMD [ "/opt/mssql/bin/sqlservr" ]

```
3. Open up a terminal and run the following inside the ProductApi folder:
```shell
docker build -t product-api-db .
```
4. This will create the docker image for our MSSQL database. We will come back to this in a while. First, lets setup our Product Api. 
5. We need to add some dependencies, open the terminal and run in the following:
```shell
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.17
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.17
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.17
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 7.0.12
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.17
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.17
```
6. Next, lets create a Model folder and then add a Product.cs file with the following:
```c#
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models;

public class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category {get; set; }
    public string? ImageUrl { get; set; }
    public double? Price {get; set;}
    
    [DataType(DataType.Date)]
    public DateTime? DateAdded {get; set;}
};
```
7. We have made some minor changes. The ID will no longer be an int but a GUID. This is better for security and also keeps Ids consistant in terms of length. 
8. Next, lets create a valid Data Context for our Products. 
9. Create a new folder called Data and add a new file called ProductsDbContext.cs and add the following:
```c#
{
   public class ProductsDbContext : DbContext
   {
       public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
       {}


       public DbSet<Product> Products { get; set; } = null!;
   }

}
```
10. We now need to add our connection string to the configs. Open the appsetting.json file and add the following directly under AllowedHosts
```shell
  "ConnectionStrings": {
    "ProductApiDbConnection": "Server=localhost;Database=ProductDB;User=sa;Password=P@ssw0rd!;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
```
11. Next, open the Program.cs file and add the following:
```c#
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.Data;


var builder = WebApplication.CreateBuilder(args);

//Add and configure database connection
var connectionString = builder.Configuration
    .GetConnectionString("ProductApiDbConnection") ?? 
    throw new InvalidOperationException("Connection string 'ProductApiDbConnection' not found.");

builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseSqlServer(connectionString));
```
12. We can now scaffold our Controller by running the following command:
```shell
dotnet-aspnet-codegenerator controller -name ProductController -async -api -m Product -dc ProductsDbContext -outDir Controllers -dbProvider mssql
``` 
13. Next, some minor cleanup, Delete the WeatherForecastController.cs and WeatherForecast.cs file.
14. We can now begin our migration but before we start, we need to make sure our docker image is running and we can access it. Open up Docker Deskop and Click on the Images in the left hand navigation. 
15. Locate the product-api-db we created earlier and then click on the Play button to the right. A modal will appear with Opional settings. Click the dropdown and add a container name of ProductDB and set the Host port as 1433 and then click on Run. 
16. We can now create and run our migrations. Open the terminal and run the following:
```shell
dotnet ef migrations add AddProduct
dotnet ef database update
```
17. If successful you should have a new DB created along with a Products table. You can view it by opeing up [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16).


## Tasks

1. Scaffold data into the Database.
2. Create new endpoints for returning a category of products like we have from the MVC application.  