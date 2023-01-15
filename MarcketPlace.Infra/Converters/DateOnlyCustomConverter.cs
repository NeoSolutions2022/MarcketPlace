using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MarcketPlace.Infra.Converters;

public sealed class DateOnlyCustomConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyCustomConverter() 
        : base(d => d.ToDateTime(TimeOnly.MinValue), d => DateOnly.FromDateTime(d))
    { }
}