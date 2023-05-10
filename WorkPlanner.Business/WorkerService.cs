using WorkPlanner.Data.Entities;
using WorkPlanner.Data.Interfaces;

namespace WorkPlanner.Business;

public class WorkerService
{
    private readonly IRepository _repository;

    public WorkerService(IRepository repository)
    {
        _repository = repository;
    }

    public WorkerEntity Create()
    {

    }
}