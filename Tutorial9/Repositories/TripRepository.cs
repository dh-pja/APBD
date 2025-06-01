using Microsoft.EntityFrameworkCore;
using Tutorial9.Models;
using Tutorial9.Repositories.Interfaces;

namespace Tutorial9.Repositories;

public class TripRepository : ITripRepository
{
    private TutorialNineContext _db;
    
    public TripRepository(TutorialNineContext db)
    {
        _db = db;
    }

    public Task<int> CountTripsAsync(CancellationToken token = default)
    {
        return _db.Trips.CountAsync(token);
    }

    public Task<IEnumerable<Trip>> GetAllTripsAsyncPaged(int page, int pageSize, CancellationToken token = default)
    {
        return _db.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token)
            .ContinueWith(task => (IEnumerable<Trip>)task.Result, token);
    }

    public Task<Trip?> GetTripByIdAsync(int id, CancellationToken token = default)
    {
        return _db.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClient)
            .FirstOrDefaultAsync(t => t.IdTrip == id, token)
            .ContinueWith(task => task.Result, token);
    }
}