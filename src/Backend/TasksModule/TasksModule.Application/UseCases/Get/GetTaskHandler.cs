using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Get;

public class GetTaskHandler  : IGetTaskByIdUseCase
{
    private readonly ITaskReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GetTaskHandler(ITaskReadOnlyRepository repository, 
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTaskJson> Handle(long taskId)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _repository.GetById(loggedUser, taskId);
        
        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        var response = task.Adapt<ResponseTaskJson>();
        return response;
    }
}