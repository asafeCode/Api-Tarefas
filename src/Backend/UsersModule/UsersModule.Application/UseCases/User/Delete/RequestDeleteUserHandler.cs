using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;

namespace UsersModule.Application.UseCases.User.Delete;

public class RequestDeleteUserHandler 
{
    private readonly ILoggedUser  _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserUpdateOnlyRepository _updateRepository;
    private readonly IUserDeletedPublisher _publisher;

    public RequestDeleteUserHandler(ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork, 
        IUserUpdateOnlyRepository updateRepository, 
        IUserDeletedPublisher publisher
        )
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _updateRepository = updateRepository;
        _publisher = publisher;
    }
    public async Task Handle(RequestDeleteUserCommand command)
    {
        if (command.Force.IsFalse())
            throw new ConflictException(ResourceMessagesException.CONFIRMATION_REQUIRED_TO_DELETE_ACCOUNT);
        var loggedUser = await _loggedUser.User();
        var user = await _updateRepository.GetUserById(loggedUser.Id);

        user.Active = false;
        _updateRepository.Update(user);
        await _unitOfWork.Commit();
        await _publisher.SendAsync(loggedUser.UserId, loggedUser.Email);
    }
}