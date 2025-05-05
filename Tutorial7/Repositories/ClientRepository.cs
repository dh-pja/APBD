using Microsoft.Data.SqlClient;
using Tutorial7.Entities;
using Tutorial7.Repositories.Interfaces;

namespace Tutorial7.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly string _connectionString;
    
    public ClientRepository(IConfiguration cfg)
    {
        _connectionString = cfg.GetConnectionString("DefaultConnection");
    }
    
    public async Task<Client?> GetClientByIdAsync(int clientId, CancellationToken token)
    {
        const string query = """
                             SELECT * FROM Client
                             WHERE IdClient = @clientId
                             """;
        Client? client = null;
        
        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new(query, con);
        command.Parameters.AddWithValue("@clientId", clientId);
        await con.OpenAsync(token);
        var reader = await command.ExecuteReaderAsync(token);
        try
        {
            while (await reader.ReadAsync(token)) 
            {
                client = new Client
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Telephone = reader.GetString(4),
                    Pesel = reader.GetString(5)
                };
            }

        }
        finally
        {
            await reader.CloseAsync();
        }
        return client;
    }

    public async Task<ICollection<Trip>?> GetAllClientTripsAsync(int clientId, CancellationToken token)
    {
        bool clientExists = await DoesClientExistAsync(clientId, token);
        
        if (!clientExists)
            return null;
        
        List<Trip> trips = new List<Trip>();
        const string query = """
                       SELECT t.*, c.*, clt.RegisteredAt, clt.PaymentDate FROM Country_Trip as ct 
                          JOIN Trip as t ON ct.IdTrip = t.IdTrip
                          JOIN Country as c ON ct.IdCountry = c.IdCountry
                          JOIN Client_Trip as clt ON clt.IdTrip = t.IdTrip
                          JOIN Client as cl ON clt.IdClient = cl.IdClient
                       WHERE cl.IdClient = @clientId
                       """;
        
        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new(query, con);
        
        await con.OpenAsync(token);
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
                
                var clientTrip = new ClientTrip
                {
                    Trip = trip,
                    RegisteredAt = reader.GetInt32(8),
                    PaymentDate = reader.IsDBNull(9) ? null : reader.GetInt32(9)
                };
                trip.Participants.Add(clientTrip);
            }
        }
        finally
        {
            await reader.CloseAsync();
            await con.CloseAsync();
        }

        return trips;
    }

    public async Task<bool> DoesClientExistAsync(int clientId, CancellationToken token)
    {
        const string query = """
                             SELECT COUNT(1) FROM Client
                             WHERE IdClient = @clientId
                             """;

        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new(query, con);
        command.Parameters.AddWithValue("@clientId", clientId);
        await con.OpenAsync(token);
        bool exists = (int)await command.ExecuteScalarAsync(token) > 0;
        await con.CloseAsync();
        return exists;
    }

    public async Task<bool> DoesPeselExistAsync(string pesel, CancellationToken token)
    {
        const string query = """
                                     SELECT COUNT(1) FROM Client
                                     WHERE Pesel = @pesel
                                     """;
        
        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new(query, con);
        command.Parameters.AddWithValue("@pesel", pesel);
        await con.OpenAsync(token);
        bool exists = (int)await command.ExecuteScalarAsync(token) > 0;
        await con.CloseAsync();
        return exists;
    }

    public async Task<Client> CreateClientAsync(Client client, CancellationToken token)
    {
        const string insertQuery = """
                                   INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                                   OUTPUT INSERTED.IdClient
                                   VALUES (@firstName, @lastName, @email, @telephone, @pesel)
                                   """;

        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new(insertQuery, con);

        command.Parameters.AddWithValue("@firstName", client.FirstName);
        command.Parameters.AddWithValue("@lastName", client.LastName);
        command.Parameters.AddWithValue("@email", client.Email);
        command.Parameters.AddWithValue("@telephone", client.Telephone ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@pesel", client.Pesel ?? (object)DBNull.Value);

        await con.OpenAsync(token);
        client.Id = (int)await command.ExecuteScalarAsync(token);
        await con.CloseAsync();
        return client;
    }

    public async Task<(bool success, string message)> CreateClientTripAsync(int clientId, int tripId, CancellationToken token)
    {
        bool clientExists = await DoesClientExistAsync(clientId, token);
        
        if (!clientExists)
            return (false, "Client does not exist");
        
        const string getTripQuery = """
                                    SELECT MaxPeople FROM Trip
                                    WHERE IdTrip = @tripId
                                    """;
        int maxPeople;

        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command1 = new(getTripQuery, con);
        command1.Parameters.AddWithValue("@tripId", tripId);
        await con.OpenAsync(token);
        var reader = await command1.ExecuteReaderAsync(token);

        if (!await reader.ReadAsync(token))
        {
            return (false, "Trip does not exist");
        }
        maxPeople = reader.GetInt32(0);
        await reader.CloseAsync();
        
        const string checkParticipantsQuery = """
                                              SELECT COUNT(1) FROM Client_Trip
                                              WHERE IdTrip = @tripId
                                              """;
        await using SqlCommand command2 = new(checkParticipantsQuery, con);
        command2.Parameters.AddWithValue("@tripId", tripId);
        await con.OpenAsync(token);
        if ((int)await command2.ExecuteScalarAsync(token) >= maxPeople)
        {
            return (false, "Trip is full");
        }
        
        const string insertClientTripQuery = """
                                             INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
                                             VALUES (@clientId, @tripId, @registeredAt, NULL)
                                             """;
        
        await using SqlCommand insertCommand = new(insertClientTripQuery, con);
        insertCommand.Parameters.AddWithValue("@clientId", clientId);
        insertCommand.Parameters.AddWithValue("@tripId", tripId);
        insertCommand.Parameters.AddWithValue("@registeredAt", DateTime.Now);

        await con.OpenAsync(token);
        await insertCommand.ExecuteNonQueryAsync(token);
        await con.CloseAsync();
        return (true, "Client registered to trip successfully");
    }

    public async Task<(bool success, string message)> DeleteClientTripAsync(int clientId, int tripId, CancellationToken token)
    {
        const string checkRegistrationQuery = """
                                              SELECT Count(1) FROM Client_Trip
                                              WHERE IdClient = @clientId AND IdTrip = @tripId
                                              """;
        
        await using (SqlConnection con = new(_connectionString))
        {
            await using SqlCommand command = new(checkRegistrationQuery, con);
            command.Parameters.AddWithValue("@clientId", clientId);
            command.Parameters.AddWithValue("@tripId", tripId);
            await con.OpenAsync(token);
            var reader = await command.ExecuteReaderAsync(token);

            if (!await reader.ReadAsync(token))
            {
                return (false, "Client registration for this trip not found");
            }

            await reader.CloseAsync();
        }
        
        const string deleteQuery = """
                                   DELETE FROM Client_Trip
                                   WHERE IdClient = @clientId AND IdTrip = @tripId
                                   """;
        await using (SqlConnection con = new(_connectionString))
        {
            await using SqlCommand command = new(deleteQuery, con);
            command.Parameters.AddWithValue("@clientId", clientId);
            command.Parameters.AddWithValue("@tripId", tripId);
            await con.OpenAsync(token);
            await command.ExecuteNonQueryAsync(token);
        }

        return (true, "Client registration deleted successfully");
    }
}