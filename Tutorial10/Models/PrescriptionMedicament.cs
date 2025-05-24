using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial10.Models;

public class PrescriptionMedicament
{
    [Required]
    public int IdMedicament { get; set; }

    [Required]
    public int IdPrescription { get; set; }

    [Required]
    public int Dose { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; }

    [ForeignKey(nameof(IdMedicament))]
    public Medicament Medicament { get; set; }

    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; }
}