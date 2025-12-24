namespace UsersModule.Infrastructure.Settings;

public sealed record JwtSettings 
{
    public uint ExpirationTimeMinutes { get; init; }
    public string SigningKey { get; init; } = null!;
}