using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Data.Entities;
using WorkPlanner.Data.Interfaces;
using WorkPlanner.Domain.Interfaces;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Domain.Services;

public class WorkerShiftService : IWorkerShiftService
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public WorkerShiftService(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<ShiftVm>> Get(Guid workerId)
    {
        var shifts = await _repository.Set<ShiftEntity>()
            .Where(i => i.WorkerId == workerId)
            .ToListAsync();

        return _mapper.Map<ICollection<ShiftVm>>(shifts);
    }

    public async Task Log(Guid workerId, ShiftCreateVm vm)
    {
        var worker = await GetWorkerWithShifts(workerId);

        if (worker.Shifts.Any(i => i.Date.Date == vm.Date.Date))
        {
            throw new BusinessRuleViolationException("The worker already has a shift for the specified day.");
        }

        var shift = _mapper.Map<ShiftEntity>(vm);
        shift.WorkerId = workerId;

        _repository.Add(shift);
        await _repository.SaveChangesAsync();
    }

    public async Task Update(Guid workerId, int shiftId, ShiftCreateVm vm)
    {
        var worker = await GetWorkerWithShifts(workerId);
        var shift = GetShift(shiftId, worker);

        if (shift.Date != vm.Date)
        {
            if (worker.Shifts.Any(i => i.Date.Date == vm.Date.Date))
            {
                throw new BusinessRuleViolationException("On the specified day a shift already exists.");
            }
        }

        _mapper.Map(vm, shift);
        await _repository.SaveChangesAsync();
    }
    
    public async Task Delete(Guid workerId, int shiftId)
    {
        var worker = await GetWorkerWithShifts(workerId);
        var shift = GetShift(shiftId, worker);

        _repository.Delete(shift);
        await _repository.SaveChangesAsync();
    }
    
    private async Task<WorkerEntity> GetWorkerWithShifts(Guid workerId)
    {
        var worker = await _repository.Set<WorkerEntity>()
            .Include(i => i.Shifts)
            .FirstOrDefaultAsync(i => i.Id == workerId);

        return worker ?? throw new EntityNotFoundException("Worker not found.");
    }

    private static ShiftEntity GetShift(int shiftId, WorkerEntity worker)
    {
        var shift = worker.Shifts.FirstOrDefault(i => i.Id == shiftId);

        return shift ?? throw new EntityNotFoundException("Shift not found.");
    }
}