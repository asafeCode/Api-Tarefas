namespace TasksModule.Application.RoutineTask.Register;

public class RegisterTaskUseCase : IRegisterTaskUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _systemClock;
    
    public RegisterTaskUseCase(ILoggedUser loggedUser, 
        ITaskWriteOnlyRepository repository, 
        IUnitOfWork unitOfWork, 
        ISystemClock systemClock)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
    }
    public async Task<ResponseRegisteredTaskJson> Execute(RequestTaskJson request)
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

    private void Validate(RequestTaskJson request)
    {
        var date = _systemClock.UseCaseDate.ToDateOnly();;
        var validator = new TaskValidator(date);
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        HandleValidationResult.ThrowError(result);
    }
}