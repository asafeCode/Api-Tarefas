using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Tasks;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class TasksRepository : ITaskWriteOnlyRepository,  ITaskReadOnlyRepository, ITaskUpdateOnlyRepository,  ITaskDeleteOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public TasksRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task Add(TaskEntity task) => await _dbContext.Tasks.AddAsync(task);
    async Task<TaskEntity?> ITaskReadOnlyRepository.GetById(User user, long taskId) => await _dbContext
        .Tasks
        .AsNoTracking()
        .FirstOrDefaultAsync(task => task.Active && task.Id == taskId && task.UserId == user.Id);

    public void Update(TaskEntity task) => _dbContext.Tasks.Update(task); 

    async Task<TaskEntity?> ITaskUpdateOnlyRepository.GetById(User user, long taskId) => await _dbContext
        .Tasks
        .FirstOrDefaultAsync(task => task.Active && task.Id == taskId && task.UserId == user.Id);
}