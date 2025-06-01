using Tutorial9.DTO;
using Tutorial9.Models;

namespace Tutorial9.Mappers;

public static class TripMapper
{
    public static TripResponse ToResponse(this Trip trip)
    {
        return new TripResponse
        {
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            MaxPeople = trip.MaxPeople,
            Countries = trip.IdCountries.Select(c => new TripCountryResponse { Name = c.Name }),
            Clients = trip.ClientTrips.Select(c => new TripClientResponse
            {
                FirstName = c.IdClientNavigation.FirstName, 
                LastName = c.IdClientNavigation.LastName
            })
        };
    }
}