using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UsersModule.Domain.Services.Tokens;
using UsersModule.Domain.ValueObjects;

namespace UsersModule.Infrastructure.Services.Tokens;

public class VerificationLinkGenerators : IVerificationLinkGenerators
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;
    public VerificationLinkGenerators(
        IHttpContextAccessor httpContextAccessor, 
        LinkGenerator linkGenerator)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
    }

    public string CreateAccountVerificationLink(VerificationToken verificationToken)
    {
        var verificationLink = _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, 
            endpointName: "VerifyEmail",
            values: new { Token = verificationToken.Value });
        
        return verificationLink ?? throw new InvalidOperationException("Could not create email verification link");
    }

    public string CreateAccountRecoveryLink(VerificationToken verificationToken)
    {
        var verificationLink = _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, 
            endpointName: "AccountRecovery",
            values: new { Token = verificationToken.Value });
        
        return verificationLink ?? throw new InvalidOperationException("Could not create account recovery verification link");
    }
}