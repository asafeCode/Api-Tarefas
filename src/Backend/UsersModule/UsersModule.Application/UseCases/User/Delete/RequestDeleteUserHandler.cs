using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;

namespace UsersModule.Application.UseCases.User.Delete;

public class RequestDeleteUserHandler(
    ILoggedUser loggedUser, 
    IUnitOfWork unitOfWork, 
    IUserUpdateOnlyRepository updateRepository, 
    IUserDeletedPublisher publisher)
{
    public async Task Handle(RequestDeleteUserCommand command)
    {
        if (command.Force.IsFalse())
            throw new ConflictException(ResourceMessagesException.CONFIRMATION_REQUIRED_TO_DELETE_ACCOUNT);
        var userLogged = await loggedUser.User();
        var user = await updateRepository.GetUserById(userLogged.Id);

        user.Active = false;
        updateRepository.Update(user);
        
        await unitOfWork.Commit();
        await publisher.SendAsync(userLogged.UserId, userLogged.Email);
    }
}