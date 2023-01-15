namespace MarcketPlace.Domain.Contracts;

public interface ISoftDelete
{
    public bool Desativado { get; set; }
}