using FluentValidation.Results;
using TarefasCrud.Core.Exceptions;
using TarefasCrud.Exceptions;
using UsersModule.Application.SharedValidators;
using UsersModule.Application.Validators;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;

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
    public async Task Handle(ChangePasswordCommand request)
    {
        var loggedUser = await _loggedUser.User();
        Validate(request, loggedUser.Password);
        
        var user = await _repository.GetUserById(loggedUser.Id);
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);
        
        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private void Validate(ChangePasswordCommand request, string currentPassword)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);
        
        if (result.IsValid.IsFalse())
            HandleValidationResult.ThrowError(result);
        
        if (_passwordEncripter.IsValid(request.Password, currentPassword))
            return;
        
        result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        HandleValidationResult.ThrowError(result);
    }
}