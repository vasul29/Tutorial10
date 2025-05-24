namespace Tutorial10.Models.DTOs;

public class PrescriptionDto
{
    public PatientDto Patient { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
}