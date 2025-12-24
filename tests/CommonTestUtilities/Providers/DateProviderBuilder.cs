using Moq;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Factories;

namespace CommonTestUtilities.Providers;

public class DateProviderBuilder
{
    private readonly Mock<ISystemClock> _date = new();

    public DateProviderBuilder UseCaseToday(DateTime date)
    {
        _date.Setup(provider => provider.UseCaseDate).Returns(date);
        return this;
    }  
    public ISystemClock Build() => _date.Object;
}