using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;

namespace TarefasCrud.Infrastructure.Providers;

public class DateProvider : IDateProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateOnly UseCaseToday => UtcNow.BrasiliaTz().ToDateOnly();
}