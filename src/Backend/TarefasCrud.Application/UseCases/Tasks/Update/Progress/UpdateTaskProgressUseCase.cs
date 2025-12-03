using TarefasCrud.Domain.Enums;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Domain.ValueObjects;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.Tasks.Update.Progress;

public class UpdateTaskProgressUseCase : IUpdateTaskProgressUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateTaskProgressUseCase(ILoggedUser loggedUser, 
        ITaskUpdateOnlyRepository updateRepository, 
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
    }
    public async System.Threading.Tasks.Task Execute(long taskId, ProgressOperation operation)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _updateRepository.GetById(loggedUser, taskId);
        
        if (task is null)
            throw new NotFoundException("Tarefa não encontrada!");

        if (operation == ProgressOperation.Increment && task.IsCompleted)
            throw new ConflictException("A tarefa já está concluída e não pode ser incrementada.");
        
        if (operation == ProgressOperation.Decrement && task.Progress == TarefasCrudRuleConstants.INITIAL_PROGRESS)
            throw new ConflictException("A tarefa já está com nenhum progresso e não pode ser decrementada.");
        
        if (operation == ProgressOperation.Decrement && task.IsCompleted)
            task.IsCompleted = false;
        
        task.Progress += operation.ToInt();
        
        if (task.Progress == task.WeeklyGoal)
            task.IsCompleted = true;
        
        _updateRepository.Update(task);
        await _unitOfWork.Commit();
    }
}