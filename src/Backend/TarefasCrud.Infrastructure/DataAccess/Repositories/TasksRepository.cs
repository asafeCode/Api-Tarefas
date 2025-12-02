using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Tasks;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class TasksRepository : ITaskWriteOnlyRepository,  ITaskReadOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public TasksRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task Add(TaskEntity task) => await _dbContext.Tasks.AddAsync(task);
    public async Task<TaskEntity?> GetById(User user, long taskId) => await _dbContext
        .Tasks
        .AsNoTracking()
        .FirstOrDefaultAsync(task => task.Active && task.Id == taskId && task.UserId == user.Id);
}