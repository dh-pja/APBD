using System.ComponentModel.DataAnnotations;

namespace Tutorial10.Models;

public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }

    public int IdPrescription { get; set; }

    public int? Dose { get; set; }

    [MaxLength(100)]
    public string Details { get; set; }
}