using CipherLock.Application.Interfaces.Services;
using CipherLock.Domain.Exceptions;
using MailKit.Net.Smtp;
using MimeKit;

namespace CipherLock.Infrastructure.Services;

public class EmailService(
    IConfiguration configuration,
    IWebHostEnvironment environment
) : IEmailService
{
    private readonly string address = configuration["Email:Address"]
            ?? throw new EmailNotConfiguredException("address is not configured");

    private readonly string password = configuration["Email:Password"]
            ?? throw new EmailNotConfiguredException("password is not configured");

    public async Task ResetPasswordAsync(string email, string name, string code)
    {
        var path = Path.Combine(
            environment.ContentRootPath,
            "Templates",
            "ResetPassword.html"
        );

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CipherLock", address));
        message.To.Add(new MailboxAddress(name, email));
        message.Subject = "Password Reset";

        var html = await File.ReadAllTextAsync(path);

        html = html.Replace("{{NAME}}", name);
        html = html.Replace("{{CODE}}", code);
        html = html.Replace("{{EMAIL}}", email);

        message.Body = new TextPart("html")
        {
            Text = html
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync(address, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public async Task EmailAlreadyExistsAsync(string email, string name)
    {
        var path = Path.Combine(
            environment.ContentRootPath,
            "Templates",
            "EmailAlreadyExists.html"
        );

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Cipherlock", address));
        message.To.Add(new MailboxAddress(name, email));
        message.Subject = "Email Already Exists";

        var html = await File.ReadAllTextAsync(path);

        html = html.Replace("{{NAME}}", name);
        html = html.Replace("{{EMAIL}}", email);

        message.Body = new TextPart("html")
        {
            Text = html
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync(address, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public async Task WelcomeConfirmAsync(string email, string name, string code)
    {
        var path = Path.Combine(
            environment.ContentRootPath,
            "Templates",
            "WelcomeConfirm.html"
        );

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Cipherlock", address));
        message.To.Add(new MailboxAddress(name, email));
        message.Subject = "Welcome Confirm";

        var html = await File.ReadAllTextAsync(path);

        html = html.Replace("{{NAME}}", name);
        html = html.Replace("{{EMAIL}}", email);
        html = html.Replace("{{CODE}}", code);

        message.Body = new TextPart("html")
        {
            Text = html
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync(address, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
