namespace TasksModule.Application.RoutineTask.Register;

public interface IRegisterTaskUseCase
{
    public Task<ResponseRegisteredTaskJson> Execute(RequestTaskJson request);
}