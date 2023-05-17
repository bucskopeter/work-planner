using Microsoft.EntityFrameworkCore;
using Moq;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Data.Entities;
using WorkPlanner.Data.Repositories;
using WorkPlanner.Domain.Models;
using WorkPlanner.Domain.Services;

namespace WorkPlanner.Tests;

public class WorkerServiceTests : TestBase
{
    private List<WorkerEntity>? _workers;

    [Theory]
    [InlineData(1, null)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public async Task Get_All_Returns_Expected(int expectedCount, bool? includeDeleted)
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        // Act
        var result = includeDeleted.HasValue ? await service.Get(includeDeleted.Value) : await service.Get();

        // Assert
        Assert.Equal(expectedCount, result.Count);
    }

    [Fact]
    public async Task Get_Returns_Null()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        // Act
        var result = await service.Get(Guid.Empty);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Get_Returns_Expected()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        var expectedGuid = _workers!.First().Id;

        // Act
        var result = await service.Get(expectedGuid);

        // Assert
        Assert.Equal(expectedGuid, result!.Id);
    }

    [Fact]
    public async Task Create_Existing_Worker_Throws()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        var worker = new WorkerCreateVm
        {
            EmailAddress = _workers!.First().EmailAddress
        };

        // Act
        var result = service.Create(worker);

        // Assert
        await Assert.ThrowsAsync<EntityDuplicateException>(() => result);
    }

    [Fact]
    public async Task Create_Successfully()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        var worker = new WorkerCreateVm
        {
            FirstName = "First3",
            LastName = "Last3",
            EmailAddress = "email3@email.com"
        };

        // Act
        var result = await service.Create(worker);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Update_Not_Found_Worker_Throws()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        // Act
        var result = service.Update(Guid.Empty, It.IsAny<WorkerCreateVm>());

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    [Fact]
    public async Task Update_Deleted_Worker_Throws()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);
        var deletedWorkerId = _workers!.First(i => i.Deleted.HasValue).Id;
        
        // Act
        var result = service.Update(deletedWorkerId, It.IsAny<WorkerCreateVm>());

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    [Fact]
    public async Task Update_Duplicate_Worker_Throws()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        var workerId = _workers!.First(i => !i.Deleted.HasValue).Id;
        var existingWorkerEmail = _workers!.First(i => i.Id != workerId).EmailAddress;
        var worker = new WorkerCreateVm
        {
            EmailAddress = existingWorkerEmail
        };
        
        // Act
        var result = service.Update(workerId, worker);

        // Assert
        await Assert.ThrowsAsync<EntityDuplicateException>(() => result);
    }
    
    [Fact]
    public async Task Deactivate_Not_Found_Worker_Throws()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        // Act
        var result = service.Deactivate(Guid.Empty);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    [Fact]
    public async Task Reactivate_Not_Found_Worker_Throws()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerService(new Repository(context), Mapper);

        await SeedWorkers(context);

        // Act
        var result = service.Reactivate(Guid.Empty);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    private async Task SeedWorkers(DbContext context)
    {
        // the list should contain both deleted and not deleted workers
        _workers = new List<WorkerEntity>
        {
            new()
            {
                FirstName = "First",
                LastName = "Last",
                EmailAddress = "email@email.com"
            },
            new()
            {
                FirstName = "First2",
                LastName = "Last2",
                EmailAddress = "email2@email.com",
                Deleted = new DateTime()
            }
        };

        context.AddRange(_workers);
        await context.SaveChangesAsync();
    }
}