namespace UsersModule.Domain.Services.Security;

public interface IPasswordEncripter
{
    public string Encrypt(string password);
    public bool IsValid(string password, string passwordHash);
}