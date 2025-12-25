namespace UsersModule.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistsActiveUserWithEmail(string email);
    public Task<TarefasCrud.Shared.SharedEntities.User?> GetUserByEmail(string email);
    public Task<bool> ExistActiveUserWithIdentifier(Guid userId);
    public Task<TarefasCrud.Shared.SharedEntities.User?> GetByUserIdentifier(Guid userId);
}