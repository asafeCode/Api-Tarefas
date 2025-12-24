namespace UsersModule.Domain.Services.Tokens;

public interface IAccessTokenValidator
{
    public Guid ValidateAndGetUserId(string token);
}