using CommonTestUtilities.ValueObjects;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Factories;

namespace CommonTestUtilities.Providers;

public class FixedFakeDateForTests : ISystemClock
{ 
    public DateTime UseCaseDate => TarefasCrudTestsConstants.DateForTests;
}