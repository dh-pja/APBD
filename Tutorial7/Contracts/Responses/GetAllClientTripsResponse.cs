namespace Tutorial7.Contracts.Responses;

public record struct GetAllClientTripsResponse(
    int Id,
    string Name,
    string Description,
    DateTime DateFrom,
    DateTime DateTo,
    int MaxPeople,
    int RegisteredAt,
    int? PaymentDate,
    ICollection<GetCountryResponse> Countries
);