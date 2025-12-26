using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Events.Publishers;
using Wolverine;

namespace UsersModule.Infrastructure.Services.Events.Publishers;

public sealed class EmailPublisher : IEmailPublisher
{
    private readonly IMessageBus _messageBus;

    public EmailPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }
    public async Task SendAccountVerificationAsync(string email, string verificationLink) => 
        await _messageBus.SendAsync(new EmailVerifiedEvent(email, verificationLink));

    public async Task SendAccountRecoveryAsync(string email, string verificationLink) => 
        await _messageBus.SendAsync(new AccountRecoveredEvent(email, verificationLink));
}