using TarefasCrud.Shared.SharedEntities;

namespace TasksModule.Domain.Repositories;

public interface ITaskUpdateOnlyRepository
{
    public Task<TaskEntity?> GetById(User user, long taskId);
    public void Update(TaskEntity task);
}