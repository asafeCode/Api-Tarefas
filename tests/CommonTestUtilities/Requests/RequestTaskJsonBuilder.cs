using Bogus;
using CommonTestUtilities.Extensions;
using TarefasCrud.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestTaskJsonBuilder
{
    public static RequestTaskJson Build(int weeklyGoal = 1, int descriptionChar = 20, DayOfWeek targetDay = DayOfWeek.Monday) => new Faker<RequestTaskJson>()
        .RuleFor(t => t.Title, f => f.Random.Word())
        .RuleFor(t => t.Description, f => f.Random.String(descriptionChar))
        .RuleFor(t => t.Category, f => f.Random.Word())
        .RuleFor(t => t.WeeklyGoal, f => weeklyGoal)
        .RuleFor(t => t.StartDate, f => GetFutureStartDate(f.Date.Future(0).ToDateOnly(), targetDay));
    
    private static DateOnly GetFutureStartDate(DateOnly currentDate, DayOfWeek targetDay)
    {
        return currentDate.NextWeekday(targetDay);
    }
}