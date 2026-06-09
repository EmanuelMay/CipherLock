namespace CipherLock.Domain.Entities;

public interface IEmailService
{
    public Task ResetPasswordEmailAsync(string email, string name, string code);
}
