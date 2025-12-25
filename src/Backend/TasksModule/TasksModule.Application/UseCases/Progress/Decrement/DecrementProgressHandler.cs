using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Enums;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;

namespace TasksModule.Application.UseCases.Progress.Decrement;

public class DecrementProgressHandler
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _systemClock;
    public DecrementProgressHandler(ILoggedUser loggedUser, 
        ITaskUpdateOnlyRepository updateRepository, 
        IUnitOfWork unitOfWork, 
        ISystemClock systemClock)
    {
        _loggedUser = loggedUser;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
    }
    public async Task Handle(DecrementProgressCommand command)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _updateRepository.GetById(loggedUser, command.TaskId);

        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        const ProgressOperation operation = ProgressOperation.Decrement;
        
        ValidateProgressChange(task);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = false;
        task.ModifiedAt = _systemClock.UseCaseDate;
        
        _updateRepository.Update(task);
        await _unitOfWork.Commit();
    }
    private void ValidateProgressChange(TaskEntity task)
    {
        if (task.IsInCurrentWeek(_systemClock).IsFalse())
            throw new ConflictException(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);
        
        if (task.IsInInitialProgress())
            throw new ConflictException(ResourceMessagesException.NOT_DECREMENT_INITIAL_PROGRESS_TASK);
    }
}
