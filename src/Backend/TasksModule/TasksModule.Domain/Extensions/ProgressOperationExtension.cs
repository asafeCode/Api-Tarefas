using TasksModule.Domain.Enums;

namespace TasksModule.Domain.Extensions;

public static class ProgressOperationExtension
{
    public static int ToInt(this ProgressOperation enumValue) => (int)enumValue;
}