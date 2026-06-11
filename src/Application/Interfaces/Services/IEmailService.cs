namespace CipherLock.Application.Interfaces.Services;

public interface IEmailService
{
    public Task ResetPasswordAsync(string email, string name, string code);

    public Task EmailAlreadyExistsAsync(string email, string name);

    public Task WelcomeConfirmAsync(string email, string name, string code);
}
