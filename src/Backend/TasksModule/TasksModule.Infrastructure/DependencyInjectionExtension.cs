using Microsoft.Extensions.DependencyInjection;
using TasksModule.Domain.Repositories;
using TasksModule.Infrastructure.Repositories;
using TasksModule.Infrastructure.Services;
using ISystemClock = TasksModule.Domain.Services.ISystemClock;

namespace TasksModule.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddTasksModuleInfrastructure(this IServiceCollection services)
    {
        AddRepositories(services);
        AddServices(services);
    }
    
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ITaskWriteOnlyRepository, TaskRepository>();
        services.AddScoped<ITaskUpdateOnlyRepository, TaskRepository>();
        services.AddScoped<ITaskReadOnlyRepository, TaskRepository>();
    }
    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ISystemClock, SystemClock>();
    }
}   