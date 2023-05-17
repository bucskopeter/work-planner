using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Domain.Interfaces;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Controllers;

/// <summary>
/// Controller responsible for managing worker shifts.
/// </summary>
[ApiController]
[Route("api/worker/{id:guid}/shift")]
public class WorkerShiftController : ControllerBase
{
    private readonly IWorkerShiftService _workerShiftService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkerShiftController"/> class.
    /// </summary>
    /// <param name="workerShiftService"></param>
    public WorkerShiftController(IWorkerShiftService workerShiftService)
    {
        _workerShiftService = workerShiftService;
    }

    /// <summary>
    /// Get all the shifts for a worker.
    /// </summary>
    /// <param name="id">The worker ID.</param>
    /// <response code="200">A list of all the shifts for the worker.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ICollection<ShiftVm>> Get(Guid id)
    {
        return _workerShiftService.Get(id);
    }

    /// <summary>
    /// Log a new shift detail for a worker.
    /// </summary>
    /// <param name="id">The worker ID.</param>
    /// <param name="vm">The shift details.</param>
    /// <response code="204">The worker shift was successfully created.</response>
    /// <response code="400">The provided data was invalid.</response>
    /// <response code="404">The worker was not found.</response>
    /// <response code="409">The worker already has a shift for the specified day.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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

    /// <summary>
    /// Update an existing worker shift.
    /// </summary>
    /// <param name="id">The worker ID.</param>
    /// <param name="shiftId">The shift ID.</param>
    /// <param name="vm">The shift new details.</param>
    /// <response code="204">The worker shift was successfully updated.</response>
    /// <response code="400">The provided data was invalid.</response>
    /// <response code="404">The worker or the shift was not found.</response>
    /// <response code="409">The worker already has a shift for the specified day.</response>
    [HttpPut("{shiftId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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

    /// <summary>
    /// Delete a worker shift.
    /// </summary>
    /// <param name="id">The worker ID.</param>
    /// <param name="shiftId">The shift ID.</param>
    /// <response code="204">The worker shift was successfully updated.</response>
    /// <response code="404">The worker or the shift was not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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