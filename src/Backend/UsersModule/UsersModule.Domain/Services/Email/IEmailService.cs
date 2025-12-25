namespace UsersModule.Domain.Services.Email;

public interface IEmailService
{
    Task SendAccountVerificationEmail(string email, string verificationLink);
    Task SendDeleteCompletedEmail(string email);
}