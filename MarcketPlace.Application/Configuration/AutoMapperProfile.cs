using AutoMapper;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Domain.Entities;
using MarcketPlace.Domain.Paginacao;

namespace MarcketPlace.Application.Configuration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Administrador

        CreateMap<MarcketPlace.Application.Dtos.V1.Administrador.AdministradorDto, Administrador>().ReverseMap();
        CreateMap<MarcketPlace.Application.Dtos.V1.Administrador.AdicionarAdministradorDto, Administrador>().ReverseMap();
        CreateMap<MarcketPlace.Application.Dtos.V1.Administrador.AlterarAdministradorDto, Administrador>().ReverseMap();
        CreateMap<PagedDto<MarcketPlace.Application.Dtos.V1.Administrador.BuscarAdministradorDto>, ResultadoPaginado<Administrador>>().ReverseMap();

        #endregion

        #region Cliente

        CreateMap<MarcketPlace.Application.Dtos.V1.Cliente.ClienteDto, Cliente>()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros())
            .AfterMap((_, dest) => dest.Telefone = dest.Telefone.SomenteNumeros())
            .ReverseMap();
        
        CreateMap<MarcketPlace.Application.Dtos.V1.Cliente.AlterarClienteDto, Cliente>()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros())
            .AfterMap((_, dest) => dest.Telefone = dest.Telefone.SomenteNumeros())
            .ReverseMap();
        
        CreateMap<MarcketPlace.Application.Dtos.V1.Cliente.CadastrarClienteDto, Cliente>()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros())
            .AfterMap((_, dest) => dest.Telefone = dest.Telefone.SomenteNumeros())
            .ReverseMap();
        
        CreateMap<MarcketPlace.Application.Dtos.V1.Cliente.BuscarClienteDto, ResultadoPaginado<Cliente>>().ReverseMap();

        #endregion
        
        #region Fornecedor

        CreateMap<MarcketPlace.Application.Dtos.V1.Fornecedor.FornecedorDto, Fornecedor>()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros())
            .AfterMap((_, dest) => dest.Cnpj = dest.Cnpj.SomenteNumeros())
            .AfterMap((_, dest) => dest.Telefone = dest.Telefone.SomenteNumeros())
            .ReverseMap();
        
        CreateMap<MarcketPlace.Application.Dtos.V1.Fornecedor.AlterarFornecedorDto, Fornecedor>()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros())
            .AfterMap((_, dest) => dest.Cnpj = dest.Cnpj.SomenteNumeros())
            .AfterMap((_, dest) => dest.Telefone = dest.Telefone.SomenteNumeros())
            .ReverseMap();
        
        CreateMap<MarcketPlace.Application.Dtos.V1.Fornecedor.CadastrarFornecedorDto, Fornecedor>()
            .AfterMap((_, dest) => dest.Cpf = dest.Cpf.SomenteNumeros())
            .AfterMap((_, dest) => dest.Cnpj = dest.Cnpj.SomenteNumeros())
            .AfterMap((_, dest) => dest.Telefone = dest.Telefone.SomenteNumeros())
            .ReverseMap();
        
        CreateMap<MarcketPlace.Application.Dtos.V1.Fornecedor.BuscarFornecedorDto, ResultadoPaginado<Fornecedor>>().ReverseMap();

        #endregion
    }
}