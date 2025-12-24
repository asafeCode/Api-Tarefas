using FluentValidation.Results;
using TarefasCrud.Exceptions;
using UsersModule.Application.SharedValidators;
using UsersModule.Application.Validators;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;

namespace UsersModule.Application.UseCases.User.Update;

public class UpdateUserHandler 
{
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly ILoggedUser  _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(IUserUpdateOnlyRepository repository, 
        IUserReadOnlyRepository readOnlyRepository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateUserCommand request)
    {
        var loggedUser = await _loggedUser.User();
        await Validate(request, loggedUser.Email);
        
        var user = await _repository.GetUserById(loggedUser.Id);
        user.Name = request.Name;
        user.Email = request.Email;
        
        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(UpdateUserCommand request,  string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var result = await validator.ValidateAsync(request);
        
        if (result.IsValid.IsFalse())
            HandleValidationResult.ThrowError(result);
        
        if (currentEmail.Equals(request.Email).IsFalse())
        {
            var emailExists = await _readOnlyRepository.ExistsActiveUserWithEmail(request.Email);
            if (emailExists)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
                HandleValidationResult.ThrowError(result);
            }
        }
    }
}