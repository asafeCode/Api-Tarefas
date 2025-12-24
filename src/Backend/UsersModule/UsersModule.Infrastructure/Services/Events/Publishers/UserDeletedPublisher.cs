using UsersModule.Domain.Events.Publishers;

namespace UsersModule.Infrastructure.Services.Events.Publishers;

internal sealed class UserDeletedPublisher : IUserDeletedPublisher
{
    public Task SendAsync(long userId)
    {
        throw new NotImplementedException();
    }
}