using Microsoft.Data.SqlClient;
using Tutorial7.Entities;
using Tutorial7.Repositories.Interfaces;

namespace Tutorial7.Repositories;

public class TripRepository : ITripRepository
{
    private readonly string _connectionString;

    public TripRepository(IConfiguration cfg)
    {
        _connectionString = cfg.GetConnectionString("DefaultConnection");
    }
    
    public async Task<ICollection<Trip>> GetAllTripsAsync(CancellationToken token)
    {
        var trips = new List<Trip>();
        const string query = """
                       SELECT t.*, c.* FROM Country_Trip as ct 
                          JOIN Trip as t ON ct.IdTrip = t.IdTrip
                          JOIN Country as c ON ct.IdCountry = c.IdCountry
                       """;

        await using SqlConnection connection = new(_connectionString);
        await using SqlCommand command = new(query, connection);
        
        await connection.OpenAsync(token);
        await using SqlDataReader reader = await command.ExecuteReaderAsync(token);

        try
        {
            while (await reader.ReadAsync(token))
            {
                var trip = new Trip
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    DateFrom = reader.GetDateTime(3),
                    DateTo = reader.GetDateTime(4),
                    MaxPeople = reader.GetInt32(5)
                };

                var country = new Country
                {
                    Id = reader.GetInt32(6),
                    Name = reader.GetString(7)
                };

                trip.Destinations.Add(country);
                trips.Add(trip);
            }
        }
        finally
        {
            await reader.CloseAsync();
        }

        return trips;
    }

    
}