using WorkPlanner.Domain.Models;

namespace WorkPlanner.Domain.Interfaces;

public interface IWorkerShiftService
{
    Task<ICollection<ShiftVm>> Get(Guid workerId);
    Task Log(Guid workerId, ShiftCreateVm vm);
    Task Update(Guid workerId, int shiftId, ShiftCreateVm vm);
    Task Delete(Guid workerId, int shiftId);
}