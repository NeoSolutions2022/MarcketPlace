﻿using MarcketPlace.Domain.Contracts.Paginacao;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Contracts.Repositories;

public interface IClienteRepository : IRepository<Cliente>
{
    void Adicionar(Cliente cliente);
    void Alterar(Cliente cliente);
    Task<Cliente?> ObterPorId(int id);
    Task<Cliente?> ObterPorEmail(string email);
    Task<Cliente?> ObterPorCpf(string cpf);
    Task<IResultadoPaginado<Cliente>> Buscar(IBuscaPaginada<Cliente> filtro);
    void Remover(Cliente cliente);
}