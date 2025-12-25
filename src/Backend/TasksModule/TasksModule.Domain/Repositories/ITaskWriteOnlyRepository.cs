using TarefasCrud.Shared.SharedEntities;

namespace TasksModule.Domain.Repositories;

public interface ITaskWriteOnlyRepository
{
    public Task Add(TaskEntity task);
    public Task Delete(long taskId);
}