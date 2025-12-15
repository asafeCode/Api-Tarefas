using TarefasCrud.Domain.Dtos;

namespace CommonTestUtilities.Dtos;

public static class FilterTasksDtoBuilder
{
    public static string BuildQuery(FilterTasksDto filter)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query.Add($"title={Uri.EscapeDataString(filter.Title)}");

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query.Add($"category={Uri.EscapeDataString(filter.Category)}");

        if (filter.IsCompleted.HasValue)
            query.Add($"isCompleted={filter.IsCompleted.Value.ToString().ToLower()}");

        if (filter.WeeklyGoalMin.HasValue)
            query.Add($"weeklyGoalMin={filter.WeeklyGoalMin}");

        if (filter.WeeklyGoalMax.HasValue)
            query.Add($"weeklyGoalMax={filter.WeeklyGoalMax}");

        if (filter.ProgressMin.HasValue)
            query.Add($"progressMin={filter.ProgressMin}");

        if (filter.ProgressMax.HasValue)
            query.Add($"progressMax={filter.ProgressMax}");

        if (filter.WeekOfMonth.HasValue)
            query.Add($"weekOfMonth={filter.WeekOfMonth}");

        if (filter.Month.HasValue)
            query.Add($"month={filter.Month}");

        if (filter.Year.HasValue)
            query.Add($"year={filter.Year}");

        return query.Count == 0
            ? string.Empty
            : "?" + string.Join("&", query);
    }
}