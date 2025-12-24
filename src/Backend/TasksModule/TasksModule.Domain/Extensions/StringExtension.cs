using System.Diagnostics.CodeAnalysis;

namespace TasksModule.Domain.Extensions;

public static class StringExtension
{
    public static bool NotEmpty([NotNullWhen(true)]this string? value) => 
        string.IsNullOrWhiteSpace(value).IsFalse();
    
}