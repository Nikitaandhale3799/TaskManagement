using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.DTOs;
using TaskManagement.API.Models;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] Models.TaskStatus? status,
        [FromQuery] Models.TaskPriority? priority,
        [FromQuery] string? sortBy,
        [FromQuery] bool descending = false)
    {
        var tasks = await _taskService.GetAllAsync(status, priority, sortBy, descending);
        return Ok(tasks);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<IEnumerable<TaskSummaryDto>>> GetSummary()
    {
        var summary = await _taskService.GetSummaryAsync();
        return Ok(summary);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create([FromBody] TaskCreateDto dto)
    {
        var created = await _taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> Update(int id, [FromBody] TaskUpdateDto dto)
    {
        var updated = await _taskService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.SoftDeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
