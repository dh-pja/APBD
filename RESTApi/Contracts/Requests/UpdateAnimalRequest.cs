using System.ComponentModel.DataAnnotations;

namespace RESTApi.Contracts.Requests;

public class UpdateAnimalRequest
{
    [StringLength(128, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 128 characters")]
    public string Name { get; set; }
    
    [Range(0.01, 10000, ErrorMessage = "Weight must be between 0.01 and 10000")]
    public double Weight { get; set; }
    
    [StringLength(128, MinimumLength = 2, ErrorMessage = "FurColor must be between 2 and 128 characters")]
    public string FurColor { get; set; }
}