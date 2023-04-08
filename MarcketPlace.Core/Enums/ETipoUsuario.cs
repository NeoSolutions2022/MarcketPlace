using System.ComponentModel;

namespace MarcketPlace.Core.Enums;

public enum ETipoUsuario
{
    [Description("Administrador")]
    Administrador = 1,
    [Description("Fornecedor")]
    Fornecedor = 2,
    [Description("Cliente")]
    Cliente = 3
}