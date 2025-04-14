using System.ComponentModel.DataAnnotations;

namespace RESTApi.Contracts.Requests;

public class CreateAnimalRequest
{
    [Required]
    [StringLength(128, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 128 characters")]
    public string Name { get; set; }
    
    [Required]
    [StringLength(128, MinimumLength = 2, ErrorMessage = "Category must be between 2 and 128 characters")]
    public string Category { get; set; }
    
    [Required]
    [Range(0.01, 10000, ErrorMessage = "Weight must be between 0.01 and 10000")]
    public double Weight { get; set; }
    
    [Required]
    [StringLength(128, MinimumLength = 2, ErrorMessage = "FurColor must be between 2 and 128 characters")]
    public string FurColor { get; set; }
}