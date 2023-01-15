using System.ComponentModel;

namespace MarcketPlace.Core.Extensions;

public static class EnumExtensions
{
    public static string ToDescriptionString(this Enum val)
    {
        var attibutes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString())
            ?.GetCustomAttributes(typeof(DescriptionAttribute), false)!;
        return attibutes.Length > 0 ? attibutes[0].Description : string.Empty;
    }
}