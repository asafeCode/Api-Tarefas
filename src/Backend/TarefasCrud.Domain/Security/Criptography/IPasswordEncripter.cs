namespace TarefasCrud.Domain.Security.Criptography;

public interface IPasswordEncripter
{
    public string Encrypt(string password);
}