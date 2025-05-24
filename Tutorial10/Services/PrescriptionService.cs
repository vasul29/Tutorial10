using Microsoft.EntityFrameworkCore;
using Tutorial10.DAL;
using Tutorial10.Models;
using Tutorial10.Models.DTOs;

namespace Tutorial10.Services;

public interface IPrescriptionService
{
    Task<(bool Success, string ErrorMessage, int? PrescriptionId)> CreatePrescriptionAsync(PrescriptionDto dto);
}

public class PrescriptionService : IPrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string ErrorMessage, int? PrescriptionId)> CreatePrescriptionAsync(PrescriptionDto dto)
    {
        if (dto.Medicaments.Count > 10)
            return (false, "Recepta nie może zawierać więcej niż 10 leków.", null);
        
        if (dto.DueDate < dto.Date)
            return (false, "Data realizacji nie może być wcześniejsza niż data wystawienia.", null);
        
        var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
        if (doctor == null)
            return (false, $"Lekarz o ID {dto.IdDoctor} nie został znaleziony.", null);
        
        var patient = await _context.Patients.FindAsync(dto.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                BirthDate = dto.Patient.BirthDate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }
        
        var medicamentIds = dto.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicamentIds = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();

        var missing = medicamentIds.Except(existingMedicamentIds).ToList();
        if (missing.Any())
            return (false, $"Nie znaleziono leków o ID: {string.Join(", ", missing)}", null);
        
        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdDoctor = dto.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = dto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Description = m.Description
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return (true, null, prescription.IdPrescription);
    }
}