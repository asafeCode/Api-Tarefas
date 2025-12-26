using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Events.Publishers;
using Wolverine;
using TarefasCrudRuleConstants = UsersModule.Domain.ValueObjects.TarefasCrudRuleConstants;

namespace UsersModule.Infrastructure.Services.Events.Publishers;

public sealed class UserDeletedPublisher : IUserDeletedPublisher
{
    private readonly IMessageBus _messageBus;

    public UserDeletedPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task SendDeleteAsync(Guid userId, string email) => await _messageBus
        .SendAsync(new UserDeletedEvent(userId, email));

    public async Task SendScheduleAsync(Guid userId, string email) => await _messageBus
        .ScheduleAsync(new UserDeleteScheduledEvent(userId, email), TarefasCrudRuleConstants.TimeToRecoverAccount());
}