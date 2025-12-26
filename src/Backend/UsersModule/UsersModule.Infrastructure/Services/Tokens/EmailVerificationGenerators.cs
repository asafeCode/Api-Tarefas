using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using UsersModule.Domain.Services;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure.Services.Tokens;

public class EmailVerificationGenerators(
    IHttpContextAccessor httpContextAccessor, 
    LinkGenerator linkGenerator, 
    IOptions<EmailVerificationSettings> options
    ) : IEmailVerificationLinkGenerator, IEmailVerificationTokenGenerator
{
    public string CreateLink(EmailVerificationToken emailVerificationToken)
    {
        var verificationLink = linkGenerator.GetUriByName(httpContextAccessor.HttpContext!, 
            endpointName: "VerifyEmail",
            values: new { Token = emailVerificationToken.Value });
        
        return verificationLink ?? throw new InvalidOperationException("Could not create email verification link");
    }

    public EmailVerificationToken CreateToken(long userId)
    {
        return new EmailVerificationToken
        {
            UserId = userId,
            Value = Guid.NewGuid(),
            ExpiresOn = DateTime.UtcNow.AddMinutes(options.Value.ExpirationTimeMinutes),
        };
    }
}