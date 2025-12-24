namespace UsersModule.Domain.Services.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(Guid userId);
}