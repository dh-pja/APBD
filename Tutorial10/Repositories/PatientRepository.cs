using Microsoft.EntityFrameworkCore;
using Tutorial10.DAL;
using Tutorial10.Models;
using Tutorial10.Repositories.Interfaces;

namespace Tutorial10.Repositories;

public class PatientRepository : IPatientRepository
{
    private TutTenContext _db;
    
    public PatientRepository(TutTenContext db)
    {
        _db = db;
    }

    public Task<bool> PatientExistsAsync(int id, CancellationToken token = default)
    {
        return _db.Patients.AnyAsync(p => p.IdPatient == id, token);
    }

    public Task<int> AddPatientAsync(Patient patient, CancellationToken token = default)
    {
        _db.Patients.Add(patient);
        return _db.SaveChangesAsync(token);
    }

    public Task<bool> MedicationExistsAsync(int id, CancellationToken token = default)
    {
        return _db.Medicaments.AnyAsync(m => m.IdMedicament == id, token);
    }
}