using TarefasCrud.Shared.Constants;
using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Events.Publishers;
using Wolverine;

namespace UsersModule.Infrastructure.Services.Events.Publishers;

public sealed class UserDeletedPublisher(IMessageBus bus) : IUserDeletedPublisher
{
    public async Task SendAsync(Guid userId, string email) => 	
        await bus.SendAsync(new UserDeletedEvent(userId, email));

}