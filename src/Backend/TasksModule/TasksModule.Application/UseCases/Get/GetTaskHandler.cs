using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Responses.TasksModule;
using TarefasCrud.Shared.Services;
using TasksModule.Application.Mappers;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Get;

public class GetTaskHandler
{
    private readonly ITaskReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GetTaskHandler(ITaskReadOnlyRepository repository, 
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTaskJson> Handle(GetTaskQuery query)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _repository.GetById(loggedUser, query.TaskId);
        
        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        return task.ToResponse();
    }
}