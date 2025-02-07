

using SSO_Backend.Models.Mailing;

namespace SSO_Backend.Services;

public interface IEmailService
{
    Task SendEmailAsync(Email email);
    Task SendEmailWithTemplateAsync(EmailTemplate emailTemplate, dynamic obj);
}
