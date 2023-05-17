using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Domain.Interfaces;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Controllers;

/// <summary>
/// Controller responsible for managing workers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WorkerController : ControllerBase
{
    private readonly IWorkerService _workerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkerController"/> class.
    /// </summary>
    /// <param name="workerService">The worker service.</param>
    public WorkerController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    /// <summary>
    /// Get the list of workers.
    /// </summary>
    /// <param name="includeDeactivated">A flag whether the deactivated workers should be returned or not.</param>
    /// <response code="200">A list of all the workers.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ICollection<WorkerVm>> Get(bool includeDeactivated = false)
    {
        return _workerService.Get(includeDeactivated);
    }

    /// <summary>
    /// Get a specific worker details.
    /// </summary>
    /// <param name="id">The worker ID.</param>
    /// <response code="200">The worker details.</response>
    /// <response code="404">The worker was not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkerVm>> Get(Guid id)
    {
        var worker = await _workerService.Get(id);

        if (worker == null)
            return NotFound();

        return Ok(worker);
    }

    /// <summary>
    /// Create a new worker.
    /// </summary>
    /// <param name="vm">The worker details.</param>
    /// <returns>The ID of the created worker.</returns>
    /// <response code="200">The created worker's ID.</response>
    /// <response code="400">The provided data was invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create([FromBody] WorkerCreateVm vm)
    {
        try
        {
            var guid = await _workerService.Create(vm);
            return Ok(guid);
        }
        catch (EntityDuplicateException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing worker.
    /// </summary>
    /// <param name="id">The ID of the worker to be updated.</param>
    /// <param name="vm">The worker details.</param>
    /// <response code="204">The worker was successfully updated.</response>
    /// <response code="400">The provided data was invalid.</response>
    /// <response code="404">The worker was not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(Guid id, [FromBody] WorkerCreateVm vm)
    {
        try
        {
            await _workerService.Update(id, vm);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (EntityDuplicateException ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    /// <summary>
    /// Deactivate a worker.
    /// </summary>
    /// <param name="id">The ID of the worker to be deactivated.</param>
    /// <response code="204">The worker was successfully deactivated.</response>
    /// <response code="404">The worker was not found.</response>
    [HttpPut("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Deactivate(Guid id)
    {
        try
        {
            await _workerService.Deactivate(id);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return NoContent();
    }

    /// <summary>
    /// Reactivate a previously deactivated worker.
    /// </summary>
    /// <param name="id">The ID of the worker to be reactivated.</param>
    /// <response code="204">The worker was successfully reactivated.</response>
    /// <response code="404">The worker was not found.</response>
    [HttpPut("{id:guid}/reactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Reactivate(Guid id)
    {
        try
        {
            await _workerService.Reactivate(id);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return NoContent();
    }
}