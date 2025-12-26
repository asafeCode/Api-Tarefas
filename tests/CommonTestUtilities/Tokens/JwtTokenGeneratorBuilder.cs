using Microsoft.Extensions.Options;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure.Services.Tokens;
using UsersModule.Infrastructure.Settings;

namespace CommonTestUtilities.Tokens;

public static class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var settings = Options.Create(new JwtSettings
        {
            ExpirationTimeMinutes = 15,
            SigningKey = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08"
        });
        
        return new JwtTokenGenerator(settings);
    }
}