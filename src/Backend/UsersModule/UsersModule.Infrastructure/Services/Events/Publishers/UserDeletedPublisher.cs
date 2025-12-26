using TarefasCrud.Shared.Constants;
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

    public async Task SendAsync(Guid userId, string email) => await _messageBus
        .SendAsync(new UserDeletedEvent(userId, email));
}