namespace CipherLock.Domain.Exceptions;

public class CredentialNotFoundException : Exception
{
    public CredentialNotFoundException() : base() { }

    public CredentialNotFoundException(string message) : base(message) { }

    public CredentialNotFoundException(string message, Exception inner) : base(message, inner) { }
}
