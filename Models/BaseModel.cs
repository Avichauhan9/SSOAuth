
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

public class BaseModel
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public int? CreatedById { get; set; }
    [ForeignKey("CreatedById")]

    public virtual User? CreatedBy { get; set; }

    public DateTimeOffset LastUpdatedAt { get; set; } = DateTimeOffset.UtcNow;


    public int? UpdatedById { get; set; }
    [ForeignKey("UpdatedById")]

    public virtual User? UpdatedBy { get; set; }
}
