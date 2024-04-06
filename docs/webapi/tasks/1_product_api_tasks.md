# Product API Taks

## Seed Data

1. We will use the migrations script to add our data, open a terminal and type the following:
```shell
dotnet ef migrations add LoadProductData
```
2. Go to the Migration file for the LoadProductData.cs (Not the designer file)
3. We have two methods - Up and Down. Up is for the insertions and down is for rollback - delete all data. 
4. In the Up method add the following code:
```c#
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] {"Id", "Name","Category", "Description","ImageUrl","Price","DateAdded"},
                values: new object[,]
                {
                    { Guid.NewGuid(), "Fitness Trackers", "Jewellery", "Aliexpress fintness trackers", "imgs/aliexpress-fitness-trackers.jpg", 89.99, DateTime.Now },
                    { Guid.NewGuid(), "Black Bag", "Bags", "Black over the shoulder bag", "imgs/black-bag-over-the-shoulder.jpg", 59.95, DateTime.Now },
                    { Guid.NewGuid(), "Runners","Runners", "Black Sneakers with white sole", "imgs/black-sneakers-with-white-sole.jpg", 40.00, DateTime.Now },
                    { Guid.NewGuid(), "Headphones", "Headphones", "Volume Control Headphones", "imgs/volume-control-headphones.jpg", 179.99, DateTime.Now },
                    { Guid.NewGuid(), "Sport Runners", "Runners", "All black sports runners", "imgs/right-foot-all-black-sneaker.jpg", 44.99, DateTime.Now },
                    { Guid.NewGuid(), "QR Codes", "Tech", "QR Codes In Store", "imgs/qr-codes-in-store.jpg", 2.99, DateTime.Now },
                    { Guid.NewGuid(), "Gemstone Necklace","Jewellery", "Purple Gemstone necklace", "imgs/purple-gemstone-necklace.jpg", 19.95, DateTime.Now },
                    { Guid.NewGuid(), "Modern Watch", "Jewellery", "Modern time piece watch", "imgs/modern-time-pieces.jpg", 45.00, DateTime.Now },
                    { Guid.NewGuid(), "Leather Jacket", "Clothes", "Kids Leather Jacket", "imgs/kids-leather-jacket.jpg", 119.99, DateTime.Now }
                }
            );
        }
```
5. In the down method add the following:
```c#
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                    table: "Products",
                    keyColumn: "Id",
                    keyValues: null);
        }
```
6. Run the following:
```shell
dotnet ef database udpate
```
7. Start and run the Web API and go to the Swagger UI - http://localhost:PORTNO/swagger/index.html
8. Click on the /api/Product GET request, then click the "Try it out" button and then the "Execute" button. 
9. Verify that the response has data. 


## Categories Endpoint

1. Open the ProductsController and add a new method called GetProductCategories()
```c#
        [HttpGet("categories")]
        public async Task<ActionResult<Product>> GetProductCategories()
        {

        }
```
2. First update the return type so it provides us what we need in terms of a key: value of categories:
```c#
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<IGrouping<string?, Product>>>> GetProductCategories()
        {
            
        }
```
3. Let's implement the method. We first want to get a list of data from the database and then check if it has any values:
```c#
    var products = await _context.Products.ToListAsync();

    if(products ==  null){
        return Ok(new List<IGrouping<string?, Product>>());
    }
```
4. If no products exist we return back an empty list.
5. Next, we need to categorise the data and group it based on the category type:
```c#
var categories = products.GroupBy(item => item.Category)
                                         .Select(group => new { category = group.Key, products = group.ToList() });
```
6. Finally, return the data back to the client
````c#
return Ok(categories);
````
7. Full code below:
```c#
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<IGrouping<string?, Product>>>> GetProductCategories()
        {
            var products = await _context.Products.ToListAsync();

            if(products ==  null){
                return Ok(new List<IGrouping<string?, Product>>());
            }
            var categories = products.GroupBy(item => item.Category)
                                         .Select(group => new { category = group.Key, products = group.ToList() });
            return Ok(categories);
        }
```

### Related Articles

* MigrationBuilder InsertData - https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.migrations.migrationbuilder.insertdata?view=efcore-7.0
