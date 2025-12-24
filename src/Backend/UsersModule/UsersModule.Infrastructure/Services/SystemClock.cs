using UsersModule.Domain.Extensions;
using UsersModule.Domain.Services;

namespace UsersModule.Infrastructure.Factories;

public class SystemClock : ISystemClock
{
    public DateTime UseCaseDate => DateTime.UtcNow.BrasiliaTz();
}