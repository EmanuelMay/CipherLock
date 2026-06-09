namespace CipherLock.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base() { }

    public UserNotFoundException(string message) : base(message) { }

    public UserNotFoundException(string message, Exception inner) : base(message, inner) { }
}

public class InvalidPasswordLengthException : Exception
{
    public InvalidPasswordLengthException() : base() { }

    public InvalidPasswordLengthException(string message) : base(message) { }

    public InvalidPasswordLengthException(string message, Exception inner) : base (message, inner) { }
}

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base() { }

    public InvalidCredentialsException(string message) : base(message) { }

    public InvalidCredentialsException(string message, Exception inner) : base(message, inner) { }
}
