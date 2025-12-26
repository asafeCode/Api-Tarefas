using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using TarefasCrud.Shared.Repositories;
using UsersModule.Domain.Repositories.Token;

namespace UsersModule.Application.UseCases.Auth.RecoverAccount;

public class RecoverAccountHandler 
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RecoverAccountHandler(ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(RecoverAccountCommand command)
    {
        var token = await _tokenRepository.GetVerificationToken(command.TokenValue);
        if (token is null || token.ExpiresOn < DateTime.UtcNow || token.User.Active) throw new ExpiredTokenException();
        
        token.User.Active = true;
        await _unitOfWork.Commit();
    }
} 