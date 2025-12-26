using FluentValidation;
using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Application.Mappers;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Security;
using UsersModule.Domain.Services.Tokens;

namespace UsersModule.Application.UseCases.Auth.Register;
public class RegisterUserHandler 
{
    private readonly IUserWriteOnlyRepository  _userWriteOnlyRepository;
    private readonly IUserInternalRepository _internalRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IVerificationTokenGenerator _verificationToken;
    private readonly IVerificationLinkGenerators _linkGenerators;
    private readonly IEmailPublisher _publisher;
    private readonly ITokenRepository _tokenRepository;

    public RegisterUserHandler(
        IUserWriteOnlyRepository userWriteOnlyRepository, 
        IUserInternalRepository internalRepository, 
        IUnitOfWork unitOfWork, 
        IPasswordEncripter passwordEncripter, 
        IVerificationTokenGenerator verificationToken, 
        IVerificationLinkGenerators linkGenerators, 
        IEmailPublisher publisher, 
        ITokenRepository tokenRepository
        )
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _internalRepository = internalRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _verificationToken = verificationToken;
        _linkGenerators = linkGenerators;
        _publisher = publisher;
        _tokenRepository = tokenRepository;
    }
    public async Task<ResponseRegisteredUserJson> Handle(RegisterUserCommand request)
    {
        await ValidateAsync(request);
        var user = request.ToUser(_passwordEncripter);
        
        await _userWriteOnlyRepository.AddUserAsync(user);
        await _unitOfWork.Commit();
        
        var token = _verificationToken.CreateToken(user.Id);
        var verificationLink = _linkGenerators.CreateAccountVerificationLink(token);
        
        await _tokenRepository.AddVerificationToken(token);
        await _unitOfWork.Commit();
        
        await _publisher.SendAccountVerificationAsync(user.Email, verificationLink);
        
        return new ResponseRegisteredUserJson
        {
            Message = $"Confirm your account: {user.Email}", 
        };
    }
    
    private async Task ValidateAsync(RegisterUserCommand request)
    {
        var emailExists = await _internalRepository.ExistsUserWithEmail(request.Email);
        if (emailExists)
            throw new ValidationException(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
    }
}