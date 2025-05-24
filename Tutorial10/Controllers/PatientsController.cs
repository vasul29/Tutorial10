using Microsoft.AspNetCore.Mvc;
using Tutorial10.Models.DTOs;
using Tutorial10.Services;

namespace Tutorial10.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("{idPatient}")]
        public async Task<IActionResult> GetPatientDetails(int idPatient)
        {
            var patientDetails = await _patientService.GetPatientDetailsAsync(idPatient);
            if (patientDetails == null)
                return NotFound($"Pacjent o ID {idPatient} nie został znaleziony.");

            return Ok(patientDetails);
        }
    }
}