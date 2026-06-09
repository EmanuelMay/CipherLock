using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;
using MailKit.Net.Smtp;
using MimeKit;

namespace CipherLock.Application.Services;

public class EmailService(
    IConfiguration configuration,
    IWebHostEnvironment environment
) : IEmailService
{
    public async Task ResetPasswordEmailAsync(string email, string name, string code)
    {
        var path = Path.Combine(
            environment.ContentRootPath,
            "Templates",
            "ResetPassword.html"
        );

        var address = configuration["Email:Address"]
            ?? throw new EmailNotConfiguredException("address is not configured");
        var password = configuration["Email:Password"]
            ?? throw new EmailNotConfiguredException("password is not configured");

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
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync(address, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
