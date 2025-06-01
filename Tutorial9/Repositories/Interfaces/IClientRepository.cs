using Tutorial9.Models;

namespace Tutorial9.Repositories.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(int id, CancellationToken token = default);
    Task<bool> HasTripsAsync(int clientId, CancellationToken token = default);
    Task DeleteClientAsync(Client client, CancellationToken token = default);
    Task<bool> ClientExistsWithPeselAsync(string pesel, CancellationToken token = default);
    Task<bool> AddClientToTripAsync(ClientTrip clientTrip, CancellationToken token = default);
}