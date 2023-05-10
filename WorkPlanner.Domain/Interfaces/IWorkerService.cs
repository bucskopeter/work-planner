using WorkPlanner.Domain.Models;

namespace WorkPlanner.Domain.Interfaces;

public interface IWorkerService
{
    Task<ICollection<WorkerVm>> Get(bool includeDeleted = false);
    Task<WorkerVm?> Get(Guid id);
    Task<Guid> Create(WorkerCreateVm vm);
    Task Update(Guid id, WorkerCreateVm vm);
    Task Deactivate(Guid id);
    Task Reactivate(Guid id);
}