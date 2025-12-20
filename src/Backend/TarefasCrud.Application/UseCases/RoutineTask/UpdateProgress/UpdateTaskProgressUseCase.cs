using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Domain.Dtos;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Enums;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Domain.ValueObjects;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.RoutineTask.UpdateProgress;

public class UpdateTaskProgressUseCase : IUpdateTaskProgressUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateProvider _dateProvider;
    public UpdateTaskProgressUseCase(ILoggedUser loggedUser, 
        ITaskUpdateOnlyRepository updateRepository, 
        IUnitOfWork unitOfWork, 
        IDateProvider dateProvider)
    {
        _loggedUser = loggedUser;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
        _dateProvider = dateProvider;
    }
    public async Task ExecuteIncrement(long taskId, bool force = false)
    {
        var task = await GetTaskOrThrow(taskId);
        
        const ProgressOperation operation = ProgressOperation.Increment;
        
        ValidateProgressChange(task, operation, force);
        
        task.Progress += operation.ToInt();
        task.IsCompleted = task.Progress == task.WeeklyGoal;
        task.ModifiedAt = _dateProvider.UseCaseDate;
        
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
        task.ModifiedAt = _dateProvider.UseCaseDate;
        
        _updateRepository.Update(task);
        await _unitOfWork.Commit();
    }
    private void ValidateProgressChange(TaskEntity task, ProgressOperation action, bool force = false)
    {
        if (task.IsInCurrentWeek(_dateProvider).IsFalse())
            throw new ConflictException(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);
        switch (action)
        {
            case ProgressOperation.Increment when task.IsCompleted:
                throw new ConflictException(ResourceMessagesException.NOT_INCREMENT_COMPLETED_TASK);
            
            case ProgressOperation.Increment when task.WasModifiedToday(_dateProvider) && force.IsFalse():
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
