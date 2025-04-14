namespace RESTApi.Models;

public class Animal
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required double Weight { get; set; }
    public required string FurColor { get; set; }
}