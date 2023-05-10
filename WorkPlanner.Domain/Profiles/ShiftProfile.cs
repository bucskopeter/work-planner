using AutoMapper;
using WorkPlanner.Data.Entities;
using WorkPlanner.Domain.Models;

namespace WorkPlanner.Domain.Profiles;

public class ShiftProfile : Profile
{
    public ShiftProfile()
    {
        CreateMap<ShiftEntity, ShiftVm>();
        CreateMap<ShiftCreateVm, ShiftEntity>();
    }
}