# Creating MVC Web Applications

ASP.NET Core MVC Website that will have its own local DB and setup for Authentication. .Net Core provides a lot of handy setups out of the box using its cli or via Visual Studio. 

## Help Info

To see a lis of options that are provided run the query below via the cli in the command. Observe and read the attributes and flags that can be passed in when creating your application. 
```shell
dontet new mvc --help
```

## Create MVC Application

For the MVC app we want authentication and also to use a SQLite DB. Run the following command

```shell
dotnet new mvc -n ECommerceMVC -au Individual -f net7.0
```

The MVC app called ECommerce MVC should not be generated. Go inside the directory and look at the files and folders created.


## Folders:

* **Areas (Optional):** Used for organizing the application into logical sections with their own controllers, models, and views. Not included by default.

* **Controllers:** Stores all the controller classes that handle user requests and interact with models and views.

* **Data:** Can hold data access related files, like database context classes or entity configuration files (if using Entity Framework).

* **Models:** Contains the data models representing the data your application works with (e.g., product, customer).

* **obj:** Used by the build process for temporary compilation files. Typically excluded from version control.

* **Properties:** Stores project configuration files:

* **appsettings.json:** Key-value pairs for application configuration.

* **appsettings.Development.json:** Similar to appsettings.json but specific to the development environment.

* **ECommerceMVC.csproj:** The main project file containing project configuration and references.

* **Views:** Contains all the view files defining the UI using HTML, CSS, and Razor syntax.

* **wwwroot:** Stores static files used by the application, such as images, CSS, JavaScript, and other assets.

* **app.db** (Optional): Your database file, depending on the chosen technology during project creation (e.g., SQLite database).

## Files:

* **Program.cs:** The entry point for your application, responsible for starting and hosting the web server.

## Additional Notes:

    * The -au Individual argument specifies the Individual User Accounts authentication scheme, not reflected in the folder structure.
    * The .net7.0 argument indicates the use of the .NET 7.0 framework.

## Running the Application

In the command line, run the following:

```shell
cd ECommerceMVC
dotnet build
dotnet run
```

You should see output like below:

```shell
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5272
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\andyf\Coding\AdvancedWeb\ECommerceApp\ECommerceMVC
```