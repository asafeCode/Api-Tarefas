using Microsoft.OpenApi.Models;
using TarefasCrud.API.Converters;
using TarefasCrud.API.Filters;
using TarefasCrud.API.Middleware;
using TarefasCrud.API.Token;
using TarefasCrud.Infrastructure;
using TarefasCrud.Infrastructure.Extensions;
using TarefasCrud.Infrastructure.Migrations;
using TarefasCrud.Shared.Constants;
using TasksModule.Infrastructure;
using UsersModule.Application;
using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;
using Wolverine.RabbitMQ;
using Wolverine.SqlServer;

const string AUTHENTICATION_TYPE = "Bearer";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions
    .Converters.Add(new StringConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTHENTICATION_TYPE
                },
                Scheme = "oauth2",
                Name = AUTHENTICATION_TYPE,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(AssemblyMarker).Assembly);
    opts.Discovery.IncludeAssembly(typeof(UsersModule.Application.EventConsumers.EmailVerificatedConsumer).Assembly);
    opts.UseFluentValidation();

    opts.PersistMessagesWithSqlServer(builder.Configuration.ConnectionString());
    
    opts.UseRabbitMq(new Uri("amqp://guest:guest@localhost:5672"))
        .AutoProvision();

    opts.PublishMessage<UserDeletedEvent>()
        .ToRabbitQueue(Queues.UserDeleted);

    opts.PublishMessage<EmailVerifiedEvent>()
        .ToRabbitQueue(Queues.EmailVerified);
    
    opts.PublishMessage<EmailVerifiedEvent>()
        .ToRabbitQueue(Queues.AccountRecovered);

    opts.ListenToRabbitQueue(Queues.UserDeleted);
    opts.ListenToRabbitQueue(Queues.EmailVerified);
    opts.ListenToRabbitQueue(Queues.AccountRecovered);
    
    opts.Policies.UseDurableInboxOnAllListeners();
    opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
    
    opts.UseEntityFrameworkCoreTransactions();
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddUsersModuleInfrastructure(builder.Configuration);
builder.Services.AddTasksModuleInfrastructure();

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();
return;

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DatabaseMigration.Migrate(connectionString!, serviceScope.ServiceProvider);
}

namespace TarefasCrud.API
{
    public partial class Program 
    {
        protected Program(){}
    }
}

