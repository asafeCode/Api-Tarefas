using TarefasCrud.Domain.Entities;

namespace TarefasCrud.Domain.Repositories.Tasks;

public interface ITaskReadOnlyRepository
{
    public Task<TaskEntity?> GetById(Entities.User user, long taskId);
}