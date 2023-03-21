using System.Linq.Expressions;
using MarcketPlace.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace MarcketPlace.Infra.Extensions;

public static class ModelBuilderExtension
{
    public static void ApplyEntityConfiguration(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.GetEntities<IEntity>();
        var props = entities.SelectMany(c => c.GetProperties()).ToList();

        foreach (var property in props.Where(c => c.ClrType == typeof(int) && c.Name == "Id"))
        {
            property.IsKey();
        }
    }
    
    // public static void ApplyTrackingConfiguration(this ModelBuilder modelBuilder)
    // {
    //     var propDatas = new[] { "CriadoEm", "AtualizadoEm" };
    //     var propIds = new[] { "CriadoPor", "AtualizadoPor" };
    //     var propBools = new[] { "CriadoPorAdmin", "AtualizadoPorAdmin" };
    //     
    //     var entidades = modelBuilder.GetEntities<ITracking>();
    //
    //     var dataProps = entidades
    //         .SelectMany(c 
    //             => c.GetProperties().Where(p => p.ClrType == typeof(DateTime) && propDatas.Contains(p.Name)));
    //
    //     foreach (var prop in dataProps)
    //     {
    //         prop.SetColumnType("DATETIME");
    //         prop.SetDefaultValueSql("SYSDATETIME()");
    //     }
    //     
    //     var idProps = entidades
    //         .SelectMany(c 
    //             => c.GetProperties().Where(p => p.ClrType == typeof(int) && propIds.Contains(p.Name)));
    //     
    //     foreach (var prop in idProps)
    //     {
    //         prop.IsNullable = true;
    //     }
    //     
    //     var boolProps = entidades
    //         .SelectMany(c 
    //             => c.GetProperties().Where(p => p.ClrType == typeof(bool) && propBools.Contains(p.Name)));
    //     
    //     foreach (var prop in boolProps)
    //     {
    //         prop.SetDefaultValue(false);
    //         prop.IsNullable = false;
    //     }
    // }
    
    public static void ApplySoftDeleteConfiguration(this ModelBuilder modelBuilder)
    {
        var entidades = modelBuilder.GetEntities<ISoftDelete>();
        
        var props = entidades
            .SelectMany(c => c.GetProperties().Where(p => p.ClrType == typeof(bool))).ToList();
    
        foreach (var prop in props.Where(c => c.Name == "Desativado"))
        {
            prop.IsNullable = false;
            prop.SetDefaultValue(false);
        }
    }
    
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);
        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);    
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
        }
    }

    private static List<IMutableEntityType> GetEntities<T>(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(c => c.ClrType.GetInterface(typeof(T).Name) != null).ToList();

        return entities;
    }
}