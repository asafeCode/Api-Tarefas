using FluentValidation;
using TarefasCrud.Core.Exceptions;
using TarefasCrud.Core.Responses.UsersModule;
using UsersModule.Application.Mappers;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;
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
    private readonly IEmailVerificationPublisher _publisher;
    private readonly ITokenRepository _tokenRepository;

    public RegisterUserHandler(IUserWriteOnlyRepository userWriteOnlyRepository, 
        IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork, 
        IPasswordEncripter passwordEncripter, 
        IEmailVerificationTokenGenerator emailVerificationToken, 
        IEmailVerificationLinkGenerator emailVerificationLink, 
        IEmailVerificationPublisher publisher, 
        ITokenRepository tokenRepository)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _emailVerificationToken = emailVerificationToken;
        _emailVerificationLink = emailVerificationLink;
        _publisher = publisher;
        _tokenRepository = tokenRepository;
    }
    public async Task<ResponseRegisteredUserJson> Handle(RegisterUserCommand request)
    {
        await ValidateAsync(request);
        var user = request.ToUser(_passwordEncripter);
        
        await _userWriteOnlyRepository.AddUserAsync(user);
        await _unitOfWork.Commit();

        var token = _emailVerificationToken.CreateToken(user.Id);
        var verificationLink = _emailVerificationLink.CreateLink(token);
        
        await _tokenRepository.AddVerificationToken(token);
        await _unitOfWork.Commit();
        
        await _publisher.SendAsync(user.Email, verificationLink);
        
        return new ResponseRegisteredUserJson
        {
            Message = $"Confirm your account: {user.Email}", 
        };
    }
    
    private async Task ValidateAsync(RegisterUserCommand request)
    {
        var emailExists = await _userReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);
        if (emailExists)
            throw new ValidationException(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
    }
}