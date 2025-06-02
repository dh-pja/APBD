using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial10.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    public DateOnly Date { get; set; }

    public DateOnly DueDate { get; set; }

    [ForeignKey("Patient")]
    public int IdPatient { get; set; }

    [ForeignKey("Doctor")]
    public int IdDoctor { get; set; }

    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}