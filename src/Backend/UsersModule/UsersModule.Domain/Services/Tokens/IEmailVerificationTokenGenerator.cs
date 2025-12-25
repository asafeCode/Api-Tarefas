using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Services.Tokens;

public interface IEmailVerificationTokenGenerator
{
    public EmailVerificationToken CreateToken(long userId);
}