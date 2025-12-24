using TasksModule.Domain.Services;
using UsersModule.Domain.Extensions;

namespace TasksModule.Infrastructure.Services;

internal sealed class SystemClock : ISystemClock
{
    public DateTime UseCaseDate => DateTime.UtcNow.BrasiliaTz();
}