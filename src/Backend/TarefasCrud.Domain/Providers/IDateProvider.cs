using TarefasCrud.Domain.Extensions;

namespace TarefasCrud.Domain.Providers;

public interface IDateProvider
{
    public DateOnly UseCaseToday { get;}
    
}