using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Enums;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;

namespace TasksModule.Application.UseCases.Progress.Increment;

public class UpdateProgressHandler(ILoggedUser loggedUser, 
    ITaskUpdateOnlyRepository updateRepository, 
    IUnitOfWork unitOfWork, 
    ISystemClock systemClock)
{
    public async Task Handle(IncrementProgressCommand command)
    {
        var userLogged = await loggedUser.User();
        var task = await updateRepository.GetById(userLogged, command.TaskId);

        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        const ProgressOperation operation = ProgressOperation.Increment;
        
        ValidateProgressChange(task, command.Force);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = task.Progress == task.WeeklyGoal;
        task.ModifiedAt = systemClock.UseCaseDate;
        
        updateRepository.Update(task);
        await unitOfWork.Commit();
    }
   
    private void ValidateProgressChange(TaskEntity task, bool force = false)
    {
        if (task.IsInCurrentWeek(systemClock).IsFalse())
            throw new ConflictException(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);
        
        if (task.WasModifiedToday(systemClock) && force.IsFalse())
            throw new ConflictException(ResourceMessagesException.CONFIRMATION_REQUIRED_TO_UPDATE_PROGRESS);
        
        if (task.IsCompleted)
            throw new ConflictException(ResourceMessagesException.NOT_INCREMENT_COMPLETED_TASK);
    }
}
