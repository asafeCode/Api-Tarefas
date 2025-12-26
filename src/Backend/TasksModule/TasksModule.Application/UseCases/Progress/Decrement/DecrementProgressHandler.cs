using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Enums;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;

namespace TasksModule.Application.UseCases.Progress.Decrement;

public class DecrementProgressHandler(ILoggedUser loggedUser, 
    ITaskUpdateOnlyRepository updateRepository, 
    IUnitOfWork unitOfWork, 
    ISystemClock systemClock)
{
    public async Task Handle(DecrementProgressCommand command)
    {
        var userLogged = await loggedUser.User();
        var task = await updateRepository.GetById(userLogged, command.TaskId);

        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        const ProgressOperation operation = ProgressOperation.Decrement;
        
        ValidateProgressChange(task);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = false;
        task.ModifiedAt = systemClock.UseCaseDate;
        
        updateRepository.Update(task);
        await unitOfWork.Commit();
    }
    private void ValidateProgressChange(TaskEntity task)
    {
        if (task.IsInCurrentWeek(systemClock).IsFalse())
            throw new ConflictException(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);
        
        if (task.IsInInitialProgress())
            throw new ConflictException(ResourceMessagesException.NOT_DECREMENT_INITIAL_PROGRESS_TASK);
    }
}
