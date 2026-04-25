using Microsoft.AspNetCore.Mvc;
using ProjectIntelligence.Models;
using ProjectIntelligence.Services;

namespace ProjectIntelligence.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;
    public TasksController(ITaskService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int? projectId, [FromQuery] Models.TaskStatus? status,
        [FromQuery] int? memberId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        => Ok(await _service.GetAllAsync(projectId, status, memberId, from, to));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var t = await _service.GetByIdAsync(id);
        return t == null ? NotFound() : Ok(t);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectTask t)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(t);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectTask t)
    {
        if (id != t.Id) return BadRequest();
        return await _service.UpdateAsync(t) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}