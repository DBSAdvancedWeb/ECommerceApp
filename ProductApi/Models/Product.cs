using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Models;

public abstract class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category {get; set; }
    public string? SubCategory {get; set;}
    public string? ImageSmall { get; set; }
    public string? ImageMedium { get; set; }
    public string? ImageLarge { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price {get; set;}
    
    [DataType(DataType.Date)]
    public DateTime? DateAdded {get; set;}
};