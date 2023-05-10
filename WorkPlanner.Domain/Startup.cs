using Microsoft.Extensions.DependencyInjection;
using WorkPlanner.Domain.Interfaces;
using WorkPlanner.Domain.Services;

namespace WorkPlanner.Domain;

public static class Startup
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IWorkerService, WorkerService>();
        services.AddScoped<IWorkerShiftService, WorkerShiftService>();
    }
}