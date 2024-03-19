using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category {get; set; }
    public string? ImageUrl { get; set; }
    public double? Price {get; set;}
    
    [DataType(DataType.Date)]
    public DateTime? DateAdded {get; set;}
};