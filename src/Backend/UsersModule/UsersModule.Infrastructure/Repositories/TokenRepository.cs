using Microsoft.EntityFrameworkCore;
using TarefasCrud.Infrastructure;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.ValueObjects;

namespace UsersModule.Infrastructure.Repositories;

internal sealed class TokenRepository :  ITokenRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public TokenRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    public async Task<RefreshToken?> GetRefreshToken(string refreshToken) => await _dbContext
        .RefreshTokens
        .AsNoTracking()
        .Include(token => token.User)
        .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    public async Task AddRefreshToken(RefreshToken refreshToken)
    { 
        var tokens = _dbContext.RefreshTokens
            .Where(token => token.UserId == refreshToken.UserId);
        
        _dbContext.RefreshTokens.RemoveRange(tokens);
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }
    public async Task<EmailVerificationToken?> GetEmailVerificationToken(Guid token) => await _dbContext
        .EmailVerificationTokens
        .Include(e => e.User)
        .FirstOrDefaultAsync(verificationToken => token.Equals(verificationToken.Value));
    public async Task AddVerificationToken(EmailVerificationToken emailVerificationToken) 
    {
        var tokens = _dbContext.EmailVerificationTokens
            .Where(token => token.UserId == emailVerificationToken.UserId);
        
        _dbContext.EmailVerificationTokens.RemoveRange(tokens);
        await _dbContext.EmailVerificationTokens.AddAsync(emailVerificationToken);
    }
}