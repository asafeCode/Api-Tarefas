using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Repositories.Token;

public interface ITokenRepository
{
    Task<RefreshToken?> GetRefreshToken(string refreshToken);
    Task AddRefreshToken(RefreshToken refreshToken); 
    Task<VerificationToken?> GetVerificationToken(Guid token);
    Task AddVerificationToken(VerificationToken verificationToken);
}