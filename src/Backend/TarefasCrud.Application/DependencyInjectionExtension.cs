using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarefasCrud.Application.Services;
using TarefasCrud.Application.UseCases.Login;
using TarefasCrud.Application.UseCases.User;
using TarefasCrud.Application.UseCases.User.Profile;
using TarefasCrud.Application.UseCases.User.Register;

namespace TarefasCrud.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services,  IConfiguration configuration)
    {
        AddUseCases(services);
        AddMapper();
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        
    }

    private static void AddMapper()
    {
        MapConfigurations.Configure();
    }
}