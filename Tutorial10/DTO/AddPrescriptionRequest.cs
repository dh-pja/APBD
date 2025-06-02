using System.ComponentModel.DataAnnotations;

namespace Tutorial10.DTO;

public class AddPrescriptionRequest
{
    [Required]
    public PatientRequest Patient { get; set; }

    [Required]
    public List<MedicamentDetailsRequest> Medicaments { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public DateOnly DueDate { get; set; }
    
    public int? IdDoctor { get; set; }
}

public class PatientRequest
{
    public int IdPatient { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public DateTime Birthdate { get; set; }
}

public class MedicamentDetailsRequest
{
    [Required]
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
}