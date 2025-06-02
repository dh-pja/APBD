using Tutorial10.DTO;
using Tutorial10.Models;

namespace Tutorial10.Mappers;

public static class PrescriptionMapper
{
    public static Prescription ToModel(this AddPrescriptionRequest request)
    {
        return new Prescription
        {
            DueDate = request.DueDate,
            Date = request.Date,
            IdPatient = request.Patient.IdPatient,
            IdDoctor = request.IdDoctor ?? 0,
            PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose ?? 1,
            }).ToList()
        };
    }
}
