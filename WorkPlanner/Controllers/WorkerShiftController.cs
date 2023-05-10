using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Domain.Interfaces;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Controllers;

[ApiController]
[Route("api/worker/{id:guid}/shift")]
public class WorkerShiftController : ControllerBase
{
    private readonly IWorkerShiftService _workerShiftService;

    public WorkerShiftController(IWorkerShiftService workerShiftService)
    {
        _workerShiftService = workerShiftService;
    }

    [HttpGet]
    public Task<ICollection<ShiftVm>> Get(Guid id)
    {
        return _workerShiftService.Get(id);
    }

    [HttpPost]
    public async Task<ActionResult> Log(Guid id, [FromBody] ShiftCreateVm vm)
    {
        try
        {
            await _workerShiftService.Log(id, vm);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Conflict(ex.Message);
        }

        return NoContent();
    }

    [HttpPut("{shiftId:int}")]
    public async Task<ActionResult> Update(Guid id, int shiftId, [FromBody] ShiftCreateVm vm)
    {
        try
        {
            await _workerShiftService.Update(id, shiftId, vm);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Conflict(ex.Message);
        }

        return NoContent();
    }

    [HttpDelete("{shiftId:int}")]
    public async Task<ActionResult> Delete(Guid id, int shiftId)
    {
        try
        {
            await _workerShiftService.Delete(id, shiftId);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return NoContent();
    }
}