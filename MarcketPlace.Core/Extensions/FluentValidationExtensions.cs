using FluentValidation;

namespace MarcketPlace.Core.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TY> AddMessagePrefix<T, TY>(this IRuleBuilderOptions<T, TY> ruleBuilderOptions, string? prefix)
    {
        ruleBuilderOptions.Configure(configurator =>
        {
            configurator.MessageBuilder = ctx 
                => string.IsNullOrWhiteSpace(prefix) 
                    ? ctx.GetDefaultMessage() 
                    : $"{prefix}: {ctx.GetDefaultMessage()}";
        });

        return ruleBuilderOptions;
    }
}