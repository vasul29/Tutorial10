using Microsoft.AspNetCore.Mvc;
using Tutorial10.Models.DTOs;
using Tutorial10.Services;

namespace Tutorial10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionDto dto)
    {
        var result = await _prescriptionService.CreatePrescriptionAsync(dto);
        
        if (!result.Success)
            return BadRequest(result.ErrorMessage);
        
        return Ok(new { message = "Recepta została pomyślnie wystawiona", result.PrescriptionId });
    }
}