namespace ECommerceCommon.Models;

public class Book : Product
{
    public string? ISBN {get;set;}
    public string? Author{get;set;}
    public int? Year {get; set;}
    public string? Publisher {get;set;}
}