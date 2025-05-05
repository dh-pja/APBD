using Tutorial7.Entities;

namespace Tutorial7.Repositories.Interfaces;

public interface ITripRepository
{
    Task<ICollection<Trip>> GetAllTripsAsync(CancellationToken token);
}