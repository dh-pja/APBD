namespace Tutorial7.Contracts.Responses;

public record struct GetAllTripsResponse(
    int Id,
    string Name,
    string Description,
    DateTime DateFrom,
    DateTime DateTo,
    int MaxPeople,
    List<GetCountryResponse> Countries
    );