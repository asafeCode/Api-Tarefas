using TarefasCrud.Domain.Enums;

namespace TarefasCrud.Application.UseCases.RoutineTask.Update.Progress;

public interface IUpdateTaskProgressUseCase
{
    public System.Threading.Tasks.Task Execute(long taskId, ProgressOperation operation);
}