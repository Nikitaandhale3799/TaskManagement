using TaskManagement.API.DTOs;
using TaskManagement.API.Models;

namespace TaskManagement.API.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetAllAsync(Models.TaskStatus? status, Models.TaskPriority? priority, string? sortBy, bool descending);
    Task<TaskResponseDto?> GetByIdAsync(int id);
    Task<TaskResponseDto> CreateAsync(TaskCreateDto dto);
    Task<TaskResponseDto?> UpdateAsync(int id, TaskUpdateDto dto);
    Task<bool> SoftDeleteAsync(int id);
    Task<IEnumerable<TaskSummaryDto>> GetSummaryAsync();
}
