using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;
using TasksModule.Domain.Entities;
using TasksModule.Domain.Enums;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;

namespace TasksModule.Application.UseCases.Progress.Increment;

public class UpdateProgressHandler : IUpdateTaskProgressUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _systemClock;
    public UpdateProgressHandler(ILoggedUser loggedUser, 
        ITaskUpdateOnlyRepository updateRepository, 
        IUnitOfWork unitOfWork, 
        ISystemClock systemClock)
    {
        _loggedUser = loggedUser;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
    }
    public async Task Handle(long taskId, bool force = false)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _updateRepository.GetById(loggedUser, taskId);

        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        const ProgressOperation operation = ProgressOperation.Increment;
        
        ValidateProgressChange(task, operation, force);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = task.Progress == task.WeeklyGoal;
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
}
