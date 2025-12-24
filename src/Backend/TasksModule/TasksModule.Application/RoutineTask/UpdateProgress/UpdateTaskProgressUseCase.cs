namespace TasksModule.Application.RoutineTask.UpdateProgress;

public class UpdateTaskProgressUseCase : IUpdateTaskProgressUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _systemClock;
    public UpdateTaskProgressUseCase(ILoggedUser loggedUser, 
        ITaskUpdateOnlyRepository updateRepository, 
        IUnitOfWork unitOfWork, 
        ISystemClock systemClock)
    {
        _loggedUser = loggedUser;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
    }
    public async Task ExecuteIncrement(long taskId, bool force = false)
    {
        var task = await GetTaskOrThrow(taskId);
        
        const ProgressOperation operation = ProgressOperation.Increment;
        
        ValidateProgressChange(task, operation, force);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = task.Progress == task.WeeklyGoal;
        task.ModifiedAt = _systemClock.UseCaseDate;
        
        _updateRepository.Update(task);
        await _unitOfWork.Commit();
    }
    public async Task ExecuteDecrement(long taskId)
    {
        var task = await GetTaskOrThrow(taskId);
        const ProgressOperation operation = ProgressOperation.Decrement;
        
        ValidateProgressChange(task, operation);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = false;
        task.ModifiedAt = _systemClock.UseCaseDate;
        
        _updateRepository.Update(task);
        await _unitOfWork.Commit();
    }
    private void ValidateProgressChange(TaskEntity task, ProgressOperation action, bool force = false)
    {
        if (task.IsInCurrentWeek(_systemClock).IsFalse())
            throw new ConflictException(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);
        switch (action)
        {
            case ProgressOperation.Increment when task.IsCompleted:
                throw new ConflictException(ResourceMessagesException.NOT_INCREMENT_COMPLETED_TASK);
            
            case ProgressOperation.Increment when task.WasModifiedToday(_systemClock) && force.IsFalse():
                throw new ConflictException(ResourceMessagesException.CONFIRMATION_REQUIRED_TO_UPDATE_PROGRESS);
            
            case ProgressOperation.Decrement when task.IsInInitialProgress():
                throw new ConflictException(ResourceMessagesException.NOT_DECREMENT_INITIAL_PROGRESS_TASK);
        }
    }
    private async Task<TaskEntity> GetTaskOrThrow(long taskId)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _updateRepository.GetById(loggedUser, taskId);

        return task ?? throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
    }
}
