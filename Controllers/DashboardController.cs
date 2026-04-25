using Microsoft.AspNetCore.Mvc;
using ProjectIntelligence.Services;

namespace ProjectIntelligence.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;
    public DashboardController(IDashboardService service) => _service = service;

    [HttpGet("health")] public async Task<IActionResult> Health() => Ok(await _service.GetProjectHealthAsync());
    [HttpGet("performance")] public async Task<IActionResult> Performance() => Ok(await _service.GetMemberPerformanceAsync());
    [HttpGet("chart")] public async Task<IActionResult> Chart() => Ok(await _service.GetProjectChartDataAsync());
}