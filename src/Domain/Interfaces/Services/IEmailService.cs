namespace CipherLock.Domain.Entities;

public interface IEmailService
{
    public Task ResetPasswordEmail(string email, string name, string code);
}
