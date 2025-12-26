using Microsoft.Extensions.Options;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure.Services.Tokens;

public sealed class RefreshTokenGenerator(IOptions<RefreshTokenSettings> settings) : IRefreshTokenGenerator
{
    public RefreshToken CreateToken(long userId)
    {
        return new RefreshToken
        {
            Value = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            UserId = userId,
            ExpiresOn = DateTime.UtcNow.AddDays(settings.Value.ExpirationTimeDays)
        };
    } 
}