using Tutorial7.Contracts.Responses;
using Tutorial7.Entities;

namespace Tutorial7.Mappers;

public static class TripMapper
{
    public static ICollection<GetAllTripsResponse> MapToGetAllTripsResponse(ICollection<Trip> trips)
    {
        return trips.Select(x => new GetAllTripsResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            DateFrom = x.DateFrom,
            DateTo = x.DateTo,
            MaxPeople = x.MaxPeople,
            Countries = x.Destinations.Select(country => new GetCountryResponse(country.Id, country.Name)).ToList(),
        }).ToList();
    }

    public static ICollection<GetAllClientTripsResponse> MapToGetAllClientTripsResponse(ICollection<Trip> trips)
    {
        return trips.Select(x => new GetAllClientTripsResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            DateFrom = x.DateFrom,
            DateTo = x.DateTo,
            MaxPeople = x.MaxPeople,
            RegisteredAt = x.Participants.FirstOrDefault().RegisteredAt,
            PaymentDate = x.Participants.FirstOrDefault().PaymentDate,
            Countries = x.Destinations.Select(country => new GetCountryResponse(country.Id, country.Name)).ToList(),
        }).ToList();
    }
}