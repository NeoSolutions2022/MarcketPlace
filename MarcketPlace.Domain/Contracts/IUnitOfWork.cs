namespace MarcketPlace.Domain.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}