using TarefasCrud.Domain.Dtos;

namespace TarefasCrud.Application.UseCases.RoutineTask.UpdateProgress;

public interface IUpdateTaskProgressUseCase
{
    public Task ExecuteIncrement(long taskId, bool force = false);
    public Task ExecuteDecrement(long taskId);
}