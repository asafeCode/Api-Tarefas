using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Services.Tokens;

public interface IVerificationTokenGenerator
{
    public VerificationToken CreateToken(long userId);
}