using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using TasksModule.Domain.Repositories;

namespace TasksModule.Application.UseCases.Delete;

public class DeleteTaskHandler(ILoggedUser loggedUser, 
    ITaskReadOnlyRepository readRepository, 
    ITaskWriteOnlyRepository writeRepository, 
    IUnitOfWork unitOfWork)
{
    public async Task Handle(DeleteTaskCommand command)
    {
        var userLogged = await loggedUser.User();
        var task = await readRepository.GetById(userLogged, command.TaskId);

        if (task is null)
            throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        await writeRepository.Delete(command.TaskId);
        await unitOfWork.Commit();
    }
}