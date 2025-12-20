using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Tasks;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;
public class TaskWriteRepository : ITaskWriteOnlyRepository, ITaskUpdateOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public TaskWriteRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task Add(TaskEntity task) => await _dbContext.Tasks.AddAsync(task);
    public async Task Delete(long taskId)
    {
        var task = await _dbContext.Tasks.FindAsync(taskId);
        _dbContext.Tasks.Remove(task!);
    } 
    public void Update(TaskEntity task) => _dbContext.Tasks.Update(task); 
    public async Task<TaskEntity?> GetById(User user, long taskId) => await _dbContext
        .Tasks
        .FirstOrDefaultAsync(task => task.Active && task.Id == taskId && task.UserId == user.Id);
}