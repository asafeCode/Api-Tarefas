using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.EmailVerificationToken;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure.Factories;
using UsersModule.Infrastructure.Repositories;
using UsersModule.Infrastructure.Security.Tokens.Access.Validator;
using UsersModule.Infrastructure.Security.Tokens.Refresh;
using UsersModule.Infrastructure.Services;
using UsersModule.Infrastructure.Services.Tokens.Access.Generator;
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
        AddFactories(services, configuration);
        //AddPaperCut(services, configuration);

    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IEmailVerifyReadRepository, EmailVerificationTokenRepository>();
        services.AddScoped<IEmailVerifyWriteRepository, EmailVerificationTokenRepository>();
    }
    private static void AddFactories(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailVerificationSettings>(options =>
            configuration.GetSection("Settings:EmailVerification").Bind(options));
        services.AddScoped<ISystemClock, SystemClock>();
        services.AddScoped<IEmailVerificationLinkGenerator, EmailVerificationFactory>();
        services.AddScoped<IEmailVerificationTokenGenerator, EmailVerificationFactory>();
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(options =>
            configuration.GetSection("Settings:Jwt").Bind(options)
        );

        services.AddScoped<IAccessTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAccessTokenValidator, JwtTokenValidator>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
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