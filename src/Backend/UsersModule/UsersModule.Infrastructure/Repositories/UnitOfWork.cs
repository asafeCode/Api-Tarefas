using TarefasCrud.Infrastructure;
using UsersModule.Domain.Repositories;

namespace  UsersModule.Infrastructure.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly TarefasCrudDbContext _dbContext;

    public UnitOfWork(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}