using System.Data;
using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class FornecedorValidator : AbstractValidator<Fornecedor>
{
    public FornecedorValidator()
    {
        RuleFor(u => u.Bairro)
            .MinimumLength(3)
            .WithMessage("Bairro deve ter no mínimo 3 caracteres!")
            .MaximumLength(30)
            .WithMessage("Bairro deve ter no máximo 30 caracteres!")
            .NotEmpty()
            .WithMessage("Bairro não pode ser vazio!");

        RuleFor(c => c.Categoria)
            .NotNull()
            .WithMessage("Categoria não pode ser nula")
            .NotEmpty()
            .WithMessage("Categoria deve conter ao menos um item");

        RuleFor(u => u.Cep)
            .MinimumLength(8)
            .WithMessage("Cep deve ter no mínimo 8 caracteres!")
            .MaximumLength(9)
            .WithMessage("Cep deve ter no máximo 9 caracteres!")
            .NotEmpty()
            .WithMessage("Cep não pode ser vazio!");
        
        RuleFor(u => u.Cidade)
            .MinimumLength(2)
            .WithMessage("Cidade deve ter no mínimo 2 caracteres!")
            .MaximumLength(60)
            .WithMessage("Cidade deve ter no máximo 60 caracteres!")
            .NotEmpty()
            .WithMessage("Cidade não pode ser vazio!");
        
        RuleFor(u => u.Cnpj)
            .IsValidCNPJ()
            .WithMessage("Cnpj deve ser válido!")
            .NotEmpty()
            .WithMessage("Cnpj não pode ser vazio!");
        
        RuleFor(u => u.Complemento)
            .MinimumLength(2)
            .WithMessage("Complemento deve ter no mínimo 2 caracteres!")
            .MaximumLength(60)
            .WithMessage("Complemento deve ter no máximo 60 caracteres!");
        
        RuleFor(u => u.Cpf)
            .IsValidCPF()
            .NotEmpty()
            .WithMessage("Cpf não pode ser vazio!");

        RuleFor(u => u.Desativado)
            .NotNull();
        
        RuleFor(u => u.Email)
            .MinimumLength(11)
            .WithMessage("Email deve ter no mínimo 11 caracteres!")
            .MaximumLength(60)
            .WithMessage("Email deve ter no máximo 60 caracteres!")
            .NotEmpty()
            .WithMessage("Email não pode ser vazio!")
            .EmailAddress();
        
        RuleFor(u => u.Endereco)
            .MinimumLength(2)
            .WithMessage("Endereco deve ter no mínimo 2 caracteres!")
            .MaximumLength(60)
            .WithMessage("Endereco deve ter no máximo 60 caracteres!")
            .NotEmpty()
            .WithMessage("Endereco não pode ser vazio!");
        
        RuleFor(u => u.Nome)
            .MinimumLength(2)
            .WithMessage("Nome deve ter no mínimo 2 caracteres!")
            .MaximumLength(60)
            .WithMessage("Nome deve ter no máximo 60 caracteres!")
            .NotEmpty()
            .WithMessage("Nome não pode ser vazio!");
        
        RuleFor(u => u.Numero)
            .NotEmpty()
            .WithMessage("Numero não pode ser vazio!")
            .NotNull()
            .WithMessage("Numero não pode ser null!");
        
        RuleFor(u => u.Responsavel)
            .MinimumLength(2)
            .WithMessage("Responsavel deve ter no mínimo 2 caracteres!")
            .MaximumLength(60)
            .WithMessage("Responsavel deve ter no máximo 60 caracteres!")
            .NotEmpty()
            .WithMessage("Responsavel não pode ser vazio!");

        RuleFor(u => u.Senha)
            .MinimumLength(8)
            .WithMessage("Senha deve ter no mínimo 8 caracteres!")
            .NotEmpty()
            .WithMessage("Senha não pode ser vazio!");
        
        RuleFor(u => u.Telefone)
            .MinimumLength(9)
            .WithMessage("Telefone deve ter no mínimo 9 caracteres!")
            .MaximumLength(16)
            .WithMessage("Telefone deve ter no máximo 16 caracteres!");

        RuleFor(u => u.Uf)
            .Length(2, 2)
            .WithMessage("UF deve possuir 2 caracteres!")
            .NotEmpty()
            .WithMessage("UF não pode ser vazio!");
    }
}