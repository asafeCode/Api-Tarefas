using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Events.Publishers;
using Wolverine;

namespace UsersModule.Infrastructure.Services.Events.Publishers;

public sealed class UserDeletedPublisher : IUserDeletedPublisher
{
    private readonly IMessageBus _messageBus;

    public UserDeletedPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }
    public async Task SendAsync(Guid userId) => await _messageBus.PublishAsync(new UserDeletedEvent(userId));
}