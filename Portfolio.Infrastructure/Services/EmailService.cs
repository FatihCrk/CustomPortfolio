using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

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
        
        _smtpSettings = new SmtpSettings();
        _configuration.GetSection("SmtpSettings").Bind(_smtpSettings);
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        if (string.IsNullOrEmpty(_smtpSettings.Host))
        {
            _logger.LogWarning("SMTP not configured. Email not sent to: {To}, Subject: {Subject}", to, subject);
            return; // Don't throw, just log warning
        }

        try
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml,
                BodyEncoding = Encoding.UTF8
            };

            message.To.Add(to);

            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            await client.SendMailAsync(message);
            
            _logger.LogInformation("Email sent successfully to: {To}, Subject: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to: {To}, Subject: {Subject}", to, subject);
            throw;
        }
    }

    public async Task SendPasswordResetAsync(string to, string email, string resetToken)
    {
        var resetLink = $"{_smtpSettings.BaseUrl}/reset-password?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(email)}";
        
        var subject = "Şifre Sıfırlama İsteği | Password Reset Request";
        
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .button {{ display: inline-block; padding: 12px 24px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Şifre Sıfırlama İsteği</h2>
        <p>Merhaba,</p>
        <p>Hesabınız için şifre sıfırlama isteği aldık. Aşağıdaki butona tıklayarak şifrenizi sıfırlayabilirsiniz:</p>
        <p style='text-align: center;'>
            <a href='{resetLink}' class='button'>Şifremi Sıfırla</a>
        </p>
        <p>veya aşağıdaki bağlantıyı kopyalayıp tarayıcınıza yapıştırın:</p>
        <p style='word-break: break-all;'><a href='{resetLink}'>{resetLink}</a></p>
        <p><strong>Önemli:</strong> Bu bağlantı 1 saat sonra geçersiz olacaktır.</p>
        <p>Eğer bu isteği siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>
        
        <hr>
        
        <h2>Password Reset Request</h2>
        <p>Hello,</p>
        <p>We received a password reset request for your account. Click the button below to reset your password:</p>
        <p style='text-align: center;'>
            <a href='{resetLink}' class='button'>Reset Password</a>
        </p>
        <p><strong>Note:</strong> This link will expire in 1 hour.</p>
        <p>If you didn't request this, you can safely ignore this email.</p>
        
        <div class='footer'>
            <p>This is an automated message, please do not reply.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(to, subject, body, true);
    }

    public async Task SendContactNotificationAsync(string adminEmail, MessageDto message)
    {
        var subject = $"Yeni İletişim Mesajı | New Contact Message - {message.Name}";
        
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .info-box {{ background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 15px 0; }}
        .label {{ font-weight: bold; color: #555; }}
        .footer {{ margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>📬 Yeni İletişim Mesajı</h2>
        
        <div class='info-box'>
            <p><span class='label'>Gönderen / From:</span> {message.Name}</p>
            <p><span class='label'>E-posta / Email:</span> {message.Email}</p>
            {(message.Phone != null ? $"<p><span class='label'>Telefon / Phone:</span> {message.Phone}</p>" : "")}
            <p><span class='label'>Konu / Subject:</span> {message.Subject}</p>
        </div>
        
        <h3>Mesaj / Message:</h3>
        <p style='white-space: pre-wrap;'>{message.Content}</p>
        
        <div class='info-box'>
            <p><span class='label'>IP Adresi:</span> {message.IpAddress}</p>
            <p><span class='label'>Tarayıcı:</span> {message.UserAgent}</p>
            <p><span class='label'>Tarih / Date:</span> {message.CreatedDate:dd.MM.yyyy HH:mm}</p>
        </div>
        
        <p style='text-align: center; margin-top: 30px;'>
            <a href='{_smtpSettings.BaseUrl}/admin/messages' style='display: inline-block; padding: 12px 24px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>
                Admin Panelinde Görüntüle
            </a>
        </p>
        
        <div class='footer'>
            <p>Portfolio CMS - Otomatik Bildirim</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(adminEmail, subject, body, true);
    }
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Portfolio CMS";
    public bool EnableSsl { get; set; } = true;
    public string BaseUrl { get; set; } = "http://localhost:4200";
}
