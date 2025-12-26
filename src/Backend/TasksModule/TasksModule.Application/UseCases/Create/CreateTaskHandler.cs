using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Responses.TasksModule;
using TarefasCrud.Shared.Services;
using TasksModule.Application.Mappers;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;
using TasksModule.Domain.ValueObjects;

namespace TasksModule.Application.UseCases.Create;

public class CreateTaskHandler(
    ILoggedUser loggedUser, 
    ITaskWriteOnlyRepository repository, 
    IUnitOfWork unitOfWork)
{
    public async Task<ResponseRegisteredTaskJson> Handle(CreateTaskCommand request)
    {
        var userLogged = await loggedUser.User();
        
        var task = request.ToTask(userLogged.Id);
        
        task.UserId = userLogged.Id;
        task.WeekOfMonth = task.StartDate.GetMonthWeek();
        task.Progress = TarefasCrudRuleConstants.INITIAL_PROGRESS;

        await repository.Add(task);
        await unitOfWork.Commit();

        return new ResponseRegisteredTaskJson
        {
            Id = task.Id,
            Title = task.Title,
        };
    }
}