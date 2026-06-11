namespace CipherLock.Application.Interfaces.Services;

public interface IEmailService
{
    public Task ResetPasswordEmailAsync(string email, string name, string code);
}
