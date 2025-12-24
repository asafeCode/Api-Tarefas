using TasksModule.Application.Validators;
using TasksModule.Domain.Entities;
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
        Validate(request);
        var loggedUser = await _loggedUser.User();
        
        var task = request.Adapt<TaskEntity>();
        
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

    private void Validate(CreateTaskCommand request)
    {
        var date = _systemClock.UseCaseDate.ToDateOnly();;
        var validator = new CreateTaskValidator(date);
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        HandleValidationResult.ThrowError(result);
    }
}