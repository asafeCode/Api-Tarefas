using FluentValidation.Results;
using TarefasCrud.Communication.Responses.UsersModule;
using TarefasCrud.Exceptions;
using UsersModule.Application.Mappers;
using UsersModule.Application.SharedValidators;
using UsersModule.Application.Validators;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.EmailVerificationToken;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services;

namespace UsersModule.Application.UseCases.Auth.Register;
public class RegisterUserHandler 
{
    private readonly IUserWriteOnlyRepository  _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IEmailVerificationTokenGenerator _emailVerificationToken;
    private readonly IEmailVerificationLinkGenerator _emailVerificationLink;
    private readonly IEmailVerifyWriteRepository _emailVerifyWriteRepository;
    private readonly IEmailVerificationPublisher _publisher;

    public RegisterUserHandler(IUserWriteOnlyRepository userWriteOnlyRepository, 
        IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork, 
        IPasswordEncripter passwordEncripter, 
        IEmailVerificationTokenGenerator emailVerificationToken, 
        IEmailVerificationLinkGenerator emailVerificationLink, 
        IEmailVerifyWriteRepository emailVerifyWriteRepository, 
        IEmailVerificationPublisher publisher)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _emailVerificationToken = emailVerificationToken;
        _emailVerificationLink = emailVerificationLink;
        _emailVerifyWriteRepository = emailVerifyWriteRepository;
        _publisher = publisher;
    }
    public async Task<ResponseRegisteredUserJson> Handle(RegisterUserCommand request)
    {
        await ValidateAsync(request);
        var user = request.ToUser(_passwordEncripter);
        
        await _userWriteOnlyRepository.AddUserAsync(user);
        await _unitOfWork.Commit();

        var token = _emailVerificationToken.CreateToken(user.Id);
        var verificationLink = _emailVerificationLink.CreateLink(token);
        
        await _emailVerifyWriteRepository.AddTokenAsync(token);
        await _unitOfWork.Commit();
        
        await _publisher.SendAsync(user.Email, verificationLink);
        
        return new ResponseRegisteredUserJson
        {
            Message = $"Confirm your account: {user.Email}", 
        };
    }
    
    private async Task ValidateAsync(RegisterUserCommand request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);
        
        if (result.IsValid.IsFalse())
            HandleValidationResult.ThrowError(result);
        
        var emailExists = await _userReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);
        if (emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            HandleValidationResult.ThrowError(result);
        }
    }
}