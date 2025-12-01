using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.Tasks.GetById;

public interface IGetTaskByIdUseCase
{
    public Task<ResponseTaskJson> Execute(long taskId);
}