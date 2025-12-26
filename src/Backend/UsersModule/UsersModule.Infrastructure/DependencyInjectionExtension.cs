using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarefasCrud.Shared.Services;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Email;
using UsersModule.Domain.Services.Security;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure.Repositories;
using UsersModule.Infrastructure.Services.Email;
using UsersModule.Infrastructure.Services.Events.Publishers;
using UsersModule.Infrastructure.Services.LoggedUser;
using UsersModule.Infrastructure.Services.Security;
using UsersModule.Infrastructure.Services.Tokens;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddUsersModuleInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddTokens(services, configuration);
        AddLoggedUser(services);
        AddPasswordEncripter(services);
        AddBusServices(services);
        AddEmailService(services, configuration);

    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserInternalRepository, UserRepository>();
        
        services.AddScoped<ITokenRepository, TokenRepository>();
    }
    private static void AddBusServices(IServiceCollection services)
    {
        services.AddScoped<IEmailPublisher, EmailPublisher>();
        services.AddScoped<IUserDeletedPublisher, UserDeletedPublisher>();
    }
    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(options =>
            configuration.GetSection("Settings:Tokens:Jwt").Bind(options)
        );
        services.Configure<VerificationTokenSettings>(options =>
            configuration.GetSection("Settings:Tokens:Verification").Bind(options));

        services.AddScoped<IAccessTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAccessTokenValidator, JwtTokenValidator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        
        services.AddScoped<IVerificationLinkGenerators, VerificationLinkGenerators>();
        services.AddScoped<IVerificationTokenGenerator, VerificationTokenGenerator>();
    }
    private static void AddLoggedUser(IServiceCollection services)
    { 
       services.AddScoped<ILoggedUser, LoggedUser>();
    }
    private static void AddPasswordEncripter(this IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BcryptEncripter>();
    }
   
    private static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        services
            .AddFluentEmail(configuration["Settings:Email:SenderEmail"],
                configuration["Settings:Email:Sender"])
            .AddSmtpSender(configuration["Settings:Email:Host"],
                configuration.GetValue<int>("Settings:Email:Port"))
            .AddRazorRenderer();
    }
}   