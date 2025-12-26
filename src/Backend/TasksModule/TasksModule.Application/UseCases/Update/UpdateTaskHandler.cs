using FluentValidation;
using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;

namespace TasksModule.Application.UseCases.Update;

public class UpdateTaskHandler
{
    private readonly ITaskUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _systemClock;
    
    public UpdateTaskHandler(ITaskUpdateOnlyRepository repository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork, 
        ISystemClock systemClock)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
    }
    public async Task Handle(UpdateTaskCommand command)
    {
        var request = command.Request;
        var loggedUser = await _loggedUser.User();
        
        var task = await _repository.GetById(loggedUser, command.TaskId);
        
        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
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
    private void Validate(UpdateTaskRequest request, TaskEntity task)
    {
        var date = _systemClock.UseCaseDate.ToDateOnly();;
        var validator = new UpdateTaskValidator(date, task);
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        throw new ValidationException(result.Errors);
    }
}