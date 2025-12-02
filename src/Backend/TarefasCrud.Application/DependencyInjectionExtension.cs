using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarefasCrud.Application.Services;
using TarefasCrud.Application.UseCases.Login;
using TarefasCrud.Application.UseCases.Tasks;
using TarefasCrud.Application.UseCases.Tasks.GetById;
using TarefasCrud.Application.UseCases.Tasks.Register;
using TarefasCrud.Application.UseCases.Token.RefreshToken;
using TarefasCrud.Application.UseCases.User.ChangePassword;
using TarefasCrud.Application.UseCases.User.Profile;
using TarefasCrud.Application.UseCases.User.Register;
using TarefasCrud.Application.UseCases.User.Update;

namespace TarefasCrud.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddMapper();
    }
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        
        services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
        
        services.AddScoped<IRegisterTaskUseCase, RegisterTaskUseCase>();
        services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
    }
    private static void AddMapper()
    {
        MapConfigurations.Configure();
    }
}