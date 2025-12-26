using UsersModule.Domain.ValueObjects;

namespace UsersModule.Domain.Services.Tokens;

public interface IVerificationLinkGenerators
{
    public string CreateAccountVerificationLink(VerificationToken verificationToken);
    public string CreateAccountRecoveryLink(VerificationToken verificationToken);
    
}