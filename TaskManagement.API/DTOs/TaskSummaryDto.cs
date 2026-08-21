namespace TaskManagement.API.DTOs;

public class TaskSummaryDto
{
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int TaskCount { get; set; }
}
