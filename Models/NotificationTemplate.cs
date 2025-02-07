
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("NotificationTemplates")]

public class NotificationTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }

    [Required]
    [StringLength(512)]
    public string Subject { get; set; }

    [Required]
    public string Body { get; set; }

    [StringLength(2000)]
    public string? To { get; set; }

    [StringLength(2000)]
    public string? Cc { get; set; }

    [StringLength(2000)]
    public string? Bcc { get; set; }

    [Required]
    public bool IsBodyHtml { get; set; } = true;

    public bool? Priority { get; set; }
}
