using TasksModule.Domain.Dtos;
using TasksModule.Domain.Entities;

namespace TasksModule.Domain.Tasks;

public interface ITaskReadOnlyRepository
{
    public Task<TaskEntity?> GetById(Entities.User user, long taskId);
    public Task<IList<Entities.TaskEntity>> GetTasks(Entities.User user, FilterTasksDto filters);
}