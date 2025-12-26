namespace UsersModule.Domain.Repositories.User;

public interface IUserInternalRepository
{
    public Task<TarefasCrud.Shared.SharedEntities.User?> GetUserById(Guid userId);
    public Task<bool> ExistsUserWithEmail(string email);
    public Task<TarefasCrud.Shared.SharedEntities.User?> GetUserByEmail(string email);
}