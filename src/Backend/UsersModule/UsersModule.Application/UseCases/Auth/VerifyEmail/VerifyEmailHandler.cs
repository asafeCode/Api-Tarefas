using TarefasCrud.Core.Exceptions.ExceptionsBase;
using TarefasCrud.Exceptions.ExceptionsBase;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.Token;

namespace UsersModule.Application.UseCases.Auth.VerifyEmail;

public class VerifyEmailHandler 
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailHandler(ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(Guid tokenId)
    {
        var token = await _tokenRepository.GetEmailVerificationToken(tokenId);
        if (token is null || token.ExpiresOn < DateTime.UtcNow || token.User.EmailConfirmed) throw new ExpiredTokenException();
        
        token.User.EmailConfirmed = true;
        await _unitOfWork.Commit();
    }
}