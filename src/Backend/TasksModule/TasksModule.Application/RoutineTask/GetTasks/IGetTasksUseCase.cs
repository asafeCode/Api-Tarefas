namespace TasksModule.Application.RoutineTask.GetTasks;

public interface IGetTasksUseCase
{
    public Task<ResponseTasksJson> Execute(FilterTasksDto filters);
}