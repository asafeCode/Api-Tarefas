using TasksModule.Domain.Entities;

namespace TasksModule.Domain.Tasks;

public interface ITaskWriteOnlyRepository
{
    public Task Add(TaskEntity task);
    public Task Delete(long taskId);
}