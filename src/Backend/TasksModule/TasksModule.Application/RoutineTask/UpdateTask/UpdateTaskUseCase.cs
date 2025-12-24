namespace TasksModule.Application.RoutineTask.UpdateTask;

public class UpdateTaskUseCase :  IUpdateTaskUseCase
{
    private readonly ITaskUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _systemClock;
    
    public UpdateTaskUseCase(ITaskUpdateOnlyRepository repository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork, 
        ISystemClock systemClock)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
    }
    public async Task Execute(long taskId, RequestTaskJson request)
    {
        var loggedUser = await _loggedUser.User();
        
        var task = await _repository.GetById(loggedUser, taskId);
        if (task is null)
            throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        Validate(request, task);

        task.Title = request.Title;
        
        if (request.Description.NotEmpty())
            task.Description = request.Description;
        
        task.StartDate = request.StartDate;
        task.Category = request.Category;
        task.WeeklyGoal = request.WeeklyGoal;

        task.IsCompleted = task.Progress == task.WeeklyGoal;
        
        _repository.Update(task);
        await _unitOfWork.Commit();
    }
    private void Validate(RequestTaskJson request, TaskEntity task)
    {
        var date = _systemClock.UseCaseDate.ToDateOnly();;
        var validator = new TaskValidator(date, task);
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        HandleValidationResult.ThrowError(result);
    }
}