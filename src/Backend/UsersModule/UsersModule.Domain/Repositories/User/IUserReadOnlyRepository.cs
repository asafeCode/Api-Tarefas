namespace UsersModule.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithIdentifier(Guid userId);
    public Task<TarefasCrud.Shared.SharedEntities.User?> GetActiveUserById(Guid userId);
}