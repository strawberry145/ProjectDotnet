using Microsoft.AspNetCore.Mvc;
using ProjectIntelligence.Models;
using ProjectIntelligence.Services;

namespace ProjectIntelligence.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _service;
    public MembersController(IMemberService service) => _service = service;

    [HttpGet] public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var m = await _service.GetByIdAsync(id);
        return m == null ? NotFound() : Ok(m);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Member m)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(m);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Member m)
    {
        if (id != m.Id) return BadRequest();
        return await _service.UpdateAsync(m) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}