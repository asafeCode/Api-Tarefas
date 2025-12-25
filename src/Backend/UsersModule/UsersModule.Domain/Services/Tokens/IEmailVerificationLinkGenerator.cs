using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Services.Tokens;

public interface IEmailVerificationLinkGenerator
{
    public string CreateLink(EmailVerificationToken emailVerificationToken);
    
}