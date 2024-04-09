# Classlibs

As you have seen we had to change the Models in order to integrate the new attributes. This can happen quite often and it will also have an effect on the other projects such as the ECommereceMVC. Lets make it easier by creating a classlib project that will contain all our Models and Responses and then use the dotnet reference to link them. 

1. Open a new terminal and in the root of the EcommerceApp run the following:
```c#
dotnet new classlib -n ECommerceCommon -lang c# -f net7.0
```
2. A new ECommerceCommon folder will be created. Open it up and notice it has one class called Class1.cs - delete this file. 
3. Next, in the ECommerceCommon folder, create a new folder called Models.  
4. Open the ProductApi/Models folder and drag them into the ECommerceCommon/Models folder.
5. Open each of the model files, Book, Fashion and Product and update the namespace from ProductApi.Models to ECommerceCommon.Models (-- = Remove, ++ = Add)
```c#
-- namespace ProductApi.Models;
++ namespace ECommerceCommon.Models;
```
5. Next, open up the terminal and step into the ProductApi folder then run the command below:
```shell
dotnet add reference ../ECommerceCommon/ECommerceCommon.csproj 
```
6. This will add the reference to the CommonLibary inside the ProductApi.csproj file. Open it up and you should see the following:
```xml
  <ItemGroup>
    <ProjectReference Include="..\ECommerceCommon\ECommerceCommon.csproj" />
  </ItemGroup>
```
7. We now need to clean up any stale references to the namespace of ProductApi.Models in our Product API file.
8. Start with the ProductController -- = Remove, ++ = Add
```c#
-- using ProductApi.Models;
++ using ECommerceCommon.Models;
```
9. ProductDbContext is next, add and replace the following
```c#
-- using ProductApi.Models;
++ using ECommerceCommon.Models;
```
11. Program.cs file is last, remove any reference to the following:
```c#
-- using ProductApi.Models;
```
12. Remove the Model folder inside the ProductsApi folder if it still exists - should be empty.
13. Run dotnet build to verify the project can be compiled OK. 
14. Finally, run the Product Api to verify all works OK. 
