using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;

namespace UsersModule.Application.UseCases.User.Delete;

public class RequestDeleteUserHandler :  IRequestDeleteUserUseCase
{
    private readonly ILoggedUser  _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserUpdateOnlyRepository _updateRepository;
    private readonly IDeleteUserQueue _busQueue;
    public RequestDeleteUserHandler(ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork, 
        IUserUpdateOnlyRepository updateRepository, 
        IDeleteUserQueue busQueue)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _updateRepository = updateRepository;
        _busQueue = busQueue;
    }
    public async Task Execute(bool force)
    {
        if (force.IsFalse())
            throw new ConflictException(ResourceMessagesException.CONFIRMATION_REQUIRED_TO_DELETE_ACCOUNT);
        var loggedUser = await _loggedUser.User();
        var user = await _updateRepository.GetUserById(loggedUser.Id);

        user.Active = false;
        _updateRepository.Update(user);
        await _unitOfWork.Commit();
        await _busQueue.SendAsync(loggedUser);
    }
}