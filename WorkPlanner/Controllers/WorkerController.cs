using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Core.Exceptions;
using WorkPlanner.Domain.Interfaces;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkerController : ControllerBase
{
    private readonly IWorkerService _workerService;

    public WorkerController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    [HttpGet]
    public Task<ICollection<WorkerVm>> Get(bool includeDeleted = false)
    {
        return _workerService.Get(includeDeleted);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkerVm>> Get(Guid id)
    {
        var worker = await _workerService.Get(id);

        if (worker == null)
            return NotFound();

        return Ok(worker);
    }

    [HttpPost]
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

    [HttpPut("{id:guid}")]
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

    [HttpPut("{id:guid}/deactivate")]
    public async Task<ActionResult<Guid?>> Deactivate(Guid id)
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

    [HttpPut("{id:guid}/reactivate")]
    public async Task<ActionResult<Guid?>> Reactivate(Guid id)
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