using Mapster;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.RoutineTask.Get.GetById;

public class GetTaskByIdUseCase  : IGetTaskByIdUseCase
{
    private readonly ITaskReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GetTaskByIdUseCase(ITaskReadOnlyRepository repository, 
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTaskJson> Execute(long taskId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _repository.GetById(loggedUser, taskId);
        
        if (recipe is null)
            throw new NotFoundException("Tarefa não encontrada");
        
        var response = recipe.Adapt<ResponseTaskJson>();
        return response;
    }
}