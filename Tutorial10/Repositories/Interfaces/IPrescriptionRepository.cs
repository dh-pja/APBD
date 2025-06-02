using Tutorial10.Models;

namespace Tutorial10.Repositories.Interfaces;

public interface IPrescriptionRepository
{
    Task<int> AddPrescriptionAsync(Prescription prescription, CancellationToken token = default);
}