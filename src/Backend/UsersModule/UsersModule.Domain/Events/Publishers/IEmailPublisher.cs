namespace UsersModule.Domain.Events.Publishers;

public interface IEmailPublisher
{
    Task SendAccountVerificationAsync(string email, string verificationLink);
    Task SendAccountRecoveryAsync(string email, string verificationLink);
}