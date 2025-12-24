namespace UsersModule.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    public Task AddUserAsync(Entities.User user);
    public Task DeleteAccount(Guid userId);
}