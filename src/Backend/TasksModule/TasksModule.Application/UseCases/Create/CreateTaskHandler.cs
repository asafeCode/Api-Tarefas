using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Responses.TasksModule;
using TarefasCrud.Shared.Services;
using TasksModule.Application.Mappers;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;
using TasksModule.Domain.ValueObjects;

namespace TasksModule.Application.UseCases.Create;

public class CreateTaskHandler 
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _systemClock;
    
    public CreateTaskHandler(ILoggedUser loggedUser, 
        ITaskWriteOnlyRepository repository, 
        IUnitOfWork unitOfWork, 
        ISystemClock systemClock)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
    }
    public async Task<ResponseRegisteredTaskJson> Handle(CreateTaskCommand request)
    {
        var loggedUser = await _loggedUser.User();
        
        var task = request.ToTask(loggedUser.Id);
        
        task.UserId = loggedUser.Id;
        task.WeekOfMonth = task.StartDate.GetMonthWeek();
        task.Progress = TarefasCrudRuleConstants.INITIAL_PROGRESS;

        await _repository.Add(task);
        await _unitOfWork.Commit();

        return new ResponseRegisteredTaskJson
        {
            Id = task.Id,
            Title = task.Title,
        };
    }
}