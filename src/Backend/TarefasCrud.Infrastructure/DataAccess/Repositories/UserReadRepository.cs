using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.User;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class UserReadRepository : IUserReadOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public UserReadRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task<bool> ExistsActiveUserWithEmail(string email) => await _dbContext
        .Users
        .AnyAsync(user => user.Email.Equals(email) && user.Active);
    public async Task<User?> GetUserByEmail(string email) => await _dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Active);
    public async Task<bool> ExistActiveUserWithIdentifier(Guid userId) => await _dbContext
        .Users
        .AnyAsync(user => user.UserId.Equals(userId) && user.Active);
    public async Task<User?> GetByUserIdentifier(Guid userId) => await _dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.UserId.Equals(userId) && user.Active);
}