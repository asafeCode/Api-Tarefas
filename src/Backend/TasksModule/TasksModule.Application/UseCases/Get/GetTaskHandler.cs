using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Responses.TasksModule;
using TarefasCrud.Shared.Services;
using TasksModule.Application.Mappers;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Get;

public class GetTaskHandler
{
    public static async Task<ResponseTaskJson> Handle(GetTaskQuery query,
        ITaskReadOnlyRepository repository, 
        ILoggedUser loggedUser)
    {
        var userLogged = await loggedUser.User();
        var task = await repository.GetById(userLogged, query.TaskId);
        
        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        return task.ToResponse();
    }
}