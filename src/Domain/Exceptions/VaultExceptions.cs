namespace CipherLock.Domain.Exceptions;

public class VaultNotFoundException : Exception
{
    public VaultNotFoundException() : base() { }

    public VaultNotFoundException(string message) : base(message) { }

    public VaultNotFoundException(string message, Exception inner) : base(message, inner) { }
}
