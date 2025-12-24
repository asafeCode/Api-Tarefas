namespace UsersModule.Domain.Repositories.EmailVerificationToken;

public interface IEmailVerifyReadRepository
{
    public Task<ValueObjects.EmailVerificationToken?> Get(Guid token);
}