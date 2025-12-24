namespace TasksModule.Application.RoutineTask.Delete;

public interface IDeleteTaskUseCase
{
    public Task Execute(long taskId);
}