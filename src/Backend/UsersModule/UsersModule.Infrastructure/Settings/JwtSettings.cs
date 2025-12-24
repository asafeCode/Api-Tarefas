namespace UsersModule.Infrastructure.Settings;

public sealed class JwtSettings 
{
    public uint ExpirationTimeMinutes { get; init; }
    public string SigningKey { get; init; } = null!;
}