using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Services.Tokens;

public interface IRefreshTokenGenerator
{
    public RefreshToken CreateToken(long userId);
}