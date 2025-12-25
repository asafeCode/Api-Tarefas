using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Services.Email;

namespace UsersModule.Application.EventConsumers;

public class EmailVerificatedConsumer
{
    private readonly IEmailService _emailService;

    public EmailVerificatedConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }
    public async Task Handle(EmailVerifiedEvent @event)
    {
        await _emailService.SendAccountVerificationEmail(@event.Email, @event.VerificationLink);
    }
}