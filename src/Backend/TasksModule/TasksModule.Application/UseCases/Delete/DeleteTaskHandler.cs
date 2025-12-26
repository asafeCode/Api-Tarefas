using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Delete;

public class DeleteTaskHandler 
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskReadOnlyRepository _readRepository;
    private readonly ITaskWriteOnlyRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskHandler(ILoggedUser loggedUser, 
        ITaskReadOnlyRepository readRepository, 
        ITaskWriteOnlyRepository writeRepository, 
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTaskCommand command)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _readRepository.GetById(loggedUser, command.TaskId);

        if (task is null)
            throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        await _writeRepository.Delete(command.TaskId);
        await _unitOfWork.Commit();
    }
}