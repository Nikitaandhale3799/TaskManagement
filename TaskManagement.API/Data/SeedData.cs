using TaskManagement.API.Models;

namespace TaskManagement.API.Data;

public static class SeedData
{
    public static TaskItem[] Tasks => new[]
    {
        new TaskItem
        {
            Id = 1,
            Title = "Set up CI/CD pipeline",
            Description = "Configure GitHub Actions for build and test automation on every pull request.",
            Status = Models.TaskStatus.Done,
            Priority = Models.TaskPriority.High,
            AssignedTo = "Priya Sharma",
            CreatedDate = new DateTime(2026, 6, 10, 9, 0, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 6, 18, 14, 30, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 2,
            Title = "Design database schema",
            Description = "Model the core entities and relationships for the task management system.",
            Status = Models.TaskStatus.Done,
            Priority = Models.TaskPriority.Critical,
            AssignedTo = "Marcus Chen",
            CreatedDate = new DateTime(2026, 6, 12, 10, 15, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 6, 20, 16, 0, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 3,
            Title = "Implement authentication middleware",
            Description = "Add JWT-based authentication to protect API endpoints.",
            Status = Models.TaskStatus.InProgress,
            Priority = Models.TaskPriority.High,
            AssignedTo = "Marcus Chen",
            CreatedDate = new DateTime(2026, 7, 1, 8, 45, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 5, 11, 20, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 4,
            Title = "Build task filtering API",
            Description = "Support filtering tasks by status and priority via query parameters.",
            Status = Models.TaskStatus.Done,
            Priority = Models.TaskPriority.Medium,
            AssignedTo = "Priya Sharma",
            CreatedDate = new DateTime(2026, 7, 3, 9, 30, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 7, 15, 13, 0, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 5,
            Title = "Create React task list UI",
            Description = "Display tasks in a table with sortable columns.",
            Status = Models.TaskStatus.InProgress,
            Priority = Models.TaskPriority.Medium,
            AssignedTo = "Elena Rodriguez",
            CreatedDate = new DateTime(2026, 7, 10, 10, 0, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 10, 15, 45, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 6,
            Title = "Write unit tests for TaskService",
            Description = "Cover filtering, sorting, and soft delete logic.",
            Status = Models.TaskStatus.ToDo,
            Priority = Models.TaskPriority.Medium,
            AssignedTo = "Sam O'Neill",
            CreatedDate = new DateTime(2026, 7, 20, 9, 0, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 7, 20, 9, 0, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 7,
            Title = "Add global exception handling",
            Description = "Return consistent error responses across all endpoints.",
            Status = Models.TaskStatus.Done,
            Priority = Models.TaskPriority.High,
            AssignedTo = "Marcus Chen",
            CreatedDate = new DateTime(2026, 7, 22, 11, 15, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 1, 9, 30, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 8,
            Title = "Fix priority badge colors",
            Description = "Priority badges do not match the design spec on the task cards.",
            Status = Models.TaskStatus.ToDo,
            Priority = Models.TaskPriority.Low,
            AssignedTo = "Elena Rodriguez",
            CreatedDate = new DateTime(2026, 8, 2, 13, 40, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 2, 13, 40, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 9,
            Title = "Optimize summary query performance",
            Description = "Investigate slow response times on the task summary endpoint under load.",
            Status = Models.TaskStatus.InProgress,
            Priority = Models.TaskPriority.Critical,
            AssignedTo = "Priya Sharma",
            CreatedDate = new DateTime(2026, 8, 5, 8, 0, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 15, 17, 10, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 10,
            Title = "Set up Docker Compose for local dev",
            Description = "Bundle the API, frontend, and SQL Server into a single Compose file.",
            Status = Models.TaskStatus.ToDo,
            Priority = Models.TaskPriority.Medium,
            AssignedTo = "Sam O'Neill",
            CreatedDate = new DateTime(2026, 8, 8, 10, 30, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 8, 10, 30, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 11,
            Title = "Review API documentation",
            Description = "Ensure Swagger annotations accurately describe all endpoints.",
            Status = Models.TaskStatus.ToDo,
            Priority = Models.TaskPriority.Low,
            AssignedTo = "Elena Rodriguez",
            CreatedDate = new DateTime(2026, 8, 12, 9, 20, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 12, 9, 20, 0, DateTimeKind.Utc),
            IsDeleted = false
        },
        new TaskItem
        {
            Id = 12,
            Title = "Plan sprint retrospective",
            Description = "Gather feedback on the last two-week sprint and identify improvements.",
            Status = Models.TaskStatus.Done,
            Priority = Models.TaskPriority.Low,
            AssignedTo = "Marcus Chen",
            CreatedDate = new DateTime(2026, 8, 14, 14, 0, 0, DateTimeKind.Utc),
            ModifiedDate = new DateTime(2026, 8, 16, 10, 5, 0, DateTimeKind.Utc),
            IsDeleted = false
        }
    };
}
