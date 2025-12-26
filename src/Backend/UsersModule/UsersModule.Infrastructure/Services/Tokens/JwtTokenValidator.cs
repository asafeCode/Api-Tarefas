using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure.Services.Tokens;

public sealed class JwtTokenValidator(IOptions<JwtSettings> settings) : JwtTokenHandler, IAccessTokenValidator
{
    public Guid ValidateAndGetUserId(string token)
    {
        var validationParameter = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(settings.Value.SigningKey),
            ClockSkew = new TimeSpan(0)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, validationParameter, out _);

        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(userIdentifier);
    }
}