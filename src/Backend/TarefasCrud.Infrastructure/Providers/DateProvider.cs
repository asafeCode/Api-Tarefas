using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;

namespace TarefasCrud.Infrastructure.Providers;

public class DateProvider : IDateProvider
{
    public DateTime UseCaseDate => DateTime.UtcNow.BrasiliaTz();
}