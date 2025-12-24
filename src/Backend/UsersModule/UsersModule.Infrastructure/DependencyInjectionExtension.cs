using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure.Repositories;
using UsersModule.Infrastructure.Services;
using UsersModule.Infrastructure.Services.Events.Publishers;
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
        //AddPaperCut(services, configuration);

    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenRepository, TokenRepository>();
    }
    private static void AddBusServices(IServiceCollection services)
    {
        services.AddScoped<IEmailVerificationPublisher, EmailVerificationPublisher>();
        services.AddScoped<IUserDeletedPublisher, UserDeletedPublisher>();

        //services.AddWolverine();

    }
    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(options =>
            configuration.GetSection("Settings:Jwt").Bind(options)
        );
        services.Configure<EmailVerificationSettings>(options =>
            configuration.GetSection("Settings:EmailVerification").Bind(options));

        services.AddScoped<IAccessTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAccessTokenValidator, JwtTokenValidator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        
        services.AddScoped<IEmailVerificationLinkGenerator, EmailVerificationServices>();
        services.AddScoped<IEmailVerificationTokenGenerator, EmailVerificationServices>();
    }
    private static void AddLoggedUser(IServiceCollection services)
    { 
       services.AddScoped<ILoggedUser, LoggedUser>();
    }
    private static void AddPasswordEncripter(this IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BcryptEncripter>();
    }
   
    // private static void AddPaperCut(this IServiceCollection services, IConfiguration configuration)
    // {
    //     services
    //         .AddFluentEmail(configuration["Settings:Email:SenderEmail"],
    //             configuration["Settings:Email:Sender"])
    //         .AddSmtpSender(configuration["Settings:Email:Host"],
    //             configuration.GetValue<int>("Settings:Email:Port"))
    //         .AddRazorRenderer();
    // }
}   