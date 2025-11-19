using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.User;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public UserRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
    
    public async Task<bool> ExistsActiveUserWithEmail(string email) => await _dbContext
        .Users
        .AnyAsync(user => user.Email.Equals(email) && user.Active);
}