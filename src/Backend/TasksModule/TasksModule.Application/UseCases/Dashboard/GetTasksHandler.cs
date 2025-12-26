using TarefasCrud.Shared.Responses.TasksModule;
using TarefasCrud.Shared.Services;
using TasksModule.Application.Mappers;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Dashboard;

public class GetTasksHandler 
{
    private readonly ITaskReadOnlyRepository _readRepository;
    private readonly ILoggedUser _loggedUser;
    public GetTasksHandler(ITaskReadOnlyRepository readRepository, 
        ILoggedUser loggedUser)
    {
        _readRepository = readRepository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTasksJson> Handle(GetTasksQuery query)
    {
        var loggedUser = await _loggedUser.User();
        var tasks = await _readRepository.GetTasks(loggedUser, query.Filters);

        return new ResponseTasksJson
        {
            Tasks = tasks.ToResponse()
        };
    }
}