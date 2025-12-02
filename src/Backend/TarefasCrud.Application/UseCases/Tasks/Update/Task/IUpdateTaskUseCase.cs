using TarefasCrud.Communication.Requests;

namespace TarefasCrud.Application.UseCases.Tasks.Update.Task;

public interface IUpdateTaskUseCase
{
    public System.Threading.Tasks.Task Execute(long taskId, RequestTaskJson request);
    
}