namespace Tutorial7.Entities;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Trip> Trips { get; set; } = new();
}