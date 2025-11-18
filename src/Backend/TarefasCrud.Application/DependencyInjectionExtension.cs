using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TarefasCrud.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services,  IConfiguration configuration)
    {
        AddUseCases(services);
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
    } 
}