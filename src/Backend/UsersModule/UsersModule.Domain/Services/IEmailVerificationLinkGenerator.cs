using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Services;

public interface IEmailVerificationLinkGenerator
{
    public string CreateLink(EmailVerificationToken emailVerificationToken);
    
}