using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.EmailVerificationToken;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure.DataAccess;
using UsersModule.Infrastructure.DataAccess.Repositories;
using UsersModule.Infrastructure.Events.Consumers;
using UsersModule.Infrastructure.Events.Producers;
using UsersModule.Infrastructure.Factories;
using UsersModule.Infrastructure.Security.Criptography;
using UsersModule.Infrastructure.Security.Tokens.Access.Generator;
using UsersModule.Infrastructure.Security.Tokens.Access.Validator;
using UsersModule.Infrastructure.Security.Tokens.Refresh;
using UsersModule.Infrastructure.Services;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddTasksModuleInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddTokens(services, configuration);
        AddLoggedUser(services);
        AddPasswordEncripter(services);
        AddDbContext_SqlServer(services, configuration);
        AddFluentMigrator_SqlServer(services, configuration);
        AddFactories(services, configuration);
        AddBusServices(services);
        AddPaperCut(services, configuration);

    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddDbContext<TarefasCrudDbContext>(
            dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IEmailVerifyReadRepository, EmailVerificationTokenRepository>();
        services.AddScoped<IEmailVerifyWriteRepository, EmailVerificationTokenRepository>();
        
        services.AddScoped<ITaskWriteOnlyRepository, TaskRepository>();
        services.AddScoped<ITaskUpdateOnlyRepository, TaskRepository>();
        services.AddScoped<ITaskReadOnlyRepository, TaskRepository>();
    }
    private static void AddFactories(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailVerificationSettings>(options =>
            configuration.GetSection("Settings:EmailVerification").Bind(options));
        services.AddScoped<ISystemClock, SystemClock>();
        services.AddScoped<IEmailVerificationFactory, EmailVerificationFactory>();
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
    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("TarefasCrud.Infrastructure")).For.All();
        });
    }
    private static void AddLoggedUser(IServiceCollection services)
    { 
       services.AddScoped<ILoggedUser, LoggedUser>();
    }
    private static void AddPasswordEncripter(this IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BcryptEncripter>();
    }
    private static void AddBusServices(this IServiceCollection services)
    {
        services.AddScoped<IDeleteUserQueue, DeleteUserQueue>();
        services.AddScoped<IEmailVerificationQueue, EmailVerificationQueue>();
        services.AddMassTransit(busConfig =>
        {
            busConfig.AddConsumer<DeletedUserQueueConsumer>();
            busConfig.AddConsumer<EmailVerificationQueueConsumer>();
            busConfig.AddDelayedMessageScheduler();
            busConfig.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri("amqp://localhost:5672"), host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });
                cfg.UseDelayedMessageScheduler();
                cfg.ConfigureEndpoints(context);
            });
        });
    }    
    private static void AddPaperCut(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFluentEmail(configuration["Settings:Email:SenderEmail"],
                configuration["Settings:Email:Sender"])
            .AddSmtpSender(configuration["Settings:Email:Host"],
                configuration.GetValue<int>("Settings:Email:Port"))
            .AddRazorRenderer();
    }
}   