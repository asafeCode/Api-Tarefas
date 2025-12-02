using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.Tasks.Get.GetById;

public interface IGetTaskByIdUseCase
{
    public Task<ResponseTaskJson> Execute(long taskId);
}