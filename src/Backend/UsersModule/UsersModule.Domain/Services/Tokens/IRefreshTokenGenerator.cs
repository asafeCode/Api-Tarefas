namespace UsersModule.Domain.Services.Tokens;

public interface IRefreshTokenGenerator
{
    public string Generate();
}