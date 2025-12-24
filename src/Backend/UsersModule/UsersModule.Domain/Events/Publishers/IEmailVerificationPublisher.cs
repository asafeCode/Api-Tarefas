namespace UsersModule.Domain.Events.Publishers;

public interface IEmailVerificationPublisher
{
    Task SendAsync(string email, string verificationlink);
}