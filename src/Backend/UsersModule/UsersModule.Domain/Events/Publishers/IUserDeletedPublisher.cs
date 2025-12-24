namespace UsersModule.Domain.Events.Publishers;

public interface IUserDeletedPublisher
{
    Task SendAsync(long userId);
}