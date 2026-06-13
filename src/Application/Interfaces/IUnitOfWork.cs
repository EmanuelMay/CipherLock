namespace CipherLock.Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
