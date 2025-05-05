using Tutorial7.Entities;

namespace Tutorial7.Repositories.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(int clientId, CancellationToken token);
    Task<ICollection<Trip>?> GetAllClientTripsAsync(int clientId, CancellationToken token);
    Task<bool> DoesClientExistAsync(int clientId, CancellationToken token);
    Task<bool> DoesPeselExistAsync(string pesel, CancellationToken token);
    Task<Client> CreateClientAsync(Client client, CancellationToken token);
    Task<(bool success, string message)> CreateClientTripAsync(int clientId, int tripId, CancellationToken token);
    Task<(bool success, string message)> DeleteClientTripAsync(int clientId, int tripId, CancellationToken token);
}