using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Services.Email;

namespace UsersModule.Application.EventConsumers;

public class AccountRecoveredConsumer
{
    private readonly IEmailService _emailService;
    public AccountRecoveredConsumer(IEmailService emailService) => _emailService = emailService;
    
    public async Task Handle(AccountRecoveredEvent @event) => 
        await _emailService.SendAccountRecoveryEmail(@event.Email, @event.VerificationLink);
}