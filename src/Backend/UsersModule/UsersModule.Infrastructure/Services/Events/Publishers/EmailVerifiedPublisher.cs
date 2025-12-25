using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Events.Publishers;
using Wolverine;

namespace UsersModule.Infrastructure.Services.Events.Publishers;

public sealed class EmailVerifiedPublisher : IEmailVerifiedPublisher
{
    private readonly IMessageBus _messageBus;

    public EmailVerifiedPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }
    public async Task SendAsync(string email, string verificationlink) => 
        await _messageBus.PublishAsync(new EmailVerifiedEvent(email, verificationlink));
}