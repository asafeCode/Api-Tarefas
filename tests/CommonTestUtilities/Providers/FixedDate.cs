using CommonTestUtilities.ValueObjects;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;

namespace CommonTestUtilities.Providers;

public class FixedDate : IDateProvider
{ 
    public DateOnly UseCaseToday => TarefasCrudTestsConstants.DateForTests.ToDateOnly();
}