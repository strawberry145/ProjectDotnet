using Microsoft.AspNetCore.Mvc;
using ProjectIntelligence.Models;
using ProjectIntelligence.Services;

namespace ProjectIntelligence.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;
    public ProjectsController(IProjectService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? name, [FromQuery] ProjectStatus? status)
        => Ok(await _service.GetAllAsync(name, status));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _service.GetByIdAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Project p)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(p);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Project p)
    {
        if (id != p.Id) return BadRequest();
        return await _service.UpdateAsync(p) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}