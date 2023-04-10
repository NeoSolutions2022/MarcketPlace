using System.ComponentModel;

namespace MarcketPlace.Domain.Entities.Enum;

public enum ECategoriaTipo
{
    [Description("Imóveis")]
    Imoveis = 1,
    [Description("Auto e peças")]
    AutoEPecas = 2,
    [Description("Para a sua casa")]
    ParaASuaCasa = 3,
    [Description("Eletrônicos e celulares")]
    EletronicosECelurares = 4,
    [Description("Música e hobbies")]
    MusicaEHobbies = 5,
    [Description("Esportes e lazer")]
    EsportesELazer = 6,
    [Description("Artigos e infantís")]
    ArtigosEInfantis = 7,
    [Description("Animais e estimção")]
    AnimaisEEstimacao = 8,
    [Description("Moda e beleza")]
    ModaEBeleza = 9,
    [Description("Agro e indústria")]
    AgroEIndustria = 10,
    [Description("Comércio e escritório")]
    ComercioEEscritorio = 11,
    [Description("Vagas de emprego")]
    VagasDeEmprego = 12
}