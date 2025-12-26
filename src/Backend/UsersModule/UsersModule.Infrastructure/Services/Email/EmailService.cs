using FluentEmail.Core;
using UsersModule.Domain.Services.Email;

namespace UsersModule.Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _sender;
    public EmailService(IFluentEmail sender)
    {
        _sender = sender;
    }
    public async Task SendAccountVerificationEmail(string email, string verificationLink)
    {
        await _sender
            .To(email)
            .Subject("Email verification for Pontuae")
            .Body($"To verify your account click on the link below: <a href='{verificationLink}'>Click Here</a>", true)
            .SendAsync();
    }
    public async Task SendAccountRecoveryEmail(string email, string verificationLink)
    {
        await _sender
            .To(email)
            .Subject("Account Recovery for Pontuae")
            .Body($"To recover your account click on the link below: <a href='{verificationLink}'>Click Here</a>", true)
            .SendAsync();
    }

    public async Task SendDeleteCompletedEmail(string email)
    {
        await _sender
            .To(email)
            .Subject("Your account has been deleted")
            .Body($"Hi, Your account has been deleted successfully. This action is permanent and cannot be undone. Thanks.", true)
            .SendAsync();
    }    
}