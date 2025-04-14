using System.ComponentModel.DataAnnotations;

namespace RESTApi.Contracts.Requests;

public class CreateVisitRequest
{
    [Required]
    public DateTime DateOfVisit { get; set; }
    
    [Required]
    public int AnimalId { get; set; }
    
    [StringLength(1024, ErrorMessage = "Description must be less than 1024 characters")]
    public string? Description { get; set; }
    
    [Required]
    public int Price { get; set; }
}