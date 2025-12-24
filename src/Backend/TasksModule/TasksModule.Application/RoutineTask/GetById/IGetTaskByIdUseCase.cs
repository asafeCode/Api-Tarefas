namespace TasksModule.Application.RoutineTask.GetById;

public interface IGetTaskByIdUseCase
{
    public Task<ResponseTaskJson> Execute(long taskId);
}