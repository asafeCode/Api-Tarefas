using Microsoft.Extensions.Options;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure.Services.Tokens;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly RefreshTokenSettings _settings;

    public RefreshTokenGenerator(IOptions<RefreshTokenSettings> settings)
    {
        _settings = settings.Value;
    }
    public RefreshToken CreateToken(long userId)
    {
        return new RefreshToken
        {
            Value = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            UserId = userId,
            ExpiresOn = DateTime.UtcNow.AddDays(_settings.ExpirationTimeDays)
        };
    } 
}