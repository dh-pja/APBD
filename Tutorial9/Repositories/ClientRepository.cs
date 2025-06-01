using Microsoft.EntityFrameworkCore;
using Tutorial9.Models;
using Tutorial9.Repositories.Interfaces;

namespace Tutorial9.Repositories;

public class ClientRepository : IClientRepository
{
    private TutorialNineContext _db;
    
    public ClientRepository(TutorialNineContext db)
    {
        _db = db;
    }


    public Task<Client?> GetClientByIdAsync(int id, CancellationToken token = default)
    {
        return _db.Clients
            .FirstOrDefaultAsync(c => c.IdClient == id, token)
            .ContinueWith(task => task.Result, token);
    }

    public Task<bool> HasTripsAsync(int clientId, CancellationToken token = default)
    {
        return _db.ClientTrips
            .AnyAsync(ct => ct.IdClient == clientId, token)
            .ContinueWith(task => task.Result, token);
    }
    
    public async Task DeleteClientAsync(Client client, CancellationToken token = default)
    {
        _db.Clients.Remove(client);
        await _db.SaveChangesAsync(token);
    }

    public Task<bool> ClientExistsWithPeselAsync(string pesel, CancellationToken token = default)
    {
        return _db.Clients
            .AnyAsync(c => c.Pesel == pesel, token)
            .ContinueWith(task => task.Result, token);
    }

    public Task<bool> AddClientToTripAsync(ClientTrip clientTrip, CancellationToken token = default)
    {
        _db.ClientTrips.Add(clientTrip);
        return _db.SaveChangesAsync(token)
            .ContinueWith(task => task.Result > 0, token);
    }
}