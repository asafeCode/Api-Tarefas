using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure.Services.Tokens;

public class JwtTokenGenerator(IOptions<JwtSettings> settings) : JwtTokenHandler, IAccessTokenGenerator
{
    public string Generate(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, userId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(settings.Value.ExpirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(settings.Value.SigningKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}