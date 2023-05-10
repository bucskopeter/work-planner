using AutoMapper;
using WorkPlanner.Data.Entities;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Domain.Profiles;

public class WorkerProfile : Profile
{
    public WorkerProfile()
    {
        CreateMap<WorkerEntity, WorkerVm>();
        CreateMap<WorkerCreateVm, WorkerEntity>();
    }
}