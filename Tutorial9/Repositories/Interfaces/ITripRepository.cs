using Tutorial9.Models;

namespace Tutorial9.Repositories.Interfaces;

public interface ITripRepository
{
    Task<int> CountTripsAsync(CancellationToken token = default);
    Task<IEnumerable<Trip>> GetAllTripsAsyncPaged(int page, int pageSize, CancellationToken token = default);
    Task<Trip?> GetTripByIdAsync(int id, CancellationToken token = default);
}