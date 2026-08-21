using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Models;

namespace TaskManagement.API.DTOs;

public class TaskCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public Models.TaskStatus? Status { get; set; }

    public Models.TaskPriority? Priority { get; set; }

    [MaxLength(100)]
    public string? AssignedTo { get; set; }
}
