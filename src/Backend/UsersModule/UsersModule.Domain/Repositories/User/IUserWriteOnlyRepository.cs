namespace UsersModule.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    public Task AddUserAsync(TarefasCrud.Shared.SharedEntities.User user);
    public Task DeleteAccount(Guid userId);
}