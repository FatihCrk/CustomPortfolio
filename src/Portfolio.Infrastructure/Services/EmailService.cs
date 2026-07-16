using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Application.Interfaces;

namespace Portfolio.Infrastructure.Services;

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_settings.SmtpServer))
        {
            _logger.LogWarning("SMTP ayarları yapılandırılmamış. E-posta gönderilmedi: {To}", to);
            return;
        }

        try
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            await client.SendMailAsync(message, cancellationToken);
            _logger.LogInformation("E-posta gönderildi: {To}, Konu: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "E-posta gönderimi başarısız: {To}, Hata: {Error}", to, ex.Message);
            throw;
        }
    }

    public async Task SendEmailFromTemplateAsync(string to, string templateName, object model, CancellationToken cancellationToken = default)
    {
        // Template sistemi implementasyonu
        // Razor view engine veya benzeri bir sistem kullanılabilir
        var body = await RenderTemplateAsync(templateName, model);
        await SendEmailAsync(to, GetTemplateSubject(templateName), body, cancellationToken);
    }

    public async Task SendPasswordResetEmailAsync(string to, string token, CancellationToken cancellationToken = default)
    {
        var resetLink = $"https://yourdomain.com/reset-password?token={Uri.EscapeDataString(token)}";
        var body = $@"
            <html>
                <body>
                    <h2>Şifre Sıfırlama Talebi</h2>
                    <p>Şifrenizi sıfırlamak için aşağıdaki linke tıklayın:</p>
                    <a href='{resetLink}'>{resetLink}</a>
                    <p>Bu link 24 saat geçerlidir.</p>
                    <p>Eğer bu talebi siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>
                </body>
            </html>";

        await SendEmailAsync(to, "Şifre Sıfırlama Talebi", body, cancellationToken);
    }

    public async Task SendWelcomeEmailAsync(string to, string username, CancellationToken cancellationToken = default)
    {
        var body = $@"
            <html>
                <body>
                    <h2>Hoş Geldiniz, {username}!</h2>
                    <p>Portfolio CMS'e hoş geldiniz.</p>
                    <p>Hesabınız başarıyla oluşturuldu.</p>
                </body>
            </html>";

        await SendEmailAsync(to, "Hoş Geldiniz", body, cancellationToken);
    }

    public async Task SendContactReplyEmailAsync(string to, string subject, string message, CancellationToken cancellationToken = default)
    {
        var body = $@"
            <html>
                <body>
                    <h2>İletişim Formu Cevabı</h2>
                    <p>Konu: {subject}</p>
                    <p>Mesaj: {message}</p>
                </body>
            </html>";

        await SendEmailAsync(to, $"Cevap: {subject}", body, cancellationToken);
    }

    private Task<string> RenderTemplateAsync(string templateName, object model)
    {
        // Template rendering implementasyonu
        return Task.FromResult($"<html><body>Template: {templateName}</body></html>");
    }

    private string GetTemplateSubject(string templateName)
    {
        return templateName switch
        {
            "PasswordReset" => "Şifre Sıfırlama",
            "Welcome" => "Hoş Geldiniz",
            "ContactReply" => "İletişim Formu Cevabı",
            _ => "Bildirim"
        };
    }
}
