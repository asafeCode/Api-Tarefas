using UsersModule.Domain.Events.Publishers;

namespace UsersModule.Infrastructure.Services.Events.Publishers;

internal sealed class EmailVerificationPublisher : IEmailVerificationPublisher
{
    public Task SendAsync(string email, string verificationlink)
    {
        throw new NotImplementedException();
    }
}