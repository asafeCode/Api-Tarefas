namespace UsersModule.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    public Task<TarefasCrud.Shared.SharedEntities.User> GetUserById(long id);
    public void Update(TarefasCrud.Shared.SharedEntities.User user);
}