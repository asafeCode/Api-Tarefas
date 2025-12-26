using FluentValidation;
using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Services;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Security;

namespace UsersModule.Application.UseCases.User.ChangePassword;

public class ChangePasswordHandler 
{
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserUpdateOnlyRepository  _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangePasswordHandler(
        IPasswordEncripter passwordEncripter, 
        IUserUpdateOnlyRepository repository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _passwordEncripter = passwordEncripter;
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(ChangePasswordCommand command)
    {
        var request = command.Request;
        var loggedUser = await _loggedUser.User();
        Validate(request, loggedUser.Password);
        
        var user = await _repository.GetUserById(loggedUser.Id);
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);
        
        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private void Validate(ChangePasswordRequest request, string currentPassword)
    {
        if (_passwordEncripter.IsValid(request.Password, currentPassword).IsFalse())
            throw new ValidationException(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
    }
}