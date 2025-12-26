using FluentEmail.Core;
using UsersModule.Domain.Services.Email;

namespace UsersModule.Infrastructure.Services.Email;

public class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendAccountVerificationEmail(string email, string verificationLink)
    {
        await fluentEmail
            .To(email)
            .Subject("Email verification for Pontuae")
            .Body($"To verify your account click on the link below: <a href='{verificationLink}'>Click Here</a>", true)
            .SendAsync();
    }

    public async Task SendDeleteCompletedEmail(string email)
    {
        await fluentEmail
            .To(email)
            .Subject("Your account has been deleted")
            .Body($"Hi, Your account has been deleted successfully. This action is permanent and cannot be undone. Thanks.", true)
            .SendAsync();
    }
}