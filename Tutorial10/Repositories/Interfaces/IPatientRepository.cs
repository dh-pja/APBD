using Tutorial10.Models;

namespace Tutorial10.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<bool> PatientExistsAsync(int id, CancellationToken token = default);
    Task<int> AddPatientAsync(Patient patient, CancellationToken token = default);
    Task<bool> MedicationExistsAsync(int id, CancellationToken token = default);
}