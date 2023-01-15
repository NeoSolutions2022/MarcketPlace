using System.Text.RegularExpressions;
using FluentValidation;

namespace MarcketPlace.Core.Utils;

public class PasswordValidator : AbstractValidator<string>
{
    public const string MsgErroMinCaracteresTemplate =
        "A senha deve ser maior ou igual a {MinLength} caracteres. Você digitou {TotalLength} caracteres.";
    public const string MsgErroMaiuscula = "A senha deve conter letra(s) maiúscula(s).";
    public const string MsgErroMinuscula = "A senha deve conter letra(s) minúscula(s).";
    public const string MsgErroNumero = "A senha deve conter número(s).";
    public const string MsgErroCaracterEspecial = "A senha deve conter ao menos um caracterer especial.";
    
    private static readonly Regex MaiusculaRegex = new("[A-Z]", RegexOptions.Compiled);
    private static readonly Regex MinusculaRegex = new("[a-z]", RegexOptions.Compiled);
    private static readonly Regex NumeroRegex = new("[0-9]", RegexOptions.Compiled);
    private static readonly Regex CaracterEspecialRegex = new("[^a-zA-Z0-9]", RegexOptions.Compiled);

    public PasswordValidator(int minCaracteres = 8, bool requerMaiusculas = true, bool requerMinusculas = true, bool requerNumeros = true, bool requerCaracteresEspeciais = true)
    {
        if (minCaracteres > 0)
        {
            RuleFor(c => c)
                .MinimumLength(minCaracteres)
                .WithMessage(MsgErroMinCaracteresTemplate)
                .WithName("senha");
        }
        
        if (requerMaiusculas)
        {
            RuleFor(c => c)
                .Must(str => MaiusculaRegex.Match(str).Success)
                .WithMessage(MsgErroMaiuscula);
        }

        if (requerMinusculas)
        {
            RuleFor(c => c)
                .Must(str => MinusculaRegex.Match(str).Success)
                .WithMessage(MsgErroMinuscula);
        }

        if (requerNumeros)
        {
            RuleFor(c => c)
                .Must(str => NumeroRegex.Match(str).Success)
                .WithMessage(MsgErroNumero);
        }

        if (requerCaracteresEspeciais)
        {
            RuleFor(c => c)
                .Must(str => CaracterEspecialRegex.Match(str).Success)
                .WithMessage(MsgErroCaracterEspecial);
        }
    }
}