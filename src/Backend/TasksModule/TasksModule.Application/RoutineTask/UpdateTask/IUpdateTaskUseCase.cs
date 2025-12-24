namespace TasksModule.Application.RoutineTask.UpdateTask;

public interface IUpdateTaskUseCase
{
    public System.Threading.Tasks.Task Execute(long taskId, RequestTaskJson request);
    
}