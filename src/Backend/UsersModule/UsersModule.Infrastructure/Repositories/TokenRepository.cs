using Microsoft.EntityFrameworkCore;
using TarefasCrud.Infrastructure;
using TarefasCrud.Infrastructure.DataAccess;
using UsersModule.Domain.Repositories.Token;
using UsersModule.Domain.ValueObjects;

namespace UsersModule.Infrastructure.Repositories;

public sealed class TokenRepository(TarefasCrudDbContext dbContext) :  ITokenRepository
{
    public async Task<RefreshToken?> GetRefreshToken(string refreshToken) => await dbContext
        .RefreshTokens
        .AsNoTracking()
        .Include(token => token.User)
        .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    public async Task AddRefreshToken(RefreshToken refreshToken)
    { 
        var tokens = dbContext.RefreshTokens
            .Where(token => token.UserId == refreshToken.UserId);
        
        dbContext.RefreshTokens.RemoveRange(tokens);
        await dbContext.RefreshTokens.AddAsync(refreshToken);
    }
    public async Task<EmailVerificationToken?> GetEmailVerificationToken(Guid token) => await dbContext
        .EmailVerificationTokens
        .Include(e => e.User)
        .FirstOrDefaultAsync(verificationToken => token.Equals(verificationToken.Value));
    public async Task AddVerificationToken(EmailVerificationToken emailVerificationToken) 
    {
        var tokens = dbContext.EmailVerificationTokens
            .Where(token => token.UserId == emailVerificationToken.UserId);
        
        dbContext.EmailVerificationTokens.RemoveRange(tokens);
        await dbContext.EmailVerificationTokens.AddAsync(emailVerificationToken);
    }
}