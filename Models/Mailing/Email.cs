using System.Text;

namespace SSO_Backend.Models.Mailing;

public class Email
{
    public string From { get; set; } = null!;
    public List<string> To { get; set; } = null!;
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public bool IsBodyHtml { get; set; }
    public StringBuilder? Subject { get; set; }
    public StringBuilder? Body { get; set; }
    public Email()
    {
        To = [];
        Cc = [];
        Bcc = [];
        Attachments = [];
    }
    public bool? Priority { get; set; }
    public List<Attachment> Attachments { get; set; }
}
