
using Microsoft.Extensions.Options;
using SSO_Backend.Models.AppSettings;
using SSO_Backend.Models.Mailing;
using MimeKit;
using MimeKit.Text;
using System.Text;
using MailKit.Net.Smtp;
using Fluid;
using SSO_Backend.Context;


namespace SSO_Backend.Services;

public class EmailService(ILogger<EmailService> logger, IOptions<SmtpSettings> smtpSettings, AppDBContext _context) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private SmtpSettings _smtpSettings { get; set; } = smtpSettings.Value;

    public async Task SendEmailAsync(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        try
        {
            // create message
            var mimeMsg = new MimeMessage();

            var defaultFrom = MailboxAddress.Parse(_smtpSettings.Sender);

            if (string.IsNullOrWhiteSpace(email.From))
            {
                mimeMsg.From.Add(defaultFrom);
            }
            else
            {
                mimeMsg.From.Add(MailboxAddress.Parse(email.From));
            }

            AddEmails(mimeMsg.To, email.To);

            if (email.Cc != null && email.Cc.Count > 0)
                AddEmails(mimeMsg.Cc, email.Cc);
            if (email.Bcc != null && email.Bcc.Count > 0)
                AddEmails(mimeMsg.Bcc, email.Bcc);

            mimeMsg.Subject = email.Subject!.ToString();

            if (email.IsBodyHtml)
            {
                mimeMsg.Body = new TextPart(TextFormat.Html) { Text = email.Body!.ToString() };
            }
            else
            {
                mimeMsg.Body = new TextPart(TextFormat.Plain) { Text = email.Body!.ToString() };
            }
            // Attachments
            if (email.Attachments != null && email.Attachments.Count > 0)
            {
                foreach (var attachment in email.Attachments)
                {
                    var attachmentPart = new MimePart(attachment.ContentType)
                    {
                        Content = new MimeContent(new MemoryStream(attachment.Data), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = attachment.FileName
                    };

                    mimeMsg.Body = new Multipart("mixed") { mimeMsg.Body, attachmentPart };
                }
            }
            // send email
            var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.SmtpServer, _smtpSettings.Port, _smtpSettings.SSL);

            if (!string.IsNullOrWhiteSpace(_smtpSettings.UserName))
                await smtp.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);

            await smtp.SendAsync(mimeMsg);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Unexpected error while sending email.");
            throw;
        }
    }

    private static void AddEmails(InternetAddressList list, List<string> emails)
    {
        if (emails?.Count > 0)
        {
            list.AddRange(emails.Select(e => MailboxAddress.Parse(e)));
        }
    }

    public async Task SendEmailWithTemplateAsync(EmailTemplate emailTemplate, dynamic obj)
    {
        var email = new Email
        {
            Body = new StringBuilder(await emailTemplate!.Body.RenderAsync(obj)),
            Subject = new StringBuilder(await emailTemplate!.Subject.RenderAsync(obj))
        };

        if (emailTemplate.To?.Count > 0) email.To.AddRange(emailTemplate.To);
        if (emailTemplate.Cc?.Count > 0) email.Cc.AddRange(emailTemplate.Cc);
        if (emailTemplate.Bcc?.Count > 0) email.Bcc.AddRange(emailTemplate.Bcc);
        if (emailTemplate.Attachments?.Count > 0) email.Attachments.AddRange(emailTemplate.Attachments);


        email.IsBodyHtml = emailTemplate.IsBodyHtml;
        await SendEmailAsync(email);
    }

    public static EmailTemplate? GetFluidTemplate(string templateName, AppDBContext _context)
    {

        var notifyTemplate = _context.NotificationTemplates.SingleOrDefault(s => s.Name == templateName);

        if (notifyTemplate == null)
        {
            return null;
        }

        var parser = new FluidParser();
        if (parser.TryParse(notifyTemplate.Subject, out var subjectFluidTemplate, out var error) &&
            parser.TryParse(notifyTemplate.Body, out var bodyFluidTemplate, out var bodyerror))
        {
            var subjectLiquidTemplate = new FluidTemplateAdapter(subjectFluidTemplate);
            var bodyLiquidTemplate = new FluidTemplateAdapter(bodyFluidTemplate);

            var emailTemplate = new EmailTemplate
            {
                Subject = subjectLiquidTemplate,
                Body = bodyLiquidTemplate,
                To = notifyTemplate.To?.Split(',').ToList() ?? new List<string>(),
                Cc = notifyTemplate.Cc?.Split(',').ToList() ?? new List<string>(),
                Bcc = notifyTemplate.Bcc?.Split(',').ToList() ?? new List<string>(),
                Priority = notifyTemplate.Priority,
                IsBodyHtml = notifyTemplate.IsBodyHtml
            };

            return emailTemplate;
        }

        return null;
    }
}