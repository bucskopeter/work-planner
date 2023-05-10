using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Data.Entities;
using WorkPlanner.Data.Interfaces;
using WorkPlanner.Domain.Interfaces;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Domain.Services;

public class WorkerService : IWorkerService
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public WorkerService(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<WorkerVm>> Get(bool includeDeleted = false)
    {
        var query = _repository.Set<WorkerEntity>();

        if (!includeDeleted)
        {
            query = query.Where(i => !i.Deleted.HasValue);
        }

        var workers = await query.ToListAsync();
        return _mapper.Map<ICollection<WorkerVm>>(workers);
    }

    public async Task<WorkerVm?> Get(Guid id)
    {
        var worker = await _repository.Find<WorkerEntity>(id);
        return _mapper.Map<WorkerVm>(worker);
    }

    public async Task<Guid> Create(WorkerCreateVm vm)
    {
        var existingWorker = await _repository.Set<WorkerEntity>()
            .AsNoTracking()
            .AnyAsync(i => i.FirstName == vm.FirstName && i.LastName == vm.LastName);

        if (existingWorker)
            throw new EntityDuplicateException("A worker with the specified name already exists.");

        var worker = _mapper.Map<WorkerEntity>(vm);
        _repository.Add(worker);
        await _repository.SaveChangesAsync();

        return worker.Id;
    }

    public async Task Update(Guid id, WorkerCreateVm vm)
    {
        var worker = await _repository.Set<WorkerEntity>().FirstOrDefaultAsync(i => i.Id == id && !i.Deleted.HasValue);

        if (worker == null)
            throw new EntityNotFoundException("Worker not found.");

        var existingNames = await _repository.Set<WorkerEntity>()
            .AnyAsync(i => i.FirstName == vm.FirstName && i.LastName == vm.LastName && i.Id != id);

        if (existingNames)
            throw new EntityDuplicateException("A worker with the specified name already exists.");

        _mapper.Map(vm, worker);
        await _repository.SaveChangesAsync();
    }

    public async Task Deactivate(Guid id)
    {
        var worker = await _repository.Set<WorkerEntity>().FirstOrDefaultAsync(i => i.Id == id);

        if (worker == null)
            throw new EntityNotFoundException("Worker not found.");

        if (worker.Deleted.HasValue)
            return;

        _repository.SoftDelete(worker);
        await _repository.SaveChangesAsync();
    }

    public async Task Reactivate(Guid id)
    {
        var worker = await _repository.Set<WorkerEntity>().FirstOrDefaultAsync(i => i.Id == id);

        if (worker == null)
            throw new EntityNotFoundException("Worker not found.");

        if (!worker.Deleted.HasValue)
            return;

        worker.Deleted = null;
        await _repository.SaveChangesAsync();
    }
}