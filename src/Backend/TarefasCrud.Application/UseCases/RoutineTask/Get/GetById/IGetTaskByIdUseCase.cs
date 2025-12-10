using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.RoutineTask.Get.GetById;

public interface IGetTaskByIdUseCase
{
    public Task<ResponseTaskJson> Execute(long taskId);
}