using Microsoft.AspNetCore.Identity.UI.Services;

namespace Company.Presentation.Services;

/// <summary>
/// برای توسعه: ایمیل‌ها در لاگ نوشته می‌شوند. در تولید SMTP یا ارائه‌دهنده واقعی را جایگزین کنید.
/// </summary>
public sealed class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation(
            "Email to {Email}\nSubject: {Subject}\nBody:\n{Body}",
            email,
            subject,
            htmlMessage);

        return Task.CompletedTask;
    }
}
