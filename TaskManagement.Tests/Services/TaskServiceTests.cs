using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Services;
using Xunit;
using TaskStatus = TaskManagement.API.Models.TaskStatus;
using TaskPriority = TaskManagement.API.Models.TaskPriority;

namespace TaskManagement.Tests.Services;

public class TaskServiceTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateAsync_WithNoStatusOrPriority_DefaultsToToDoAndLow()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);

        var result = await service.CreateAsync(new TaskCreateDto { Title = "New task" });

        Assert.Equal(TaskStatus.ToDo, result.Status);
        Assert.Equal(TaskPriority.Low, result.Priority);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateAsync_WithExplicitLowPriority_PersistsLowNotOverridden()
    {
        // Regression test: an earlier EF Core default-value configuration treated Low
        // (the enum's CLR default, 0) as "unset" and silently substituted the DB default
        // instead. Priority no longer has a DB-level default, so an explicit Low must
        // round-trip exactly as given.
        await using var context = CreateContext();
        var service = new TaskService(context);

        var result = await service.CreateAsync(new TaskCreateDto
        {
            Title = "Low priority task",
            Priority = TaskPriority.Low
        });

        Assert.Equal(TaskPriority.Low, result.Priority);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateAsync_SetsCreatedAndModifiedDateToSameTimestamp()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);

        var result = await service.CreateAsync(new TaskCreateDto { Title = "Timestamped task" });

        Assert.Equal(result.CreatedDate, result.ModifiedDate);
        Assert.True(result.CreatedDate <= DateTime.UtcNow);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllAsync_FiltersByStatus()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);
        await service.CreateAsync(new TaskCreateDto { Title = "Todo task", Status = TaskStatus.ToDo });
        await service.CreateAsync(new TaskCreateDto { Title = "Done task", Status = TaskStatus.Done });

        var result = await service.GetAllAsync(TaskStatus.Done, null, null, false);

        var task = Assert.Single(result);
        Assert.Equal("Done task", task.Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllAsync_FiltersByStatusAndPriorityCombined()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);
        await service.CreateAsync(new TaskCreateDto { Title = "Match", Status = TaskStatus.InProgress, Priority = TaskPriority.Critical });
        await service.CreateAsync(new TaskCreateDto { Title = "Wrong priority", Status = TaskStatus.InProgress, Priority = TaskPriority.Low });
        await service.CreateAsync(new TaskCreateDto { Title = "Wrong status", Status = TaskStatus.Done, Priority = TaskPriority.Critical });

        var result = await service.GetAllAsync(TaskStatus.InProgress, TaskPriority.Critical, null, false);

        var task = Assert.Single(result);
        Assert.Equal("Match", task.Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllAsync_ExcludesSoftDeletedTasks()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);
        var created = await service.CreateAsync(new TaskCreateDto { Title = "To be deleted" });
        await service.SoftDeleteAsync(created.Id);

        var result = await service.GetAllAsync(null, null, null, false);

        Assert.Empty(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ReturnsNullForSoftDeletedTask()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);
        var created = await service.CreateAsync(new TaskCreateDto { Title = "To be deleted" });
        await service.SoftDeleteAsync(created.Id);

        var result = await service.GetByIdAsync(created.Id);

        Assert.Null(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ReturnsNullForNonexistentId()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);

        var result = await service.GetByIdAsync(9999);

        Assert.Null(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateAsync_UpdatesFieldsAndBumpsModifiedDate()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);
        var created = await service.CreateAsync(new TaskCreateDto { Title = "Original title" });

        var updated = await service.UpdateAsync(created.Id, new TaskUpdateDto
        {
            Title = "Updated title",
            Status = TaskStatus.Done,
            Priority = TaskPriority.High
        });

        Assert.NotNull(updated);
        Assert.Equal("Updated title", updated!.Title);
        Assert.Equal(TaskStatus.Done, updated.Status);
        Assert.Equal(TaskPriority.High, updated.Priority);
        Assert.True(updated.ModifiedDate >= updated.CreatedDate);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateAsync_ReturnsNullForNonexistentId()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);

        var result = await service.UpdateAsync(9999, new TaskUpdateDto
        {
            Title = "Doesn't matter",
            Status = TaskStatus.ToDo,
            Priority = TaskPriority.Low
        });

        Assert.Null(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task SoftDeleteAsync_SetsIsDeletedAndReturnsTrue()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);
        var created = await service.CreateAsync(new TaskCreateDto { Title = "To delete" });

        var result = await service.SoftDeleteAsync(created.Id);

        Assert.True(result);
        var entity = await context.Tasks.IgnoreQueryFilters().FirstAsync(t => t.Id == created.Id);
        Assert.True(entity.IsDeleted);
    }

    [Fact]
    public async System.Threading.Tasks.Task SoftDeleteAsync_ReturnsFalseWhenAlreadyDeleted()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);
        var created = await service.CreateAsync(new TaskCreateDto { Title = "To delete twice" });
        await service.SoftDeleteAsync(created.Id);

        var result = await service.SoftDeleteAsync(created.Id);

        Assert.False(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task SoftDeleteAsync_ReturnsFalseForNonexistentId()
    {
        await using var context = CreateContext();
        var service = new TaskService(context);

        var result = await service.SoftDeleteAsync(9999);

        Assert.False(result);
    }
}
