using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace MarcketPlace.Infra.Extensions;

public static class EntityTypeBuilderExtension
{
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
    {
        propertyBuilder
            .HasColumnType("json")
            .HasConversion(v
                => JsonConvert.SerializeObject(v), v 
                => JsonConvert.DeserializeObject<T>(v)!);
        
        return propertyBuilder;
    }
}