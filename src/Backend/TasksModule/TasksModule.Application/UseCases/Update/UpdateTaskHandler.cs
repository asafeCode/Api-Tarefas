using FluentValidation;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;
using TasksModule.Domain.Services;

namespace TasksModule.Application.UseCases.Update;

public class UpdateTaskHandler(
    ITaskUpdateOnlyRepository repository, 
    ILoggedUser loggedUser, 
    IUnitOfWork unitOfWork, 
    ISystemClock systemClock)
{
    public async Task Handle(UpdateTaskCommand command)
    {
        var request = command.Request;
        var userLogged = await loggedUser.User();
        
        var task = await repository.GetById(userLogged, command.TaskId);
        
        if (task is null) throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        Validate(request, task);

        task.Title = request.Title;
        
        if (request.Description.NotEmpty())
            task.Description = request.Description;
        
        task.StartDate = request.StartDate;
        task.Category = request.Category;
        task.WeeklyGoal = request.WeeklyGoal;

        task.IsCompleted = task.Progress == task.WeeklyGoal;
        
        repository.Update(task);
        await unitOfWork.Commit();
    }
    private void Validate(UpdateTaskRequest request, TaskEntity task)
    {
        var date = systemClock.UseCaseDate.ToDateOnly();;
        var validator = new UpdateTaskValidator(date, task);
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        throw new ValidationException(result.Errors);
    }
}