# Paging Data

Our currnent application only has a small handful of data to test and verify our changes. But now, we need to think about larger datasets and how that can effect our API. We wil begin by loading data from a csv file that contains over 250,000 records. 


## Getting Started

1. Let's start by importing some data - choose the script that works for you based on your database!
2. You will need to the data file, it is too big to put on Github
3. Step into the data folder and run the script by running:
```shell
python import-data-mssql.py
```
or
```shell
python import-data-sqlite.py
```
4. The data import will being and will take about 10 mins to complete. 

## Testing the Product API

1. After the data has been imported, open the terminal and run the app:
```shell
dotnet run
```
2. Next, open your browser and go to the swagger UI - http://localhost:&lt;YOUR PORT&gt;/swagger/index.html
3. Click on the GET request for /api/Product, pass in the productType of 'book' and click the Try it out button and then Execute
4. You will most likely hear your computer getting louder and notice your browser is not responding.
5. So what happened? Why did it crash and become unresponsive. The issue is down to data - more specifically the ammount of it. The data import we just done added over 250,000 records into our database. When calling the GET request for /api/Product it tried to return all these records which is nearly 80 megabyes in size!

## Paging API Data

1. Let's start with the GET Request for getting a list of products. 
2. Open the ECommerceCommon folder and inside it, create a new folder called Responses. 
3. Create a new class called ProductListResponse.cs. 


```c#
[HttpGet]
public async Task<ActionResult<ProductListResponse>> GetProducts(string? category, int page = 1, int pageSize = 20)
{
            if (_context.Products == null)
            {
                return NotFound();
            }
            //first get total pages
            int totalCount = await _context.Books.CountAsync();
            int totalPages = totalCount / pageSize;
            var productsPage = await _context.Books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ProductListResponse
            {
                paging = new Paging() {
                    page =page,
                    pageSize = pageSize,
                    totalPages = totalPages,
                    total = totalCount
                },
                data = productsPage
            };
        }
```