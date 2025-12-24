using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Repositories.Token;

public interface ITokenRepository
{
    Task<RefreshToken?> Get(string refreshToken);
    Task SaveNewRefreshToken(RefreshToken refreshToken);   
}