using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.Tasks.GetById;

public class GetTaskByIdUseCase  : IGetTaskByIdUseCase
{
    
    public GetTaskByIdUseCase()
    {
        
    }
    public Task<ResponseTaskJson> Execute(long taskId)
    {
        throw new NotImplementedException();
    }
}