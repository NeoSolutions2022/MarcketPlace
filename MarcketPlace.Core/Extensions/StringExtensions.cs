using System.Text;

namespace MarcketPlace.Core.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string? value, string valueToCompare)
    {
        return value is not null && value.Equals(valueToCompare, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool ContainsIgnoreCase(this string? value, string valueToCompare)
    {
        return value is not null && value.Contains(valueToCompare, StringComparison.InvariantCultureIgnoreCase);
    }
    
    public static string? SomenteNumeros(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) 
            ? null 
            : string.Join("", System.Text.RegularExpressions.Regex.Split(value, @"[^\d]"));
    }
    
    public static string FromBase64(this string? value)
    {
        var bytes = Convert.FromBase64String(value ?? string.Empty);
        return Encoding.UTF8.GetString(bytes);
    }
}