﻿using FluentValidation.Results;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Validation;

namespace MarcketPlace.Domain.Entities;

public class ProdutoServicoCaracteristica : EntityNotTracked, IAggregateRoot
{
    public string? Chave { get; set; }
    public string? Valor { get; set; }
    public int ProdutoServicoId { get; set; }
    
    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ProdutoServicoCaracteristicaValidator().Validate(this);
        return validationResult.IsValid;
    }
}