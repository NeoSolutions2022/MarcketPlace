using AutoMapper;
using MarcketPlace.Application.Dtos.V1.Base;
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
    }
}