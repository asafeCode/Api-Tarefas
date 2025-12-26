using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Enums;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;

namespace TasksModule.Application.UseCases.Progress.Increment;

public class UpdateProgressHandler 
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
    public async Task Handle(IncrementProgressCommand command)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _updateRepository.GetById(loggedUser, command.TaskId);

        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        const ProgressOperation operation = ProgressOperation.Increment;
        
        ValidateProgressChange(task, command.Force);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = task.Progress == task.WeeklyGoal;
        task.ModifiedAt = _systemClock.UseCaseDate;
        
        _updateRepository.Update(task);
        await _unitOfWork.Commit();
    }
   
    private void ValidateProgressChange(TaskEntity task, bool force = false)
    {
        if (task.IsInCurrentWeek(_systemClock).IsFalse())
            throw new ConflictException(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);
        
        if (task.WasModifiedToday(_systemClock) && force.IsFalse())
            throw new ConflictException(ResourceMessagesException.CONFIRMATION_REQUIRED_TO_UPDATE_PROGRESS);
        
        if (task.IsCompleted)
            throw new ConflictException(ResourceMessagesException.NOT_INCREMENT_COMPLETED_TASK);
    }
}
