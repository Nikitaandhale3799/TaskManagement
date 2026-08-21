using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Models;

namespace TaskManagement.API.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(Models.TaskStatus? status, Models.TaskPriority? priority, string? sortBy, bool descending)
    {
        var query = _context.Tasks.AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        query = sortBy?.ToLower() switch
        {
            "title" => descending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "priority" => descending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            "status" => descending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            _ => descending ? query.OrderByDescending(t => t.CreatedDate) : query.OrderBy(t => t.CreatedDate)
        };

        var tasks = await query.ToListAsync();
        return tasks.Select(ToDto);
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        return task is null ? null : ToDto(task);
    }

    public async Task<TaskResponseDto> CreateAsync(TaskCreateDto dto)
    {
        var now = DateTime.UtcNow;

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status ?? Models.TaskStatus.ToDo,
            Priority = dto.Priority ?? Models.TaskPriority.Low,
            AssignedTo = dto.AssignedTo,
            CreatedDate = now,
            ModifiedDate = now,
            IsDeleted = false
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return ToDto(task);
    }

    public async Task<TaskResponseDto?> UpdateAsync(int id, TaskUpdateDto dto)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
            return null;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status!.Value;
        task.Priority = dto.Priority!.Value;
        task.AssignedTo = dto.AssignedTo;
        task.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ToDto(task);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
            return false;

        task.IsDeleted = true;
        task.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<TaskSummaryDto>> GetSummaryAsync()
    {
        return await _context.Database
            .SqlQuery<TaskSummaryDto>($"""
                SELECT Status, Priority, COUNT(*) AS TaskCount
                FROM Tasks
                WHERE IsDeleted = 0
                GROUP BY Status, Priority
                """)
            .ToListAsync();
    }

    private static TaskResponseDto ToDto(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        Priority = task.Priority,
        AssignedTo = task.AssignedTo,
        CreatedDate = task.CreatedDate,
        ModifiedDate = task.ModifiedDate
    };
}
