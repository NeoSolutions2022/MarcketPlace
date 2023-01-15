namespace MarcketPlace.Core.Extensions;

public static class DateTimeExtensions
{
    // Convert datetime to UNIX time
    public static string ToUnixTime(this DateTime dateTime)
    {
        var dto = new DateTimeOffset(dateTime.ToUniversalTime());
        return dto.ToUnixTimeSeconds().ToString();
    }
 
    // Convert datetime to UNIX time including miliseconds
    public static string ToUnixTimeMilliSeconds(this DateTime dateTime)
    {
        var dto = new DateTimeOffset(dateTime.ToUniversalTime());
        return dto.ToUnixTimeMilliseconds().ToString();
    }
}