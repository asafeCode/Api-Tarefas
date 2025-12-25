namespace UsersModule.Domain.Events.EventsDtos;

public record UserDeletedEvent(Guid UserId, string Email);