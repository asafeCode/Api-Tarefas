using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Services;

public interface IEmailVerificationTokenGenerator
{
    public EmailVerificationToken CreateToken(long userId);
}