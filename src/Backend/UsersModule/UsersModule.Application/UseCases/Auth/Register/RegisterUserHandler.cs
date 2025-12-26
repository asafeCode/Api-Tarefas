using FluentValidation;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Application.Mappers;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Security;
using UsersModule.Domain.Services.Tokens;

namespace UsersModule.Application.UseCases.Auth.Register;
public class RegisterUserHandler(IUserWriteOnlyRepository userWriteOnlyRepository, 
    IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork, 
    IPasswordEncripter passwordEncripter, 
    IEmailVerificationTokenGenerator emailVerificationToken, 
    IEmailVerificationLinkGenerator emailVerificationLink, 
    IEmailVerifiedPublisher publisher, 
    ITokenRepository tokenRepository) 
{
    public async Task<ResponseRegisteredUserJson> Handle(RegisterUserCommand request)
    {
        await ValidateAsync(request);
        var user = request.ToUser(passwordEncripter);
        
        await userWriteOnlyRepository.AddUserAsync(user);
        await unitOfWork.Commit();
        
        var token = emailVerificationToken.CreateToken(user.Id);
        var verificationLink = emailVerificationLink.CreateLink(token);
        
        await tokenRepository.AddVerificationToken(token);
        await unitOfWork.Commit();
        
        await publisher.SendAsync(user.Email, verificationLink);
        
        return new ResponseRegisteredUserJson
        {
            Message = $"Confirm your account: {user.Email}", 
        };
    }
    
    private async Task ValidateAsync(RegisterUserCommand request)
    {
        var emailExists = await userReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);
        if (emailExists)
            throw new ValidationException(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
    }
}