using CommonTestUtilities.Entities;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarefasCrud.Infrastructure.DataAccess;
using Testcontainers.MsSql;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Strong_password_123!")
        .Build();
    private string _connectionString = string.Empty;
    
    private string _password = string.Empty;
    private TarefasCrud.Domain.Entities.User _user = null!;
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var masterConnectionString = _dbContainer.GetConnectionString();
        var connectionStringBuilder = new SqlConnectionStringBuilder(masterConnectionString)
        {
            InitialCatalog = "db_tarefascrud"
        };
        _connectionString = connectionStringBuilder.ToString();
        var dbContext = Services.GetRequiredService<TarefasCrudDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        (_user, _password) = UserBuilder.Build();
        await dbContext.Users.AddAsync(_user);
        await dbContext.SaveChangesAsync();
    }
    public new async Task DisposeAsync() => await Task.CompletedTask;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureAppConfiguration(opt =>
            {
                opt.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {
                        "ConnectionStrings:ConnectionSqlServer",  _connectionString
                    }
                });
            })
            
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(desc =>
                    desc.ServiceType == typeof(DbContextOptions<TarefasCrudDbContext>));
                
                if (descriptor is not null)
                    services.Remove(descriptor);
                
                services.AddDbContext<TarefasCrudDbContext>(opt =>
                    opt.UseSqlServer(_connectionString));
            });
    }
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetName() => _user.Name;
    public Guid GetUserId() => _user.UserId;
}