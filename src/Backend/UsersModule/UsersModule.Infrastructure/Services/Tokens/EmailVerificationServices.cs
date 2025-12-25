using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using UsersModule.Domain.Services;
using UsersModule.Domain.ValueObjects;
using UsersModule.Infrastructure.Settings;

namespace UsersModule.Infrastructure.Services.Tokens;

public class EmailVerificationServices : IEmailVerificationLinkGenerator, IEmailVerificationTokenGenerator
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;
    private readonly EmailVerificationSettings _options;
    public EmailVerificationServices(
        IHttpContextAccessor httpContextAccessor, 
        LinkGenerator linkGenerator, 
        IOptions<EmailVerificationSettings> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
        _options = options.Value;

    }

    public string CreateLink(EmailVerificationToken emailVerificationToken)
    {
        var verificationLink = _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, 
            endpointName: "VerifyEmail",
            values: new { Token = emailVerificationToken.Value });
        
        return verificationLink ?? throw new InvalidOperationException("Could not create email verification link");
    }

    public EmailVerificationToken CreateToken(long userId)
    {
        return new EmailVerificationToken
        {
            UserId = userId,
            Value = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiresOn = DateTime.UtcNow.AddMinutes(_options.ExpirationTimeMinutes),
        };
    }
}