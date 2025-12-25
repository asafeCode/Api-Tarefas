using Microsoft.EntityFrameworkCore;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Entities;
using UsersModule.Domain.ValueObjects;

namespace TarefasCrud.Infrastructure;

public class TarefasCrudDbContext : DbContext
{
    public TarefasCrudDbContext(DbContextOptions options) : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TarefasCrudDbContext).Assembly);
    }
    
}