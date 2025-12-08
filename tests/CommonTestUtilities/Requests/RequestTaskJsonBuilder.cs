using Bogus;
using TarefasCrud.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestTaskJsonBuilder
{
    public static RequestTaskJson Build(int weeklyGoal = 2, int descriptionChar = 20) => new Faker<RequestTaskJson>()
        .RuleFor(t => t.Title, f => f.Random.Word())
        .RuleFor(t => t.Description, f => f.Random.String(descriptionChar))
        .RuleFor(t => t.Category, f => f.Random.Word())
        .RuleFor(t => t.WeeklyGoal, f => weeklyGoal)
        .RuleFor(t => t.StartDate, f => f.Date.Future(0)
            .NextMondayToWednesday().ToDateOnly());
    private static DateTime NextMondayToWednesday(this DateTime date)
    {
        if (date.Date <= DateTime.Today)
            date = date.AddDays(1);
        
        while (date.DayOfWeek is not (DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday))
        {
            date = date.AddDays(1);
        }
        return date;
    }
    private static DateOnly ToDateOnly(this DateTime date) => DateOnly.FromDateTime(date);    
}