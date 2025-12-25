using UsersModule.Domain.Services.Security;

namespace CommonTestUtilities.Cryptography;

public static class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new BcryptEncripter();
}