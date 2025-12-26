namespace UsersModule.Domain.Events.Publishers;

public interface IUserDeletedPublisher
{
    Task SendDeleteAsync(Guid userId, string email);
    Task SendScheduleAsync(Guid userId, string email);
}