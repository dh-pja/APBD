using Tutorial9.Models;

namespace Tutorial9.DTO;


public class TripClientResponse
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class TripCountryResponse
{
    public string Name { get; set; } = string.Empty;
}

public class TripResponse
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; } = DateTime.MinValue;
    public DateTime DateTo { get; set; } = DateTime.MinValue;
    public int MaxPeople { get; set; }
    public IEnumerable<TripCountryResponse> Countries { get; set; } = new List<TripCountryResponse>();
    public IEnumerable<TripClientResponse> Clients { get; set; } = new List<TripClientResponse>();
}

public class PagedTripsResponse
{
    public int AllPages { get; set; }
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<TripResponse> Trips { get; set; } = new List<TripResponse>();
}