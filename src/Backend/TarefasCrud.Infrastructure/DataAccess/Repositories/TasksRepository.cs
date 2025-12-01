using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Tasks;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class TasksRepository : ITaskWriteOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public TasksRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task Add(TaskEntity task) => await _dbContext.Tasks.AddAsync(task);
}