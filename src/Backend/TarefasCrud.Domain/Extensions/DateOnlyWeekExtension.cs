namespace TarefasCrud.Domain.Extensions;

public static class DateOnlyWeekExtension
{
    public static int GetMonthWeek(this DateOnly startDate)
    {
        var firstDayOfMonth = new DateOnly(startDate.Year, startDate.Month, 1);

        var offset = ((int)DayOfWeek.Monday - (int)firstDayOfMonth.DayOfWeek + 7) % 7;

        var firstMonday = firstDayOfMonth.AddDays(offset);
        
        var week = (startDate.DayNumber - firstMonday.DayNumber) / 7 + 1;

        return week;
    }
}