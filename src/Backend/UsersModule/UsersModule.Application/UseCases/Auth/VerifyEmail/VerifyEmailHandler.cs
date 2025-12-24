using TarefasCrud.Exceptions.ExceptionsBase;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.EmailVerificationToken;

namespace UsersModule.Application.UseCases.Auth.VerifyEmail;

public class VerifyEmailHandler 
{
    private readonly IEmailVerifyReadRepository _emailVerifyReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailHandler(IEmailVerifyReadRepository emailVerifyReadRepository, IUnitOfWork unitOfWork)
    {
        _emailVerifyReadRepository = emailVerifyReadRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(Guid tokenId)
    {
        var token = await _emailVerifyReadRepository.Get(tokenId);
        if (token is null || token.ExpiresOn < DateTime.UtcNow || token.User.EmailConfirmed) throw new ExpiredTokenException();
        
        token.User.EmailConfirmed = true;
        await _unitOfWork.Commit();
    }
}