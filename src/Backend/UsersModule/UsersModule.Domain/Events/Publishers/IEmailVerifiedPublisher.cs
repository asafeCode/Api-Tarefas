namespace UsersModule.Domain.Events.Publishers;

public interface IEmailVerifiedPublisher
{
    Task SendAsync(string email, string verificationlink);
}