using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Portfolio.Domain.Interfaces;
using Portfolio.Shared.DTOs.Email;

namespace Portfolio.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>() ?? new SmtpSettings();
    }

    public async Task SendEmailAsync(EmailRequestDto request)
    {
        if (!_smtpSettings.IsEnabled)
        {
            _logger.LogWarning("SMTP is disabled. Email not sent to {Email}", request.To);
            return;
        }

        try
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = request.IsHtml
            };

            mailMessage.To.Add(request.To);
            
            if (!string.IsNullOrEmpty(request.Cc))
                mailMessage.CC.Add(request.Cc);

            await client.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Email sent successfully to {Email} with subject: {Subject}", request.To, request.Subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}. Error: {Error}", request.To, ex.Message);
            throw;
        }
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetLink)
    {
        var subject = "Şifre Sıfırlama İsteği - Portfolio CMS";
        var body = $@"
            <html>
            <body>
                <h2>Şifre Sıfırlama</h2>
                <p>Merhaba,</p>
                <p>Şifrenizi sıfırmak için aşağıdaki linke tıklayınız:</p>
                <a href='{resetLink}'>Şifremi Sıfırla</a>
                <p>Bu link 1 saat süreyle geçerlidir.</p>
                <p>Eğer bu isteği siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>
                <hr/>
                <p><small>Portfolio CMS - Otomatik Bildirim</small></p>
            </body>
            </html>";

        await SendEmailAsync(new EmailRequestDto
        {
            To = email,
            Subject = subject,
            Body = body,
            IsHtml = true
        });
    }

    public async Task SendContactFormNotificationAsync(Domain.Entities.ContactMessage message)
    {
        var subject = $"Yeni İletişim Mesajı: {message.Subject}";
        var body = $@"
            <html>
            <body>
                <h2>Yeni İletişim Mesajı</h2>
                <table border='1' cellpadding='5'>
                    <tr><td><strong>Ad Soyad:</strong></td><td>{message.Name}</td></tr>
                    <tr><td><strong>E-Posta:</strong></td><td>{message.Email}</td></tr>
                    <tr><td><strong>Telefon:</strong></td><td>{message.Phone ?? "Belirtilmemiş"}</td></tr>
                    <tr><td><strong>Konu:</strong></td><td>{message.Subject}</td></tr>
                    <tr><td><strong>Mesaj:</strong></td><td>{message.MessageBody}</td></tr>
                    <tr><td><strong>IP Adresi:</strong></td><td>{message.IpAddress}</td></tr>
                    <tr><td><strong>Tarih:</strong></td><td>{message.CreatedDate:dd.MM.yyyy HH:mm}</td></tr>
                </table>
                <hr/>
                <p><small>Portfolio CMS - Otomatik Bildirim</small></p>
            </body>
            </html>";

        // Send to admin email configured in settings
        var adminEmail = _configuration["AdminEmail"];
        if (!string.IsNullOrEmpty(adminEmail))
        {
            await SendEmailAsync(new EmailRequestDto
            {
                To = adminEmail,
                Subject = subject,
                Body = body,
                IsHtml = true
            });
        }
    }
}
