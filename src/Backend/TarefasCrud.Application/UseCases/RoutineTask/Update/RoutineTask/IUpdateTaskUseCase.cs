using TarefasCrud.Communication.Requests;

namespace TarefasCrud.Application.UseCases.RoutineTask.Update.RoutineTask;

public interface IUpdateTaskUseCase
{
    public System.Threading.Tasks.Task Execute(long taskId, RequestTaskJson request);
    
}