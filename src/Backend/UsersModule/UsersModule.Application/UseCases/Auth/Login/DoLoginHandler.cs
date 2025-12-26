using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using TarefasCrud.Shared.Responses.UsersModule;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Extensions;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Security;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;

namespace UsersModule.Application.UseCases.Auth.Login;

public class DoLoginHandler(IUserReadOnlyRepository repository, 
    IPasswordEncripter passwordEncripter, 
    IAccessTokenGenerator tokenGenerator, 
    IRefreshTokenGenerator refreshTokenGenerator, 
    ITokenRepository tokenRepository, 
    IUnitOfWork unitOfWork, 
    IEmailVerificationTokenGenerator emailVerificationTokenGenerator, 
    IEmailVerificationLinkGenerator emailVerificationLinkGenerator, 
    IEmailVerifiedPublisher publisher)
{
    public async Task<ResponseLoggedUserJson> Handle(DoLoginCommand request)
    {
        var user = await repository.GetUserByEmail(request.Email);
        if (user is null || passwordEncripter.IsValid(request.Password, user.Password).IsFalse()) 
            throw new InvalidLoginException();

        if (user.EmailConfirmed.IsFalse())
        {
            var token = await CreateAndSaveVerificationToken(user.Id);
            var verificationLink = emailVerificationLinkGenerator.CreateLink(token);   
            await publisher.SendAsync(user.Email, verificationLink);
            throw new InvalidLoginException($"Account not confirmed. Verification email sent to {user.Email}.");
        }

        var refreshToken = await CreateAndSaveRefreshToken(user.Id);
        return new ResponseLoggedUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = tokenGenerator.Generate(user.UserId),
                RefreshToken = refreshToken
            }
        };
    }
    private async Task<string> CreateAndSaveRefreshToken(long userId)
    {
        var refreshToken = refreshTokenGenerator.CreateToken(userId);
        
        await tokenRepository.AddRefreshToken(refreshToken);

        await unitOfWork.Commit();

        return refreshToken.Value;
    }    
    private async Task<EmailVerificationToken> CreateAndSaveVerificationToken(long userId)
    {
        var token = emailVerificationTokenGenerator.CreateToken(userId);
        await tokenRepository.AddVerificationToken(token);

        await unitOfWork.Commit();

        return token;
    }
}