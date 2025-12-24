using TasksModule.Domain.Entities;

namespace TasksModule.Domain.Tasks;

public interface ITaskUpdateOnlyRepository
{
    public Task<TaskEntity?> GetById(Entities.User user, long taskId);
    public void Update(TaskEntity task);
}