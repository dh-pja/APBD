namespace RESTApi.Models;

public class Visit
{
    public required int Id { get; set; }
    public required DateTime DateOfVisit { get; set; }
    public required int AnimalId { get; set; }
    public string? Description { get; set; }
    public required int Price { get; set; }
}