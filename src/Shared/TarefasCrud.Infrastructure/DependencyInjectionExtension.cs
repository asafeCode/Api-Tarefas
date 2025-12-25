using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarefasCrud.Infrastructure.DataAccess;
using TarefasCrud.Infrastructure.Extensions;
using TarefasCrud.Infrastructure.Repositories;
using TarefasCrud.Shared.Repositories;

namespace TarefasCrud.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext_SqlServer(services, configuration);
        AddFluentMigrator_SqlServer(services, configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddDbContext<TarefasCrudDbContext>(dbOpt => 
            dbOpt.UseSqlServer(connectionString));
    }
    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        
        services.AddFluentMigratorCore().ConfigureRunner(options => 
            options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("TarefasCrud.Infrastructure")).For.All()
        );
    }
}   