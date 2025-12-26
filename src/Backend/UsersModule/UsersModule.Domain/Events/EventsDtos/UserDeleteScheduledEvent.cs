namespace UsersModule.Domain.Events.EventsDtos;

public record UserDeleteScheduledEvent(Guid UserId, string Email);