using System.ComponentModel;

namespace MarcketPlace.Core.Enums;

public enum ETipoUsuario
{
    [Description("Administrador")]
    Administrador = 1,
    [Description("Comum")]
    Comum = 2
}