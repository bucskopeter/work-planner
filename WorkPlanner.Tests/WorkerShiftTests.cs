using Microsoft.EntityFrameworkCore;
using Moq;
using WorkPlanner.Core.Entities;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Data.Entities;
using WorkPlanner.Data.Repositories;
using WorkPlanner.Domain.Models;
using WorkPlanner.Domain.Services;

namespace WorkPlanner.Tests;

public class WorkerShiftTests : TestBase
{
    private List<WorkerEntity>? _workers;

    [Fact]
    public async Task Get_Not_Found_Worker_Returns_Empty()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);
        
        // Act
        var result = await service.Get(Guid.Empty);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Get_Returns_Expected()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        var workerId = _workers!.First().Id;

        // Act
        var result = await service.Get(workerId);

        // Assert
        Assert.Equal(_workers!.First(i => i.Id == workerId).Shifts.Count, result.Count);
    }

    [Fact]
    public async Task Log_Not_Found_Worker_Throw()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        // Act
        var result = service.Log(Guid.Empty, It.IsAny<ShiftCreateVm>());

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    [Fact]
    public async Task Log_Worker_Has_Same_Day_Shift_Throw()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        var worker = _workers!.First();
        var workerShift = worker.Shifts.First();

        var newShiftVm = new ShiftCreateVm
        {
            Date = workerShift.Date
        };

        // Act
        var result = service.Log(worker.Id, newShiftVm);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleViolationException>(() => result);
    }

    [Fact]
    public async Task Update_Not_Found_Worker_Throw()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        // Act
        var result = service.Update(Guid.Empty, It.IsAny<int>(), It.IsAny<ShiftCreateVm>());

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    [Fact]
    public async Task Update_Not_Found_Shift_Throw()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        var workerId = _workers!.First().Id;

        // Act
        var result = service.Update(workerId, It.IsAny<int>(), It.IsAny<ShiftCreateVm>());

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    [Fact]
    public async Task Update_Same_Day_Shift_Throw()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        var worker = _workers!.First();
        var shift = worker.Shifts.First();
        var otherShiftDate = worker.Shifts.First(i => i.Id != shift.Id).Date;

        var newVm = new ShiftCreateVm
        {
            Date = otherShiftDate
        };

        // Act
        var result = service.Update(worker.Id, shift.Id, newVm);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleViolationException>(() => result);
    }

    [Fact]
    public async Task Delete_Not_Found_Worker_Throw()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        // Act
        var result = service.Delete(Guid.Empty, It.IsAny<int>());

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    [Fact]
    public async Task Delete_Not_Found_Shift_Throw()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new WorkerShiftService(new Repository(context), Mapper);

        await SeedWorkersWithShifts(context);

        var workerId = _workers!.First().Id;

        // Act
        var result = service.Delete(workerId, It.IsAny<int>());

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
    }

    private async Task SeedWorkersWithShifts(DbContext context)
    {
        // the list should contain both deleted and not deleted workers
        _workers = new List<WorkerEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "First",
                LastName = "Last",
                EmailAddress = "email@email.com",
                Shifts = new List<ShiftEntity>
                {
                    new()
                    {
                        Date = new DateTime(),
                        ShiftNumber = ShiftNumber.Shift1,

                    },
                    new()
                    {
                        Date = new DateTime().AddDays(1),
                        ShiftNumber = ShiftNumber.Shift2,
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "First2",
                LastName = "Last2",
                EmailAddress = "email2@email.com",
                Shifts = new List<ShiftEntity>
                {
                    new()
                    {
                        Date = new DateTime(),
                        ShiftNumber = ShiftNumber.Shift3,
                    },
                    new()
                    {
                        Date = new DateTime().AddDays(1),
                        ShiftNumber = ShiftNumber.Shift4,
                    }
                }
            }
        };

        context.AddRange(_workers);
        await context.SaveChangesAsync();
    }
}