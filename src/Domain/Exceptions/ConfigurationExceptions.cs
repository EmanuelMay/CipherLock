namespace CipherLock.Domain.Exceptions;

public class JwtKeyException : Exception
{
    public JwtKeyException() : base() { }

    public JwtKeyException(string message) : base(message) { }

    public JwtKeyException(string message, Exception inner) : base(message, inner) { }
}

public class EmailNotConfiguredException : Exception
{
    public EmailNotConfiguredException() : base() { }

    public EmailNotConfiguredException(string message) : base(message) { }

    public EmailNotConfiguredException(string message, Exception inner) : base(message, inner) { }
}

public class ConnectionStringException : Exception
{
    public ConnectionStringException() : base() { }

    public ConnectionStringException(string message) : base(message) { }

    public ConnectionStringException(string message, Exception inner) : base(message, inner) { }
}
