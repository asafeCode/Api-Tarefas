namespace CommonTestUtilities.Extensions;

public static class DatesExtensions
{
    public static DateOnly NextWeekday(this DateOnly currentDate, DayOfWeek targetDay)
    {
        var currentDayOfWeek = (int)currentDate.DayOfWeek;
        var targetDayOfWeek = (int)targetDay;
        
        var diff = targetDayOfWeek - currentDayOfWeek;
        
        if (diff <= 0)
            diff += 7;

        return currentDate.AddDays(diff);
    }    
    public static DateOnly PastWeekday(this DateOnly currentDate, DayOfWeek targetDay)
    {
        var currentDayOfWeek = (int)currentDate.DayOfWeek;
        var targetDayOfWeek = (int)targetDay;
        
        var diff = (targetDayOfWeek - currentDayOfWeek) - 7;
        
        return currentDate.AddDays(diff);
    }
    
    public static DateOnly ToDateOnly(this DateTime date) => DateOnly.FromDateTime(date);  
}