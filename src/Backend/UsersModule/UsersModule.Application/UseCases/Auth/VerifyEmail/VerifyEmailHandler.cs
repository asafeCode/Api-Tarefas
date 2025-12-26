using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;

namespace UsersModule.Application.UseCases.Auth.VerifyEmail;

public class VerifyEmailHandler(ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
{
    public async Task Handle(VerifyEmailCommand command)
    {
        var token = await tokenRepository.GetEmailVerificationToken(command.TokenValue);
        if (token is null || token.ExpiresOn < DateTime.UtcNow || token.User.EmailConfirmed) throw new ExpiredTokenException();
        
        token.User.EmailConfirmed = true;
        await unitOfWork.Commit();
    }
}