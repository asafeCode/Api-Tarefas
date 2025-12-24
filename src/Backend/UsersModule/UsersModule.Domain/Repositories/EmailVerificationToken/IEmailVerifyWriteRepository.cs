namespace UsersModule.Domain.Repositories.EmailVerificationToken;

public interface IEmailVerifyWriteRepository
{
    public Task AddTokenAsync(ValueObjects.EmailVerificationToken emailVerificationToken);
    
}