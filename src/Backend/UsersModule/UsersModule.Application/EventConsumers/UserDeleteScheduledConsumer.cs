using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Events.Publishers;
using UsersModule.Domain.Repositories.User;

namespace UsersModule.Application.EventConsumers;

public class UserDeleteScheduledConsumer
{
    private readonly IUserDeletedPublisher _bus;
    private readonly IUserInternalRepository _repository;

    public UserDeleteScheduledConsumer(IUserDeletedPublisher bus, 
        IUserInternalRepository repository)
    {
        _bus = bus;
        _repository = repository;
    }
    public async Task Handle(UserDeleteScheduledEvent @event) => await _bus.SendDeleteAsync(@event.UserId, @event.Email);
}
