using Microsoft.AspNetCore.Mvc;
using Tutorial10.DTO;
using Tutorial10.Mappers;
using Tutorial10.Repositories.Interfaces;

namespace Tutorial10.Controllers;

[ApiController]
public class PrescriptionController : ControllerBase
{
    private IPatientRepository _patientRepository;
    private IPrescriptionRepository _prescriptionRepository;
    
    public PrescriptionController(IPatientRepository patientRepository, IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
        _patientRepository = patientRepository;
    }
    
    [HttpPost]
    [Route("api/prescriptions")]
    public async Task<IActionResult> CreatePrescriptionAsync([FromBody] AddPrescriptionRequest prescription, CancellationToken token = default)
    {
        bool patientExists = await _patientRepository.PatientExistsAsync(prescription.Patient.IdPatient, token);
        
        if (!patientExists)
        {
            await _patientRepository.AddPatientAsync(prescription.Patient.ToModel(), token);
        }

        foreach (var medicament in prescription.Medicaments)
        {
            bool medicationExists = await _patientRepository.MedicationExistsAsync(medicament.IdMedicament, token);
            if (!medicationExists)
            {
                return BadRequest($"Medication with ID {medicament.IdMedicament} does not exist.");
            }
        }
        
        if (prescription.Medicaments.Count > 10)
        {
            return BadRequest("Prescription cannot contain more than 10 medicaments.");
        }
        
        if (prescription.DueDate < DateOnly.FromDateTime(DateTime.Now))
        {
            return BadRequest("Due date cannot be in the past.");
        }
        
        var prescriptionModel = prescription.ToModel();
        
        await _prescriptionRepository.AddPrescriptionAsync(prescriptionModel, token);
        
        return Ok(prescription);
    }
}