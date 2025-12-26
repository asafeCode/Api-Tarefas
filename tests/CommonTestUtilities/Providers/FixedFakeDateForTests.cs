using CommonTestUtilities.ValueObjects;

namespace CommonTestUtilities.Providers;

public class FixedFakeDateForTests : ISystemClock
{ 
    public DateTime UseCaseDate => TarefasCrudTestsConstants.DateForTests;
}