using Tutorial10.DAL;
using Tutorial10.Models;
using Tutorial10.Repositories.Interfaces;

namespace Tutorial10.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private TutTenContext _db;
    
    public PrescriptionRepository(TutTenContext db)
    {
        _db = db;
    }
    
    public async Task<int> AddPrescriptionAsync(Prescription prescription, CancellationToken token = default)
    {
        _db.Prescriptions.Add(prescription);
        
        var entries =  await _db.SaveChangesAsync(token);
        
        foreach (var medicament in prescription.PrescriptionMedicaments)
        {
            _db.PrescriptionMedicaments.Add(medicament);
        }
        
        await _db.SaveChangesAsync(token);
        return entries;
    }
}