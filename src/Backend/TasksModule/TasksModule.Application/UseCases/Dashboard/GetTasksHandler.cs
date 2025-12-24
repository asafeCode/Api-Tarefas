using TasksModule.Domain.Dtos;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Dashboard;

public class GetTasksHandler : IGetTasksUseCase
{
    private readonly ITaskReadOnlyRepository _readRepository;
    private readonly ILoggedUser _loggedUser;
    public GetTasksHandler(ITaskReadOnlyRepository readRepository, 
        ILoggedUser loggedUser)
    {
        _readRepository = readRepository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTasksJson> Handle(FilterTasksDto filters)
    {
        var loggedUser = await _loggedUser.User();
        var tasks = await _readRepository.GetTasks(loggedUser, filters);

        return new ResponseTasksJson
        {
            Tasks = tasks.Adapt<IList<ResponseShortTaskJson>>()
        };
    }
}