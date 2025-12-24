namespace TasksModule.Application.RoutineTask.GetTasks;

public class GetTasksUseCase : IGetTasksUseCase
{
    private readonly ITaskReadOnlyRepository _readRepository;
    private readonly ILoggedUser _loggedUser;
    public GetTasksUseCase(ITaskReadOnlyRepository readRepository, 
        ILoggedUser loggedUser)
    {
        _readRepository = readRepository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTasksJson> Execute(FilterTasksDto filters)
    {
        var loggedUser = await _loggedUser.User();
        var tasks = await _readRepository.GetTasks(loggedUser, filters);

        return new ResponseTasksJson
        {
            Tasks = tasks.Adapt<IList<ResponseShortTaskJson>>()
        };
    }
}