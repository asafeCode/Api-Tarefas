using Microsoft.Extensions.Options;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure.Services.Tokens;

public class VerificationTokenGenerator : IVerificationTokenGenerator
{
    private readonly VerificationTokenSettings _options;
    public VerificationTokenGenerator(IOptions<VerificationTokenSettings> options) => _options = options.Value;
    
    public VerificationToken CreateToken(long userId)
    {
        return new VerificationToken
        {
            UserId = userId,
            Value = Guid.NewGuid(),
            ExpiresOn = DateTime.UtcNow.AddMinutes(_options.ExpirationTimeMinutes),
        };
    }
}