using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Dtos;

namespace TasksModule.Domain.Repositories;

public interface ITaskReadOnlyRepository
{
    public Task<TaskEntity?> GetById(User user, long taskId);
    public Task<IList<TaskEntity>> GetTasks(User user, FilterTasks filters);
}