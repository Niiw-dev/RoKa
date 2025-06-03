using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MimeKit;
using RoKa.Hubs;

namespace RoKa.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message, string fromName, string fromEmail);
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;
    private readonly IHubContext<EmailNotificationHub> _hubContext;

    public EmailService(
        IOptions<EmailSettings> emailSettings,
        ILogger<EmailService> logger,
        IHubContext<EmailNotificationHub> hubContext)  // Inyecta el HubContext
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message, string fromName, string fromEmail)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            email.To.Add(new MailboxAddress("", toEmail));
            email.ReplyTo.Add(new MailboxAddress(fromName, fromEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = message
            };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email enviado exitosamente a {ToEmail}", toEmail);

            // Aquí notificas a todos los clientes conectados al Hub
            await _hubContext.Clients.All.SendAsync("ReceiveEmailNotification", $"Correo enviado a {toEmail} con éxito");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando email a {ToEmail}", toEmail);
            throw;
        }
    }
}

public class EmailSettings
{
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }
    public string FromEmail { get; set; } = "";
    public string FromName { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
